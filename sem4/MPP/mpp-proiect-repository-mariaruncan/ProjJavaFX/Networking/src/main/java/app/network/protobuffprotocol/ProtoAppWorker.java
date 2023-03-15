package app.network.protobuffprotocol;

import app.model.Employee;
import app.model.Game;
import app.model.dto.DtoUtils;
import app.model.dto.EmployeeDto;
import app.model.dto.TicketPurchaseDto;
import app.services.IObserver;
import app.services.IServices;
import app.services.MyException;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.List;

public class ProtoAppWorker implements Runnable, IObserver {
    private IServices server;
    private Socket connection;

    private InputStream input;
    private OutputStream output;
    private volatile boolean connected;

    public ProtoAppWorker(IServices server, Socket connection) {
        this.server = server;
        this.connection = connection;
        try{
            input = connection.getInputStream();
            output = connection.getOutputStream();
            connected = true;
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void updateGame(Game game) {
        System.out.println("Notify employees in worker, game updates " + game);
        try {
            sendResponse(ProtoUtils.createGameResponse(game));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void run() {
        while(connected){
            try {
                System.out.println("Waiting requests...");
                AppProtobuffs.Request request = AppProtobuffs.Request.parseDelimitedFrom(input);
                System.out.println("Request received: " + request);
                AppProtobuffs.Response response = handleRequest(request);
                if(response != null){
                    sendResponse(response);
                }
            } catch (IOException e) {
                e.printStackTrace();
            }
            try{
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
        try{
            input.close();
            output.close();
            connection.close();
        } catch (IOException e) {
            System.out.println("Error " + e);
        }
    }

    private AppProtobuffs.Response handleRequest(AppProtobuffs.Request request) {
        AppProtobuffs.Response response = null;
        switch (request.getType()){
            case LOGIN: {
                System.out.println("Login request...");
                EmployeeDto empDto = ProtoUtils.getEmpDto(request);
                try{
                    server.logIn(empDto.getUsername(), empDto.getPassword(), this);
                    return ProtoUtils.createOkResponse();
                } catch (MyException e) {
                    connected = false;
                    return ProtoUtils.createErrorResponse(e.getMessage());
                }
            }
            case LOGOUT:{
                System.out.println("Log out request...");
                Employee emp = DtoUtils.getFromDTO(ProtoUtils.getEmpDto(request));
                try{
                    server.logOut(emp, this);
                    connected = false;
                    return ProtoUtils.createOkResponse();
                } catch (MyException e) {
                    return ProtoUtils.createErrorResponse(e.getMessage());
                }
            }
            case PURCHASE: {
                System.out.println("Purchase request...");
                TicketPurchaseDto tpDto = ProtoUtils.getTicketPurchaseDto(request);
                try {
                    server.purchase(tpDto.getGameId(), tpDto.getNoOfTickets(), tpDto.getClientName());
                    return ProtoUtils.createOkResponse();
                } catch (MyException e) {
                    return ProtoUtils.createErrorResponse(e.getMessage());
                }
            }
            case GET_GAMES:{
                System.out.println("Get games request...");
                try {
                    List<Game> gamesList = server.findAllGames();
                    return ProtoUtils.createFindAllGamesResponse(gamesList);
                } catch (MyException e) {
                    return ProtoUtils.createErrorResponse(e.getMessage());
                }

            }
        }
        return response;
    }

    private void sendResponse(AppProtobuffs.Response response) throws IOException {
        System.out.println("Sending response " + response);
        response.writeDelimitedTo(output);
        output.flush();
    }
}

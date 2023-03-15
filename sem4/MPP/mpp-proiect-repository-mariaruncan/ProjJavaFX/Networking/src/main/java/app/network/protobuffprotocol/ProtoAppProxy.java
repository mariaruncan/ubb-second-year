package app.network.protobuffprotocol;

import app.model.Employee;
import app.model.Game;
import app.network.rpcprotocol.Response;
import app.network.rpcprotocol.ResponseType;
import app.services.IObserver;
import app.services.IServices;
import app.services.MyException;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.util.List;
import java.util.concurrent.BlockingQueue;
import java.util.concurrent.LinkedBlockingQueue;

public class ProtoAppProxy implements IServices {
    private String host;
    private int port;

    private IObserver client;

    private InputStream input;
    private OutputStream output;
    private Socket connection;

    private BlockingQueue<AppProtobuffs.Response> qresponses;
    private volatile boolean finished;

    public ProtoAppProxy(String host, int port) {
        this.host = host;
        this.port = port;
        qresponses = new LinkedBlockingQueue<AppProtobuffs.Response>();
    }

    @Override
    public void logIn(String username, String password, IObserver client) throws MyException {
        initializeConnection();
        System.out.println("Login request...");
        sendRequest(ProtoUtils.createLoginRequest(username, password));

        AppProtobuffs.Response response = readResponse();
        if(response.getType() == AppProtobuffs.Response.Type.OK){
            this.client = client;
            return;
        }
        if(response.getType() == AppProtobuffs.Response.Type.ERROR){
            String errorText = ProtoUtils.getError(response);
            closeConnection();
            throw new MyException(errorText);
        }
    }

    @Override
    public void purchase(long gameId, int noOfTickets, String clientName) throws MyException {
        sendRequest(ProtoUtils.createPurchaseRequest(gameId, noOfTickets, clientName));

        AppProtobuffs.Response response = readResponse();
        if(response.getType() == AppProtobuffs.Response.Type.ERROR){
            closeConnection();
            String errorText = ProtoUtils.getError(response);
            throw new MyException(errorText);
        }
    }

    @Override
    public void logOut(Employee employee, IObserver client) throws MyException {
        sendRequest(ProtoUtils.createLogoutRequest(employee));
        AppProtobuffs.Response response = readResponse();
        closeConnection();

        if(response.getType() == AppProtobuffs.Response.Type.ERROR){
            String errorText = ProtoUtils.getError(response);
            throw new MyException(errorText);
        }
    }

    @Override
    public List<Game> findAllGames() throws MyException {
        sendRequest(ProtoUtils.createFindAllGamesRequest());

        AppProtobuffs.Response response = readResponse();
        if(response.getType() == AppProtobuffs.Response.Type.ERROR){
            String errorText = response.getError();
            throw new MyException(errorText);
        }
        List<Game> games = ProtoUtils.getAllGames(response);
        return games;
    }

    private void handleUpdate(AppProtobuffs.Response response){
        if(response.getType() == AppProtobuffs.Response.Type.GAME){
            System.out.println("Game updated in proxy");
            client.updateGame(ProtoUtils.getGame(response));

        }
    }

    private void sendRequest(AppProtobuffs.Request request) throws MyException {
        try{
            System.out.println("Sending request... " + request);
            request.writeDelimitedTo(output);
            output.flush();
            System.out.println("Request sent.");
        } catch (IOException e) {
            throw new MyException("Error sending object " + e);
        }
    }

    private AppProtobuffs.Response readResponse() throws MyException {
        AppProtobuffs.Response response = null;
        try{
            response = qresponses.take();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        return response;
    }

    private void initializeConnection() {
        try{
            connection = new Socket(host, port);
            output = connection.getOutputStream();
            input = connection.getInputStream();
            finished = false;
            startReader();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void closeConnection() {
        finished = true;
        try {
            input.close();
            output.close();
            connection.close();
            client = null;
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void startReader() {
        Thread th = new Thread(new ReaderThread());
        th.start();
    }

    private class ReaderThread implements Runnable{
        @Override
        public void run() {
            while(!finished){
                try{
                    AppProtobuffs.Response response = AppProtobuffs.Response.parseDelimitedFrom(input);
                    System.out.println("Response received " + response);

                    if(isUpdateResponse(response.getType())){
                        handleUpdate(response);
                    }
                    else{
                        try{
                            qresponses.put(response);
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                } catch (IOException e) {
                    System.out.println("Reading error " + e);
                }
            }
        }
    }

    private boolean isUpdateResponse(AppProtobuffs.Response.Type type){
        return type == AppProtobuffs.Response.Type.GAME;
    }
}

package app;

import app.network.utils.AbstractServer;
import app.network.utils.RpcConcurrentServer;
import app.persistence.EmployeesRepository;
import app.persistence.GamesRepository;
import app.persistence.TeamsRepository;
import app.persistence.TicketPurchasesRepository;
import app.persistence.repository.jdbc.EmployeesDBRepository;
import app.persistence.repository.jdbc.GamesDBRepository;
import app.persistence.repository.jdbc.TeamsDBRepository;
import app.persistence.repository.jdbc.TicketPurchasesDBRepository;
import app.server.ServicesImpl;
import app.services.IServices;

import java.io.IOException;
import java.rmi.ServerException;
import java.util.Properties;

public class StartRpcServer {
    private static final int defaultPort = 55555;

    public static void main(String[] args) {
        Properties serverProps = new Properties();
        try {
            serverProps.load(StartRpcServer.class.getResourceAsStream("/server.properties"));
            System.out.println("Server properties set. ");
            serverProps.list(System.out);
        } catch (IOException e) {
            System.err.println("Cannot find server.properties " + e);
            return;
        }
        EmployeesRepository employeesRepository = new EmployeesDBRepository(serverProps);
        GamesRepository gamesRepository = new GamesDBRepository(serverProps);
        TeamsRepository teamsRepository = new TeamsDBRepository(serverProps);
        TicketPurchasesRepository ticketPurchasesRepository = new TicketPurchasesDBRepository(serverProps);
        IServices service = new ServicesImpl(employeesRepository, gamesRepository, teamsRepository,
                ticketPurchasesRepository);
        int serverPort = defaultPort;
        try {
            serverPort = Integer.parseInt(serverProps.getProperty("server.port"));
        }catch (NumberFormatException nef){
            System.err.println("Wrong  Port Number" + nef.getMessage());
            System.err.println("Using default port " + defaultPort);
        }
        System.out.println("Starting server on port: " + serverPort);
        AbstractServer server = new RpcConcurrentServer(serverPort, service);
        try {
            server.start();
        } catch (ServerException e) {
            System.err.println("Error starting the server" + e.getMessage());
        }finally {
            try {
                server.stop();
            }catch(ServerException e){
                System.err.println("Error stopping server " + e.getMessage());
            }
        }
    }
}

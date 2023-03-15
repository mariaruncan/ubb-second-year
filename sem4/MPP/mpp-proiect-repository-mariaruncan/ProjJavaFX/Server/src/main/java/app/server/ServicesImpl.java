package app.server;

import app.model.Employee;
import app.model.Game;
import app.model.TicketPurchase;
import app.persistence.EmployeesRepository;
import app.persistence.GamesRepository;
import app.persistence.TeamsRepository;
import app.persistence.TicketPurchasesRepository;
import app.services.IObserver;
import app.services.IServices;
import app.services.MyException;
import app.utils.Hash;

import java.util.ArrayList;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ServicesImpl implements IServices {

    private EmployeesRepository employeesRepo;
    private GamesRepository gamesRepo;
    private TeamsRepository teamsRepo;
    private TicketPurchasesRepository ticketPurchasesRepo;
    private Map<Long, IObserver> loggedEmployees;
    private final int defaultThreadsNo = 5;

    public ServicesImpl(EmployeesRepository eRepo, GamesRepository gRepo, TeamsRepository tRepo,
                        TicketPurchasesRepository tpRepo)
    {
        employeesRepo = eRepo;
        gamesRepo = gRepo;
        teamsRepo = tRepo;
        ticketPurchasesRepo = tpRepo;
        loggedEmployees = new ConcurrentHashMap<>();
    }
    @Override
    public synchronized void logIn(String username, String password, IObserver client) throws MyException {
        Employee emp = employeesRepo.findByUsername(username);
        if (emp != null) {
            if(!emp.getHashedPassword().equals(Hash.hash(password)))
                throw new MyException("Authentication failed.");
            if(loggedEmployees.get(emp.getId()) != null)
                throw new MyException("Employee already logged in!");
            loggedEmployees.put(emp.getId(), client);
        }
        else{
            throw new MyException("Authentication failed.");
        }
    }

    @Override
    public synchronized void purchase(long gameId, int noOfTickets, String clientName) throws MyException {
        Game game = gamesRepo.findById(gameId);
        float totalPrice = noOfTickets * game.getPricePerTicket();
        TicketPurchase ticketPurchase = new TicketPurchase(game, noOfTickets, totalPrice, clientName);
        ticketPurchasesRepo.add(ticketPurchase);
        game.setSoldTickets(game.getSoldTickets() + ticketPurchase.getNoOfTickets());
        gamesRepo.update(game.getId(), game);
        notifyEmployees(gamesRepo.findById(game.getId()));
    }

    @Override
    public synchronized void logOut(Employee employee, IObserver client) throws MyException {
        Employee emp = employeesRepo.findByUsername(employee.getUsername());
        IObserver client1 = loggedEmployees.remove(emp.getId());
        if(client1 == null)
            throw new MyException("Employee " + employee.getUsername() + "is not logged in!");
    }

    private void notifyEmployees(Game game){
        ExecutorService executor = Executors.newFixedThreadPool(defaultThreadsNo);
        for(IObserver client: loggedEmployees.values()){
            executor.execute(() ->{
                System.out.println("Game update in ServiceImpl" + game + " " + client);
                client.updateGame(game);
            });
        }
    }


    @Override
    public synchronized ArrayList<Game> findAllGames() {
        ArrayList<Game> result = new ArrayList<>();
        for(Game g : gamesRepo.findAll()){
            result.add(g);
        }
        return result;
    }
}

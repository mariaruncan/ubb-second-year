package app.services;

import app.model.Employee;
import app.model.Game;
import app.model.TicketPurchase;

import java.util.List;

public interface IServices {
    void logIn(String username, String password, IObserver client) throws MyException;
    void purchase(long gameId, int noOfTickets, String clientName) throws MyException;
    void logOut(Employee employee, IObserver client) throws MyException;
    List<Game> findAllGames() throws MyException;
}

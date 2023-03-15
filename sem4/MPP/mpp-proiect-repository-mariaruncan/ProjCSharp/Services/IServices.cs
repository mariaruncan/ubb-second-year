using System.Collections.Generic;
using Model;

namespace Services
{
    public interface IServices
    {
        void LogIn(string username, string pass, IObserver client);
        void Purchase(long gameId, int noOfTickets, string clientName);
        void LogOut(Employee employee, IObserver client);
        List<Game> FindAllGames();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Persistence;
using Server.utils;
using Services;

namespace Server
{
    public class ServicesImpl : IServices
    {
        private EmployeesRepository employeesRepo;
        private GamesRepository gamesRepo;
        private TeamsRepository teamsRepo;
        private TicketPurchesesRepository ticketPurchesesRepo;
        private readonly IDictionary<long, IObserver> loggedEmployees;

        public ServicesImpl(EmployeesRepository employeesRepo, GamesRepository gamesRepo, TeamsRepository teamsRepo, 
            TicketPurchesesRepository ticketPurchesesRepo)
        {
            this.employeesRepo = employeesRepo;
            this.gamesRepo = gamesRepo;
            this.teamsRepo = teamsRepo;
            this.ticketPurchesesRepo = ticketPurchesesRepo;
            loggedEmployees = new Dictionary<long, IObserver>();
        }

        public List<Model.Game> FindAllGames()
        {
            return gamesRepo.findAll().ToList();
        }

        public void LogIn(string username, string pass, IObserver client)
        {
            Model.Employee emp = employeesRepo.FindByUsername(username);
            if(emp != null)
            {
                if(emp.HashedPassword != Hash.hash(pass))
                    throw new MyException("Authentication failed.");
                if (loggedEmployees.ContainsKey(emp.Id))
                    throw new MyException("Employee already logged in!");
                loggedEmployees.Add(emp.Id, client);
            }
            else
                throw new MyException("Authentication failed.");
        }

        public void LogOut(Model.Employee employee, IObserver client)
        {
            Model.Employee emp = employeesRepo.FindByUsername(employee.Username);
            IObserver client1 = loggedEmployees[emp.Id];
            if (client1 == null)
                throw new MyException("Employee " + employee.Username + " is not logged in!");
            loggedEmployees.Remove(emp.Id);
        }

        public void Purchase(long gameId, int noOfTickets, string clientName)
        {
            Model.Game game = gamesRepo.findById(gameId);
            float totalPrice = game.PricePerTicket * noOfTickets;
            Model.TicketPurchase ticketPurchase = new Model.TicketPurchase(game, noOfTickets, totalPrice, clientName);
            ticketPurchesesRepo.add(ticketPurchase);
            game.SoldTickets += ticketPurchase.NoOfTickets;
            gamesRepo.update(game, gameId);
            notifyEmployees(gamesRepo.findById(gameId));
        }

        private void notifyEmployees(Model.Game game)
        {
            Console.WriteLine("notify game updated");
            foreach(var elem in loggedEmployees)
            {
                Console.WriteLine("notify client {0}", elem.Key);
                Task.Run(() => elem.Value.UpdateGame(game));
            }
        }
    }
}

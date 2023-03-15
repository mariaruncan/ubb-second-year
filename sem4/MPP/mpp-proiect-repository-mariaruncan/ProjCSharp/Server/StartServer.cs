using System;
using Persistence;
using Persistence.db;
using Services;
using Networking;
using System.Threading;
using System.Net.Sockets;
using System.Configuration;
using NetworkingProto;
using Persistence.db.ef;

namespace Server
{
    static class StartServer
    {
        static void Main(string[] args)
        {
            EmployeesRepository employeesRepository = new EmployeesDbEfRepository();
            GamesRepository gamesRepository = new GamesDBRepository();
            TeamsRepository teamsRepository = new TeamsDBRepository();
            TicketPurchesesRepository ticketPurchesesRepository = new TicketPurchasesDBRepository();
            
            IServices services = new ServicesImpl(employeesRepository, gamesRepository, teamsRepository, ticketPurchesesRepository);

            string host = ConfigurationManager.AppSettings.Get("host");
            int port = Int32.Parse(ConfigurationManager.AppSettings.Get("port"));

            // MyConcurrentServer server = new MyConcurrentServer(host, port, services);
            ProtoV3Server server = new ProtoV3Server(host, port, services);
            server.Start();
            Console.WriteLine("Server started ...");
            Console.ReadLine();
        }
    }

    public class ProtoV3Server : ServerUtils.ConcurrentServer
    {
        private IServices server;
        private ProtoWorker worker;

        public ProtoV3Server(string host, int port, IServices server)
            :base(host, port)
        {
            this.server = server;
            Console.WriteLine("Proto Server...");
        }

        protected override Thread createWorker(TcpClient client)
        {
            worker = new ProtoWorker(server, client);
            return new Thread(new ThreadStart(worker.run));
        }
    }


    public class MyConcurrentServer : ServerUtils.ConcurrentServer
    {
        private IServices server;
        private ClientWorker worker;

        public MyConcurrentServer(string host, int port, IServices server) : base(host, port)
        {
            this.server = server;
            Console.WriteLine("MyConcurrentServer...");
        }

        protected override Thread createWorker(TcpClient client)
        {
            worker = new ClientWorker(server, client);
            return new Thread(new ThreadStart(worker.run));
        }
    }
}

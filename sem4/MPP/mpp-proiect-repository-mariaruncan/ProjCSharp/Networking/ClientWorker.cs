using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Model;
using Model.dto;
using Services;

namespace Networking
{
    public class ClientWorker : IObserver
    {
        private IServices server;
        private TcpClient connection;

        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool connected;

        public ClientWorker(IServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public virtual void run()
        {
            while (connected)
            {
                try
                {
                    object request = formatter.Deserialize(stream);
                    object response = handleRequest((Request) request);
                    if(response != null)
                    {
                        sendResponse((Response) response);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }

            try
            {
                stream.Close();
                connection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        private Response handleRequest(Request request)
        {
            Response response = null;
            
            if(request is LogInRequest)
            {
                Console.WriteLine("Log in request...");
                LogInRequest logInRequest = (LogInRequest) request;
                EmployeeDto employee = logInRequest.empDto;
                try
                {
                    lock (server)
                    {
                        server.LogIn(employee.Username, employee.Password, this);
                    }
                    return new OkResponse();
                }
                catch (MyException e)
                {
                    connected = false;
                    return new ErrorResponse(e.Message);
                }
            }

            if(request is LogOutRequest)
            {
                Console.WriteLine("Log out request...");
                LogOutRequest logOutRequest = (LogOutRequest) request;
                Employee employee = DtoUtils.GetFromDTO(logOutRequest.empDto);
                try
                {
                    lock (server)
                    {
                        server.LogOut(employee, this);
                    }
                    connected = false;
                    return new OkResponse();
                }
                catch (MyException e)
                {
                    return new ErrorResponse(e.Message);
                }
            }

            if(request is GetGamesRequest)
            {
                Console.WriteLine("Get games request...");
                try
                {
                    List<Game> games;
                    lock (server)
                    {
                        games = server.FindAllGames();
                    }
                    return new GetGamesResponse(games);
                }
                catch (MyException e)
                {
                    return new ErrorResponse(e.Message);
                }
            }

            if(request is PurchaseRequest)
            {
                Console.WriteLine("Purchase request...");
                try
                {
                    PurchaseRequest purchaseRequest = (PurchaseRequest) request;
                    TicketPurchaseDto tpDto = purchaseRequest.ticketPurchase;
                    lock (server)
                    {
                        server.Purchase(tpDto.gameId, tpDto.noOfTickets, tpDto.clientName);
                    }
                    return new OkResponse();
                }
                catch(MyException e)
                {
                    return new ErrorResponse(e.Message);
                }
            }
            
            return response;
        }

        private void sendResponse(Response response)
        {
            Console.WriteLine("sending response " + response);
            formatter.Serialize(stream, response);
            stream.Flush();
        }

        public virtual void UpdateGame(Game game)
        {
            Console.WriteLine("Notify employees in worker " + game);
            try
            {
                sendResponse(new GameResponse(game));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

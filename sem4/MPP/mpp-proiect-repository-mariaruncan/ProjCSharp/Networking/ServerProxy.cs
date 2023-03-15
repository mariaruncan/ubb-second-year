using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Model;
using Model.dto;
using Services;

namespace Networking
{
    public class ServerProxy : IServices
    {
        private string host;
        private int port;

        private IObserver client;

        private NetworkStream stream;

        private IFormatter formatter;
        private TcpClient connection;

        private Queue<Response> responses;
        private volatile bool finished;
        private EventWaitHandle _waitHandle;

        public ServerProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            responses = new Queue<Response>();
        }

        public List<Game> FindAllGames()
        {
            sendRequest(new GetGamesRequest());

            Response response = readResponse();
            if(response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new MyException(err.Message);
            }

            GetGamesResponse response1 = (GetGamesResponse)response;
            List<Game> games = response1.games;
            return games;
        }

        public void LogIn(string username, string pass, IObserver client)
        {
            initializeConnection();
            EmployeeDto empDto = new EmployeeDto(username, pass);
            sendRequest(new LogInRequest(empDto));

            Response response = readResponse();
            if(response is OkResponse)
            {
                this.client = client;
                return;
            }

            if(response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                closeConnection();
                throw new MyException(err.Message);
            }
        }

        public void LogOut(Employee employee, IObserver client)
        {
            EmployeeDto empDto = DtoUtils.getDTO(employee);
            sendRequest(new LogOutRequest(empDto));

            Response response = readResponse();
            closeConnection();
            if(response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                throw new MyException(err.Message);
            }
        }

        public void Purchase(long gameId, int noOfTickets, string clientName)
        {
            TicketPurchaseDto ticketPurchase = new TicketPurchaseDto(gameId, noOfTickets, clientName);
            sendRequest(new PurchaseRequest(ticketPurchase));

            Response response = readResponse();
            if (response is OkResponse)
            {
                Console.WriteLine("Tickets successfully bought");
                return;
            }

            if (response is ErrorResponse)
            {
                ErrorResponse err = (ErrorResponse)response;
                closeConnection();
                throw new MyException(err.Message);
            }
        }

        private void sendRequest(Request request)
        {
            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception e)
            {
                throw new MyException("Error sending object " + e);
            }
        }

        private Response readResponse()
        {
            Response response = null;
            try
            {
                _waitHandle.WaitOne();
                lock (responses)
                {
                    response = responses.Dequeue();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return response;
        }

        private void initializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                _waitHandle = new AutoResetEvent(false);
                startReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void closeConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                connection.Close();
                _waitHandle.Close();
                client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void handleUpdate(Response response)
        {
            if(response is GameResponse)
            {
                Console.WriteLine("handle update in proxy");
                GameResponse response1 = (GameResponse)response;
                client.UpdateGame(response1.game);
            }
        }

        private void startReader()
        {
            Thread tw = new Thread(run);
            tw.Start();
        }

        private bool isUpdate(Response response)
        {
            return response is GameResponse;
        }

        public virtual void run()
        {
            while (!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream);
                    Console.WriteLine("response received " + response);
                    if (isUpdate((Response)response))
                    {
                        handleUpdate((Response)response);
                    }
                    else
                    {
                        lock (responses)
                        {
                            responses.Enqueue((Response)response);
                        }

                        _waitHandle.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Reading error " + e);
                }
            }
        }
    }
}

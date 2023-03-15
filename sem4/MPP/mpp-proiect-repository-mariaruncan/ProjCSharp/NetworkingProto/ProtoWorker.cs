using Model;
using Model.dto;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;

namespace NetworkingProto
{
    public class ProtoWorker:IObserver
    {
        private IServices server;
        private TcpClient connection;

        private NetworkStream stream;
        private volatile bool connected;

        public ProtoWorker(IServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                connected = true;
            }
            catch (Exception ex)
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
                    Request request = Request.Parser.ParseDelimitedFrom(stream);
                    Response response = handleRequest(request);
                    if (response != null)
                    {
                        sendResponse(response);
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
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            try
            {
                stream.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e);
            }
        }

        private Response handleRequest(Request request)
        {
            Response response = null;
            Request.Types.Type reqType = request.Type;

            switch (reqType)
            {
                case Request.Types.Type.Login:
                    {
                        Console.WriteLine("Log in request...");
                        Model.dto.EmployeeDto empDto = ProtoUtils.getEmpDto(request);
                        try
                        {
                            lock (server)
                            {
                                server.LogIn(empDto.Username, empDto.Password, this);
                            }
                            return ProtoUtils.createOkResponse();
                        }
                        catch(MyException e)
                        {
                            connected = false;
                            return ProtoUtils.createErrorResponse(e.Message);
                        }
                    }
                case Request.Types.Type.Logout:
                    {
                        Console.WriteLine("Log out request...");
                        Model.Employee employee = DtoUtils.GetFromDTO(ProtoUtils.getEmpDto(request));
                        try
                        {
                            lock (server)
                            {
                                server.LogOut(employee, this);
                            }
                            connected = false;
                            return ProtoUtils.createOkResponse();
                        }
                        catch (MyException e)
                        {
                            return ProtoUtils.createErrorResponse(e.Message);
                        }
                    }
                case Request.Types.Type.Purchase:
                    {
                        Console.WriteLine("Purchase request...");
                        Model.dto.TicketPurchaseDto tpDto = ProtoUtils.getTicketPurchaseDto(request);
                        try
                        {
                            lock (server)
                            {
                                server.Purchase(tpDto.gameId, tpDto.noOfTickets, tpDto.clientName);
                            }
                            return ProtoUtils.createOkResponse();
                        }
                        catch (MyException e)
                        {
                            return ProtoUtils.createErrorResponse(e.Message);
                        }
                    }
                case Request.Types.Type.GetGames:
                    {
                        Console.WriteLine("Get games request...");
                        try
                        {
                            List<Model.Game> games;
                            lock (server)
                            {
                                games = server.FindAllGames();
                            }
                            return ProtoUtils.createFindAllGamesResponse(games);
                        }
                        catch (MyException e)
                        {
                            return ProtoUtils.createErrorResponse(e.Message);
                        }
                    }
            }
            return response;
        }

        private void sendResponse(Response response)
        {
            Console.WriteLine("sending response " + response);
            lock (stream)
            {
                response.WriteDelimitedTo(stream);
                stream.Flush();
            }
        }

        public virtual void UpdateGame(Model.Game game)
        {
            Console.WriteLine("Notify employees in worker, games updates  " + game);
            try
            {
                sendResponse(ProtoUtils.createGameResponse(game));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}

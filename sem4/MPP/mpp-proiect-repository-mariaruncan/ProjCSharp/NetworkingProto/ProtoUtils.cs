using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingProto
{
    public class ProtoUtils
    {
        public static Model.dto.EmployeeDto getEmpDto(Request request)
        {
            Model.dto.EmployeeDto empDto = new Model.dto.EmployeeDto(request.EmpDto.Username, request.EmpDto.Password);
            return empDto;
        }

        public static Response createOkResponse()
        {
            Response response = new Response { Type = Response.Types.Type.Ok };
            return response;
        }

        public static Response createErrorResponse(string message)
        {
            Response response = new Response { Type = Response.Types.Type.Error, Error = message };
            return response;
        }

        public static Model.dto.TicketPurchaseDto getTicketPurchaseDto(Request request)
        {
            Model.dto.TicketPurchaseDto tpDto = new Model.dto.TicketPurchaseDto(request.TpDto.GameId, request.TpDto.NoOfTickets, request.TpDto.ClientName);
            return tpDto;
        }

        public static Response createFindAllGamesResponse(List<Model.Game> games)
        {
            Response response = new Response { Type = Response.Types.Type.GetGame };
            foreach (Model.Game game in games)
            {
                var x = game.Date;
                String date = x.Day.ToString() + "-" + x.Month.ToString() + "-" + x.Year.ToString();
                Team team1 = new Team { Id = game.Team1.Id, Name = game.Team1.Name };
                Team team2 = new Team { Id = game.Team2.Id, Name = game.Team2.Name };
                Game g = new Game { Id = game.Id, Team1 = team1, Team2 = team2,  Date = date,
                Description = game.Description, TotalNoTickets = game.TotalNoOfTickets,
                SoldTickets = game.SoldTickets, PricePerTicket = game.PricePerTicket};
                response.Games.Add(g);
            }
            return response;
        }

        public static Response createGameResponse(Model.Game game)
        {
            var x = game.Date;
            String date = x.Day.ToString() + "-" + x.Month.ToString() + "-" + x.Year.ToString();
            Team team1 = new Team { Id = game.Team1.Id, Name = game.Team1.Name };
            Team team2 = new Team { Id = game.Team2.Id, Name = game.Team2.Name };
            Game g = new Game { Id = game.Id, Team1 = team1, Team2 = team2, Date = date, Description = game.Description,TotalNoTickets = game.TotalNoOfTickets,
                SoldTickets = game.SoldTickets, PricePerTicket = game.PricePerTicket };
            Response response = new Response { Type = Response.Types.Type.Game , Game = g};
            return response;
        }
    }
}

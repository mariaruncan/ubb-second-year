using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Game : Entity<long>
    {
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int TotalNoOfTickets { get; set; }
        public int SoldTickets { get; set; }
        public float PricePerTicket { get; set; }

        public Game(): base(0)
        {
            Team1 = null;
            Team2 = null;
            Date = DateTime.MinValue;
            Description = "";
            TotalNoOfTickets = 0;
            SoldTickets = 0;
            PricePerTicket = 0;
        }

        public Game(Team team1, Team team2, DateTime date, string description, int totalNoOfTickets, int soldTickets, float pricePerTicket): base(0)
        {
            Team1 = team1;
            Team2 = team2;
            Date = date;
            Description = description;
            TotalNoOfTickets = totalNoOfTickets;
            SoldTickets = soldTickets;
            PricePerTicket = pricePerTicket;
        }

        public override string ToString()
        {
            return "Game: " + base.Id + " " + Date + " " + Team1.Name + " vs. " + Team2.Name +
                "\n" + Description + "\nTickets available: " + (TotalNoOfTickets - SoldTickets) +
                " for only " + PricePerTicket + "$ per ticket";
        }

        public override bool Equals(object obj)
        {
            return obj is Game game &&
                   Id == game.Id &&
                   EqualityComparer<Team>.Default.Equals(Team1, game.Team1) &&
                   EqualityComparer<Team>.Default.Equals(Team2, game.Team2) &&
                   Description == game.Description &&
                   TotalNoOfTickets == game.TotalNoOfTickets &&
                   SoldTickets == game.SoldTickets &&
                   PricePerTicket == game.PricePerTicket;
        }

        public override int GetHashCode()
        {
            int hashCode = 948494407;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Team>.Default.GetHashCode(Team1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Team>.Default.GetHashCode(Team2);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + TotalNoOfTickets.GetHashCode();
            hashCode = hashCode * -1521134295 + SoldTickets.GetHashCode();
            hashCode = hashCode * -1521134295 + PricePerTicket.GetHashCode();
            return hashCode;
        }

        public string AvailableTickets()
        {
            if (TotalNoOfTickets == SoldTickets)
            {
                return "SOLD OUT!";
            }
            return (TotalNoOfTickets - SoldTickets).ToString();
        }
    }
}

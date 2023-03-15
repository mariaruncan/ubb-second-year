using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class TicketPurchase : Entity<long>
    {
        public Game Game { get; set; }
        public int NoOfTickets { get; set; }
        public float TotalPrice { get; set; }
        public string ClientName { get; set; }

        public TicketPurchase() : base(0)
        {
            Game = null;
            NoOfTickets = 0;
            TotalPrice = 0;
            ClientName = "";
        }

        public TicketPurchase(Game game, int noOfTickets, float totalPrice, string clientName): base(0)
        {
            Game = game;
            NoOfTickets = noOfTickets;
            TotalPrice = totalPrice;
            ClientName = clientName;
        }

        public override string ToString()
        {
            return "TicketPurchase: " + base.Id + " for game " + Game.Id + ", " +
                NoOfTickets + " bought by " + ClientName + " for " + TotalPrice + "$";
        }

        public override bool Equals(object obj)
        {
            return obj is TicketPurchase purchase &&
                   Id == purchase.Id &&
                   EqualityComparer<Game>.Default.Equals(Game, purchase.Game) &&
                   NoOfTickets == purchase.NoOfTickets &&
                   TotalPrice == purchase.TotalPrice &&
                   ClientName == purchase.ClientName;
        }

        public override int GetHashCode()
        {
            int hashCode = 202997279;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Game>.Default.GetHashCode(Game);
            hashCode = hashCode * -1521134295 + NoOfTickets.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalPrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClientName);
            return hashCode;
        }
    }
}

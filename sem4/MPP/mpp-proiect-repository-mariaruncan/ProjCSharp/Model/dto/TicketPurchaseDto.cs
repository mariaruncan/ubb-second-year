using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.dto
{
    [Serializable]
    public class TicketPurchaseDto
    {
        public long gameId { get; set; }
        public int noOfTickets { get; set; }
        public string clientName { get; set; }

        public TicketPurchaseDto(long gameId, int noOfTickets, string clientName)
        {
            this.gameId = gameId;
            this.noOfTickets = noOfTickets;
            this.clientName = clientName;
        }

        public override bool Equals(object obj)
        {
            return obj is TicketPurchaseDto dto &&
                   gameId == dto.gameId &&
                   noOfTickets == dto.noOfTickets &&
                   clientName == dto.clientName;
        }

        public override int GetHashCode()
        {
            int hashCode = -1255186153;
            hashCode = hashCode * -1521134295 + gameId.GetHashCode();
            hashCode = hashCode * -1521134295 + noOfTickets.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(clientName);
            return hashCode;
        }
    }
}

using System;
using Model;
using Model.dto;

namespace Networking
{
    public interface Request { }

    [Serializable]
    public class LogInRequest : Request 
    { 
        public EmployeeDto empDto { get; }
        public LogInRequest(EmployeeDto employeeDto)
        {
            empDto = employeeDto;
        }
    }

    [Serializable]
    public class LogOutRequest : Request 
    {
        public EmployeeDto empDto { get; }
        public LogOutRequest(EmployeeDto employeeDto)
        {
            empDto = employeeDto;
        }
    }

    [Serializable]
    public class GetGamesRequest : Request
    { 
        public GetGamesRequest() { }
    }

    [Serializable]
    public class PurchaseRequest : Request 
    { 
        public TicketPurchaseDto ticketPurchase { get; }
        public PurchaseRequest(TicketPurchaseDto ticketPurchase)
        {
            this.ticketPurchase = ticketPurchase;
        }
    }
}

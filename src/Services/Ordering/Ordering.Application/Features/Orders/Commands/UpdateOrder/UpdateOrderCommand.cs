using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public decimal OrderTotal { get; set; }

        //Billing address
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        //Payment data
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expires { get; set; }
        public string Cvv { get; set; }
        public int PaymentMethod { get; set; }
    }
}

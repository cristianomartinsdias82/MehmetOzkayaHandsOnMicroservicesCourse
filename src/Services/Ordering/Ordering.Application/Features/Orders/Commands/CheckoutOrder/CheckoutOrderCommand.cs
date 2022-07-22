﻿using MediatR;
using System;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<Guid>
    {
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

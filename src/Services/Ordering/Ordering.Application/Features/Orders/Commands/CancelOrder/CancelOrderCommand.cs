using MediatR;
using System;

namespace Ordering.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}

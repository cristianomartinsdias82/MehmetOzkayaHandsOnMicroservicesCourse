using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CancelOrder;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException($"Argument {nameof(mediator)} cannot be null.");
        }

        [HttpGet("{userName}", Name = "GetOrders")]
        [ProducesResponseType(typeof(IList<OrderVm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrdersByUserName(string userName, CancellationToken cancellationToken)
        {
            var orders = await _mediator.Send(
                new GetOrdersListQuery(userName),
                cancellationToken);

            return Ok(orders);
        }

        //created just for implementation testing purposes
        [HttpPost("CheckoutOrder", Name = "CheckoutOrder")]
        [ProducesResponseType(typeof(IList<OrderVm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrderCommand command, CancellationToken cancellationToken)
        {
            var checkoutResult = await _mediator.Send(
                command,
                cancellationToken);

            return Ok(checkoutResult);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}", Name = "CancelOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelOrder(Guid id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CancelOrderCommand { Id = id }, cancellationToken);

            return NoContent();
        }
    }
}
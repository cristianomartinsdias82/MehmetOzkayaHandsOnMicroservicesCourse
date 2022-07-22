using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Helpers.Logging;
using System;
using System.Threading.Tasks;

namespace Ordering.API.Integrations.BasketCheckout
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<BasketCheckoutConsumer> _logger ;

        public BasketCheckoutConsumer(
            IMapper mapper,
            IMediator mediator,
            ILogger<BasketCheckoutConsumer> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException($"Argument {nameof(mapper)} cannot be null.");
            _mediator = mediator ?? throw new ArgumentNullException($"Argument {nameof(mediator)} cannot be null.");
            _logger = logger ?? throw new ArgumentNullException($"Argument {nameof(logger)} cannot be null.");
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            UnobtrusiveLoggingHelper.Log(
                _logger,
                "A new order has arrived from the event bus: {@Message}",
                args: new object[] { context.Message });

            await _mediator.Send(_mapper.Map<CheckoutOrderCommand>(context.Message));

            UnobtrusiveLoggingHelper.Log(
                _logger,
                "Order processed successfully!");
        }
    }
}

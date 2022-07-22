using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Helpers.Logging;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    internal class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, Guid>
    {
        private readonly IEmailService _emailService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(
            IEmailService emailService,
            IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<CheckoutOrderCommandHandler> logger)
        {
            _emailService = emailService ?? throw new ArgumentNullException($"Argument {emailService} cannot be null.");
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"Argument {orderRepository} cannot be null.");
            _mapper = mapper ?? throw new ArgumentNullException($"Argument {mapper} cannot be null.");
            _logger = logger ?? throw new ArgumentNullException($"Argument {logger} cannot be null.");
        }

        public async Task<Guid> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            UnobtrusiveLoggingHelper.Log(_logger, "Checkout order is being processed...");

            var order = _mapper.Map<Order>(request);

            await _orderRepository.AddAsync(order, cancellationToken);

            UnobtrusiveLoggingHelper.Log(_logger, "Order created successfully! Order number: {OrderId}", args: order.Id);

            await SendEmailToCustomerAsync(order, cancellationToken);

            return order.Id;
        }

        private async Task SendEmailToCustomerAsync(Order order, CancellationToken cancellationToken)
        {
            //TODO: Implement resiliency with Polly (Policy class) here
            try
            {
                await _emailService.SendEmailAsync(
                    new()
                    {
                        To = order.EmailAddress,
                        Body = $"Order created successfully! Order id: {order.Id}",
                        Subject = $"My Cool Online Store - Order created successfully! Order number: {order.Id}"
                    },
                    cancellationToken);
            }
            catch(Exception exc)
            {
                UnobtrusiveLoggingHelper.Log(
                    _logger,
                    "Error while attempting to send order creation successful email!",
                    exception: exc,
                    args: order.Id);
            }
        }
    }
}

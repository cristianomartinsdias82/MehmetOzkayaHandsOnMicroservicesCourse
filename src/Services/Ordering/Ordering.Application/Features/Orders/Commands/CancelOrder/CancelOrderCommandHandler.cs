using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Helpers.Logging;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CancelOrder
{
    internal class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Unit>
    {
        private readonly IEmailService _emailService;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CancelOrderCommandHandler> _logger;

        public CancelOrderCommandHandler(
            IEmailService emailService,
            IOrderRepository orderRepository,
            ILogger<CancelOrderCommandHandler> logger)
        {
            _emailService = emailService ?? throw new ArgumentNullException($"Argument {emailService} cannot be null.");
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"Argument {orderRepository} cannot be null.");
            _logger = logger ?? throw new ArgumentNullException($"Argument {logger} cannot be null.");
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var orderData = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (orderData is null)
            {
                UnobtrusiveLoggingHelper.Log(_logger, "Order with id {Id} was not found.", args: request.Id);

                throw new NotFoundException(nameof(Order), request.Id);
            }

            UnobtrusiveLoggingHelper.Log(_logger, "Order {Id} is being cancelled...", args: request.Id);

            await _orderRepository.DeleteAsync(orderData, cancellationToken);

            UnobtrusiveLoggingHelper.Log(_logger, "Order cancelled successfully! Order number: {OrderId}", args: request.Id);

            await SendEmailToCustomerAsync(orderData, cancellationToken);

            return Unit.Value;
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
                        Body = $"Your order {order.Id} was cancelled.",
                        Subject = $"My Cool Online Store - Order {order.Id} cancellation"
                    },
                    cancellationToken);
            }
            catch (Exception exc)
            {
                UnobtrusiveLoggingHelper.Log(
                    _logger,
                    "Error while attempting to send order cancellation notification email!",
                    exception: exc,
                    args: order.Id);
            }
        }
    }
}

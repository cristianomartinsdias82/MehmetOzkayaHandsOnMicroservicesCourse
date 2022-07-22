using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Application.Helpers.Logging;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(
            IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"Argument {orderRepository} cannot be null.");
            _mapper = mapper ?? throw new ArgumentNullException($"Argument {mapper} cannot be null.");
            _logger = logger ?? throw new ArgumentNullException($"Argument {logger} cannot be null.");
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderData = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
            if (orderData is null)
            {
                UnobtrusiveLoggingHelper.Log(_logger, "Order with id {Id} was not found.", args: request.Id);

                throw new NotFoundException(nameof(Order), request.Id);
            }

            //THIS IS NEW!!!!
            _mapper.Map(request, orderData, typeof(UpdateOrderCommand), typeof(Order));

            UnobtrusiveLoggingHelper.Log(_logger, "Update order data is being processed...");

            await _orderRepository.UpdateAsync(orderData, cancellationToken);

            UnobtrusiveLoggingHelper.Log(_logger, "Order updated successfully! Order number: {OrderId}", args: orderData.Id);

            return Unit.Value;
        }
    }
}

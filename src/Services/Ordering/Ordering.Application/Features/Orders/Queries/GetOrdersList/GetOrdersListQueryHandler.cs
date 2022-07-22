using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, IList<OrderVm>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(
            IOrderRepository orderRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException($"Argument {nameof(orderRepository)} cannot be null.");
            _mapper = mapper ?? throw new ArgumentNullException($"Argument {nameof(mapper)} cannot be null.");
        }

        public async Task<IList<OrderVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            //throw new Exception("The user is gonna loose his shit about this problem!");
            //TODO: Implement a decent Global exception handler in presentation layer for the proble above

            var orders = await _orderRepository.GetOrdersByUserName(request.UserName, cancellationToken);

            return _mapper.Map<List<OrderVm>>(orders);
        }
    }
}

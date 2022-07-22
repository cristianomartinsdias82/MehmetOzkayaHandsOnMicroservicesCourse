using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Solution.SharedKernel;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Basket.API.Integrations.CheckoutServices
{
    public class BasketCheckoutService : IBasketCheckoutService
    {
        private readonly IBasketRepository _basketRepository;
        //private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        private readonly ISendEndpoint _sendEndpoint;

        public BasketCheckoutService(
            IBasketRepository basketRepository,
            //IPublishEndpoint publishEndpoint,
            IMapper mapper,
            
            ISendEndpointProvider sendEndpointProvider)
        {
            _basketRepository = basketRepository ?? throw new ArgumentNullException($"Argument {nameof(basketRepository)} cannot be null.");
            //_publishEndpoint = publishEndpoint ?? throw new ArgumentNullException($"Argument {nameof(publishEndpoint)} cannot be null.");
            _mapper = mapper ?? throw new ArgumentNullException($"Argument {nameof(mapper)} cannot be null.");

            _sendEndpoint = sendEndpointProvider
                .GetSendEndpoint(new Uri($"queue:{EventBus.Messages.Common.EventBusConstants.BasketCheckoutQueue}"))
                .GetAwaiter()
                .GetResult();
        }

        public async ValueTask<OperationResult> CheckoutAsync(
            CheckoutBasket basket,
            CancellationToken cancellationToken = default)
        {
            //Get existing basket by username
            var customerBasket = await _basketRepository.GetBasket(basket.UserName, cancellationToken);
            if (customerBasket is null)
            {
                var failedOperationResult = new OperationResult(
                    "One or more problems occurred.",
                    StatusCodes.Status400BadRequest);

                failedOperationResult.AddFailureDetail(new()
                {
                    Details = $"Basket not found for user {basket.UserName}",
                    Field = "Basket"
                });
            }

            //Create checkout event
            var basketCheckoutEvent = _mapper.Map<BasketCheckoutEvent>(basket);

            //Sets explicitly the order total
            basketCheckoutEvent.OrderTotal = basket.OrderTotal;

            //Dispatch the event to the event bus
            await _sendEndpoint.Send(
                basketCheckoutEvent,
                cancellationToken);
            //await _publishEndpoint.Publish(
            //    basketCheckoutEvent,
            //    cancellationToken);

            //Remove the user's basket from the database
            await _basketRepository.RemoveBasket(basket.UserName, cancellationToken);

            return new OperationResult("Ok", StatusCodes.Status202Accepted);
        }
    }
}
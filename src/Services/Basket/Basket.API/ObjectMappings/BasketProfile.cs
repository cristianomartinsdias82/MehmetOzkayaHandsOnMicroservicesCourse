using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.API.ObjectMappings
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CheckoutBasket, BasketCheckoutEvent>()
                .ReverseMap();
        }
    }
}

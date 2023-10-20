using AutoMapper;
using EventBus.Messages.Events;

namespace Basket.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
    }
}

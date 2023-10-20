using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Command.CheckoutOrder;

namespace Ordering.API.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
    }
}

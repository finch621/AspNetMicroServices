using AutoMapper;
using MediatR;
using EventBus.Messages.Events;
using MassTransit;
using Ordering.Application.Features.Orders.Command.CheckoutOrder;

namespace Ordering.API.EventBusConsumer;

public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<BasketCheckoutConsumer> _logger;

    public BasketCheckoutConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckoutConsumer> logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        _logger.LogInformation($"Consuming message with id:{context.Message.Id}");
        var checkoutOrderCommand = _mapper.Map<CheckoutOrderCommand>(context.Message);
        var result = await _mediator.Send(checkoutOrderCommand);

        _logger.LogInformation($"BasketCheckoutEvent consumed succesfully with newly created order id:{result}");
    }
}

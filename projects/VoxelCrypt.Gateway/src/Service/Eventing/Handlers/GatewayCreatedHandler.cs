using Microsoft.Extensions.Logging;
using Service.Eventing.Contracts;

namespace Service.Eventing.Handlers;

public sealed class GatewayCreatedHandler(ILogger<GatewayCreatedHandler> logger)
{
    public Task Handle(GatewayCreated @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled Gateway event {GatewayId}", @event.GatewayId);
        return Task.CompletedTask;
    }
}

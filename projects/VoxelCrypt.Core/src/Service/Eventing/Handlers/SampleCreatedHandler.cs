using Microsoft.Extensions.Logging;
using Service.Eventing.Contracts;

namespace Service.Eventing.Handlers;

public sealed class SampleCreatedHandler(ILogger<SampleCreatedHandler> logger)
{
    public Task Handle(SampleCreated @event, CancellationToken cancellationToken) { }
}

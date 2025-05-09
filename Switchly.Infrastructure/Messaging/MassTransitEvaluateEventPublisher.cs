using MassTransit;
using Switchly.Application.Common.Messaging;
using Switchly.Shared.Events;

namespace Switchly.Infrastructure.Messaging;

public class MassTransitEvaluateEventPublisher : IEvaluateEventPublisher
{
  private readonly IPublishEndpoint _publishEndpoint;

  public MassTransitEvaluateEventPublisher(IPublishEndpoint publishEndpoint)
  {
    _publishEndpoint = publishEndpoint;
  }

  public async Task PublishAsync(FeatureFlagEvaluatedEvent @event, CancellationToken cancellationToken = default)
  {
    await _publishEndpoint.Publish(@event, cancellationToken);
  }
}

using Switchly.Shared.Events;

namespace Switchly.Application.Common.Messaging;

public interface IEvaluateEventPublisher
{
  Task PublishAsync(FeatureFlagEvaluatedEvent @event, CancellationToken cancellationToken = default);
}

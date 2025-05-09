namespace Switchly.Shared.Events;

public record FeatureFlagEvaluatedEvent(
  Guid FlagId,
  string FlagKey,
  UserSegmentContextModel UserContextModel,
  bool IsEnabled,
  DateTime EvaluatedAt,
  string ClientKey,
  List<string> RedisKeys // her zaman hashli olarak gönderilecek
);


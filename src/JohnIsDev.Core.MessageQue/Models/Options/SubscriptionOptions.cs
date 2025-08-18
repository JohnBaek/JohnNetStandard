namespace JohnIsDev.Core.MessageQue.Models.Options;

/// <summary>
/// Represents configuration options for a message subscription.
/// </summary>
public class SubscriptionOptions
{
    /// <summary>
    /// Represents the name of the consumer group associated with the messaging system.
    /// Consumer groups allow multiple consumers to coordinate message processing by ensuring
    /// that each message is processed only once within the group. This property identifies
    /// the specific group to which the consumer belongs.
    /// </summary>
    public string ConsumerGroup { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether the subscription should automatically acknowledge
    /// received messages. When set to true, messages are acknowledged immediately upon reception.
    /// When set to false, manual acknowledgment is required.
    /// </summary>
    public bool AutoAck { get; set; } = false;

    /// <summary>
    /// Gets or sets the number of messages that can be prefetched and buffered by the consumer.
    /// This property determines how many messages will be fetched from the queue
    /// and held locally by the consumer before being processed. Adjusting this value
    /// can help optimize throughput and control resource usage based on application needs.
    /// A higher value can improve performance by reducing round-trip delays, but may
    /// increase memory usage. A value of 1 typically ensures the consumer processes
    /// one message at a time.
    /// </summary>
    public int PrefetchCount { get; set; } = 1;

    /// <summary>
    /// Indicates whether the subscription is durable, allowing messages to be persisted
    /// even if the consumer is offline. When set to true, the subscription ensures that
    /// messages are retained and delivered to the consumer when it becomes available.
    /// Defaults to true.
    /// </summary>
    public bool DurableSubscription { get; set; } = true;
}
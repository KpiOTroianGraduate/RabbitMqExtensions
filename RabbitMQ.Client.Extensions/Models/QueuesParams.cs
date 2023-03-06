namespace RabbitMQ.Client.Extensions.Models;

/// <summary>
///     Used for queue with repeat queue
/// </summary>
public class QueuesParams
{
    /// <summary>
    ///     Queue name
    /// </summary>
    public string MainQueueName { get; set; } = null!;

    /// <summary>
    ///     The queue in which error messages will be placed
    /// </summary>
    public string RepeatedQueueName { get; set; } = null!;

    /// <summary>
    ///     Exchange which will control both queues
    /// </summary>
    public string ExchangeQueueName { get; set; } = null!;
}
namespace RabbitMQ.Client.RepeatQueues.Models;

public class QueuesParams
{
    public string MainQueueName { get; set; } = null!;
    public string RepeatedQueueName { get; set; } = null!;
    public string ExchangeQueueName { get; set; } = null!;
}
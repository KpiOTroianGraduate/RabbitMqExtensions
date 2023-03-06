using RabbitMQ.Client.RepeatQueues.Models;

namespace RabbitMQ.Client.RepeatQueues;

public static class QueueSetupNamesExtensions
{
    public static QueuesParams QueueSetupNames(this IModel model, string name)
    {
        name = name.ToUpper();

        return new QueuesParams
        {
            ExchangeQueueName = $"EXCHANGE_{name}",
            MainQueueName = $"OUTBOUND_{name}",
            RepeatedQueueName = $"OUTBOUND_{name}_REPEATED"
        };
    }
}
using RabbitMQ.Client.RepeatQueues.Models;

namespace RabbitMQ.Client.RepeatQueues;

public static class QueueDeleteExtensions
{
    public static void QueueDeleteWithRepeat(this IModel model, QueuesParams queriesParams)
    {
        model.ExchangeDelete(queriesParams.ExchangeQueueName);
        model.QueueDelete(queriesParams.MainQueueName);
        model.QueueDelete(queriesParams.RepeatedQueueName);
    }
}
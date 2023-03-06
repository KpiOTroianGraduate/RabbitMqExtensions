using RabbitMQ.Client.Extensions.Models;

namespace RabbitMQ.Client.Extensions.RepeatQueues;

public static class QueueDeleteExtensions
{
    /// <summary>
    ///     Delete main and repeat queues end exchange
    /// </summary>
    /// <param name="model"></param>
    /// <param name="queueParams"></param>
    public static void QueueDeleteWithRepeat(this IModel model, QueuesParams queueParams)
    {
        model.ExchangeDelete(queueParams.ExchangeQueueName);
        model.QueueDelete(queueParams.MainQueueName);
        model.QueueDelete(queueParams.RepeatedQueueName);
    }
}
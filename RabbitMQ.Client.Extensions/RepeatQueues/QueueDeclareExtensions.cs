using RabbitMQ.Client.Extensions.Models;

namespace RabbitMQ.Client.Extensions.RepeatQueues;

public static class QueueDeclareExtensions
{
    /// <summary>
    ///     Creates 2 queues, if the first message of the queue is rejected, it will be put into the second queue for N ms and
    ///     then put back into the main queue.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="queueParams"></param>
    /// <param name="timeToRetry">Time (ms) before a message is returned from the retry queue</param>
    public static void QueueDeclareWithRepeat(this IModel model, QueuesParams queueParams, int timeToRetry)
    {
        var toMainQueueKey = $"TO_{queueParams.MainQueueName}_QUEUE_KEY";
        var toRepeatQueueKey = $"TO_{queueParams.RepeatedQueueName}_QUEUE_KEY";

        model.ExchangeDeclare(queueParams.ExchangeQueueName, "direct", true, false, null);

        model.QueueDeclare(queueParams.MainQueueName, true, false, false, new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", queueParams.ExchangeQueueName },
            { "x-dead-letter-routing-key", toRepeatQueueKey }
        });

        model.QueueDeclare(queueParams.RepeatedQueueName, true, false, false, new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", queueParams.ExchangeQueueName },
            { "x-dead-letter-routing-key", toMainQueueKey },
            { "x-message-ttl", timeToRetry }
        });

        model.QueueBind(queueParams.RepeatedQueueName, queueParams.ExchangeQueueName,
            toRepeatQueueKey);
        model.QueueBind(queueParams.MainQueueName, queueParams.ExchangeQueueName, toMainQueueKey);
    }
}
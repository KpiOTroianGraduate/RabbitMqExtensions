using RabbitMQ.Client.RepeatQueues.Models;

namespace RabbitMQ.Client.RepeatQueues;

public static class QueueDeclareExtensions
{
    public static void QueueDeclareWithRepeat(this IModel model, QueuesParams queriesParams, int timeToRetry)
    {
        var toMainQueueKey = $"TO_{queriesParams.MainQueueName}_QUEUE_KEY";
        var toRepeatQueueKey = $"TO_{queriesParams.RepeatedQueueName}_QUEUE_KEY";

        model.ExchangeDeclare(queriesParams.ExchangeQueueName, "direct", true, false, null);

        model.QueueDeclare(queriesParams.MainQueueName, true, false, false, new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", queriesParams.ExchangeQueueName },
            { "x-dead-letter-routing-key", toRepeatQueueKey }
        });

        model.QueueDeclare(queriesParams.RepeatedQueueName, true, false, false, new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", queriesParams.ExchangeQueueName },
            { "x-dead-letter-routing-key", toMainQueueKey },
            { "x-message-ttl", timeToRetry }
        });

        model.QueueBind(queriesParams.RepeatedQueueName, queriesParams.ExchangeQueueName,
            toRepeatQueueKey);
        model.QueueBind(queriesParams.MainQueueName, queriesParams.ExchangeQueueName, toMainQueueKey);
    }
}
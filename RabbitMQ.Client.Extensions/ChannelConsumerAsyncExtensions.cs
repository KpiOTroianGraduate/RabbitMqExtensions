using RabbitMQ.Client.Events;
using RabbitMQ.Client.Extensions.RepeatQueues;

namespace RabbitMQ.Client.Extensions;

public static class ChannelConsumerAsyncExtensions
{
    public delegate Task ConsumerAction(BasicDeliverEventArgs ea);

    /// <summary>
    ///     Setup consumer to queue
    /// </summary>
    /// <param name="model"></param>
    /// <param name="queueName">Queue name</param>
    /// <param name="consumerAction">Action to do with message</param>
    /// <param name="useRequeue">Use requeue if action throws</param>
    /// <param name="repeatTime">Use delay in ms if throws</param>
    public static void SetConsumerAsync(this IModel model, string queueName, ConsumerAction consumerAction,
        bool useRequeue = true, int repeatTime = 0)
    {
        var queueParams = model.SetupQueueNames(queueName);
        var useRepeatQueue = repeatTime > 0;

        if (useRepeatQueue)
            model.QueueDeclareWithRepeat(queueParams, repeatTime);
        else
            model.QueueDeclare(queueParams.MainQueueName, true, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(model);
        consumer.Received += async (sender, @event) =>
        {
            try
            {
                await consumerAction(@event);

                model.BasicAck(@event.DeliveryTag, false);
            }
            catch
            {
                model.BasicNack(@event.DeliveryTag, false, !useRepeatQueue && useRequeue);
            }
        };
    }
}
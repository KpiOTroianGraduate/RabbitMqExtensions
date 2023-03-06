using RabbitMQ.Client.Extensions.Models;

namespace RabbitMQ.Client.Extensions;

public static class QueueSetupNamesExtensions
{
    /// <summary>
    ///     Sets correct names for outbound queues
    /// </summary>
    /// <param name="_"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static QueuesParams SetupQueueNames(this IModel _, string name)
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
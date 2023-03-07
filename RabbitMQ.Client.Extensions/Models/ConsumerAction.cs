using RabbitMQ.Client.Events;

namespace RabbitMQ.Client.Extensions.Models;

public delegate Task ConsumerAction(BasicDeliverEventArgs ea);
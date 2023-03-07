using Moq;
using RabbitMQ.Client.Extensions.Models;
using RabbitMQ.Client.Extensions.RepeatQueues;
using Xunit;

namespace RabbitMQ.Client.Extensions.Tests;

public class QueueDeclareTests
{
    private readonly Mock<IModel> _modelMock;

    public QueueDeclareTests()
    {
        _modelMock = new Mock<IModel>();
    }

    [Fact]
    public void QueueDeclareWithRepeat_ShouldSetupWithCorrectValues()
    {
        //Arrange
        var param = new QueuesParams
        {
            ExchangeQueueName = "ExchangeQueueName",
            MainQueueName = "MainQueueName",
            RepeatedQueueName = "RepeatedQueueName"
        };
        var toMainQueueKey = $"TO_{param.MainQueueName}_QUEUE_KEY";
        var toRepeatQueueKey = $"TO_{param.RepeatedQueueName}_QUEUE_KEY";

        //Act
        _modelMock.Object.QueueDeclareWithRepeat(param, int.MaxValue);

        //Assert
        _modelMock.Verify(
            m => m.ExchangeDeclare(It.Is<string>(s => s.Equals(param.ExchangeQueueName)), It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        _modelMock.Verify(
            m => m.QueueDeclare(It.Is<string>(s => s.Equals(param.MainQueueName)), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        _modelMock.Verify(
            m => m.QueueDeclare(It.Is<string>(s => s.Equals(param.RepeatedQueueName)), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Once());
        _modelMock.Verify(
            m => m.QueueBind(It.Is<string>(s => s.Equals(param.RepeatedQueueName)),
                It.Is<string>(s => s.Equals(param.ExchangeQueueName)),
                It.Is<string>(s => s.Equals(toRepeatQueueKey)), It.IsAny<Dictionary<string, object>>()), Times.Once());
        _modelMock.Verify(
            m => m.QueueBind(It.Is<string>(s => s.Equals(param.MainQueueName)),
                It.Is<string>(s => s.Equals(param.ExchangeQueueName)),
                It.Is<string>(s => s.Equals(toMainQueueKey)), It.IsAny<Dictionary<string, object>>()), Times.Once());
    }
}
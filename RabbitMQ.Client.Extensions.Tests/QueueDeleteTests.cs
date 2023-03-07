using Moq;
using RabbitMQ.Client.Extensions.Models;
using RabbitMQ.Client.Extensions.RepeatQueues;
using Xunit;

namespace RabbitMQ.Client.Extensions.Tests;

public class QueueDeleteTests
{
    private readonly Mock<IModel> _modelMock;

    //public QueueDeleteTests()
    //{
    //    _modelMock = new Mock<IModel>();
    //}

    //[Fact]
    //public void QueueDeleteWithRepeat_ShouldDeleteByValues()
    //{
    //    //Arrange
    //    var param = new QueuesParams
    //    {
    //        ExchangeQueueName = "ExchangeQueueName",
    //        MainQueueName = "MainQueueName",
    //        RepeatedQueueName = "RepeatedQueueName"
    //    };

    //    //Act
    //    _modelMock.Object.QueueDeleteWithRepeat(param);

    //    //Assert
    //    _modelMock.Verify(
    //        m => m.ExchangeDelete(It.Is<string>(s => s.Equals(param.ExchangeQueueName)), It.IsAny<bool>()),
    //        Times.Once());
    //    _modelMock.Verify(
    //        m => m.QueueDelete(It.Is<string>(s => s.Equals(param.MainQueueName)), It.IsAny<bool>(),
    //            It.IsAny<bool>()), Times.Once());
    //    _modelMock.Verify(
    //        m => m.QueueDelete(It.Is<string>(s => s.Equals(param.RepeatedQueueName)), It.IsAny<bool>(),
    //            It.IsAny<bool>()), Times.Once());
    //}
}
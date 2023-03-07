using Moq;
using RabbitMQ.Client.Extensions.Models;
using Xunit;

namespace RabbitMQ.Client.Extensions.Tests;

public class ChannelConsumerAsyncTests
{
    private readonly Mock<IModel> _modelMock;
    private ConsumerAction _consumerAction;

    public ChannelConsumerAsyncTests()
    {
        _consumerAction = async ea => await Task.Yield();
        _modelMock = new Mock<IModel>();
    }

    [Fact]
    public async Task SetConsumerAsync_ShouldCallBasicAck_WithTwoQueues()
    {
        //Arrange
        const string queueName = "queueName";

        //Act
        var result = _modelMock.Object.SetConsumerAsync(queueName, _consumerAction, true, int.MaxValue);
        await result.HandleBasicDeliver(It.IsAny<string>(), It.IsAny<ulong>(), It.IsAny<bool>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(),
            It.IsAny<ReadOnlyMemory<byte>>());

        //Assert
        _modelMock.Verify(m => m.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
            It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        Assert.NotNull(result);
        _modelMock.Verify(m => m.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Once);
        _modelMock.Verify(m => m.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.Is<bool>(r => !r)), Times.Never);
    }

    [Fact]
    public async Task SetConsumerAsync_ShouldCallBasicNack_WithNoRequeue_WhenActionThrows()
    {
        //Arrange
        _consumerAction = ea => throw new Exception();
        const string queueName = "queueName";

        //Act
        var result = _modelMock.Object.SetConsumerAsync(queueName, _consumerAction, true, int.MaxValue);
        await result.HandleBasicDeliver(It.IsAny<string>(), It.IsAny<ulong>(), It.IsAny<bool>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(),
            It.IsAny<ReadOnlyMemory<byte>>());

        //Assert
        _modelMock.Verify(m => m.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
            It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Exactly(2));
        Assert.NotNull(result);
        _modelMock.Verify(m => m.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Never);
        _modelMock.Verify(m => m.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.Is<bool>(r => !r)), Times.Once);
    }

    [Fact]
    public async Task SetConsumerAsync_ShouldCallBasicAck_WithOneQueue()
    {
        //Arrange
        const string queueName = "queueName";

        //Act
        var result = _modelMock.Object.SetConsumerAsync(queueName, _consumerAction, false);
        await result.HandleBasicDeliver(It.IsAny<string>(), It.IsAny<ulong>(), It.IsAny<bool>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(),
            It.IsAny<ReadOnlyMemory<byte>>());

        //Assert
        _modelMock.Verify(m => m.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
            It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        Assert.NotNull(result);
        _modelMock.Verify(m => m.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Once);
        _modelMock.Verify(m => m.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.Is<bool>(r => !r)), Times.Never);
    }

    [Fact]
    public async Task SetConsumerAsync_ShouldCallBasicAck_WithoutRequeue()
    {
        //Arrange
        const string queueName = "queueName";
        _consumerAction = ea => throw new Exception();

        //Act
        var result = _modelMock.Object.SetConsumerAsync(queueName, _consumerAction, false);
        await result.HandleBasicDeliver(It.IsAny<string>(), It.IsAny<ulong>(), It.IsAny<bool>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(),
            It.IsAny<ReadOnlyMemory<byte>>());

        //Assert
        _modelMock.Verify(m => m.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
            It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        Assert.NotNull(result);
        _modelMock.Verify(m => m.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Never);
        _modelMock.Verify(m => m.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.Is<bool>(r => !r)), Times.Once);
    }

    [Fact]
    public async Task SetConsumerAsync_ShouldCallBasicAck_WithRequeue()
    {
        //Arrange
        const string queueName = "queueName";
        _consumerAction = ea => throw new Exception();

        //Act
        var result = _modelMock.Object.SetConsumerAsync(queueName, _consumerAction);
        await result.HandleBasicDeliver(It.IsAny<string>(), It.IsAny<ulong>(), It.IsAny<bool>(),
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IBasicProperties>(),
            It.IsAny<ReadOnlyMemory<byte>>());

        //Assert
        _modelMock.Verify(m => m.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
            It.IsAny<bool>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
        Assert.NotNull(result);
        _modelMock.Verify(m => m.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()), Times.Never);
        _modelMock.Verify(m => m.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.Is<bool>(r => r)), Times.Once);
    }
}
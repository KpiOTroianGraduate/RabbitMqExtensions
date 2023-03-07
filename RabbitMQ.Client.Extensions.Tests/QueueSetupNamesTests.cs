using Moq;
using Xunit;

namespace RabbitMQ.Client.Extensions.Tests;

public class QueueSetupNamesTests
{
    private readonly Mock<IModel> _modelMock;

    public QueueSetupNamesTests()
    {
        _modelMock = new Mock<IModel>();
    }

    [Fact]
    public void SetupQueueNames_ShouldReturnCorrectValues()
    {
        //Arrange
        const string queueName = "queueName";

        //Act
        var result = _modelMock.Object.SetupQueueNames(queueName);

        //Assert
        Assert.Multiple(
            () => Assert.Equal($"EXCHANGE_{queueName}".ToUpper(), result.ExchangeQueueName),
            () => Assert.Equal($"OUTBOUND_{queueName}".ToUpper(), result.MainQueueName),
            () => Assert.Equal($"OUTBOUND_{queueName}_REPEATED".ToUpper(), result.RepeatedQueueName));
    }
}
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xena.HttpClient.Interceptors;

namespace Xena.Tests.HttpClient.Interceptors;

public class LoggerXenaHttpClientInterceptorTests
{
    [Fact]
    public async Task Intercept_ShouldLogRequestAndResponse()
    {
        // arrange
        var loggerMock = new Mock<ILogger<LoggerXenaHttpClientInterceptor>>();

        var sut = new LoggerXenaHttpClientInterceptor(loggerMock.Object);
        var request = new HttpRequestMessage(HttpMethod.Get, "test");
        var response = new HttpResponseMessage(HttpStatusCode.OK);

        // act
        await sut.Intercept(request, r => Task.FromResult(response));

        // assert
        loggerMock.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == $"Calling on address {request.RequestUri} (method {request.Method})" && @type.Name == "FormattedLogValues"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Once);

        loggerMock.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == $"Http response: {response.StatusCode}" && @type.Name == "FormattedLogValues"),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        Times.Once);
    }
}

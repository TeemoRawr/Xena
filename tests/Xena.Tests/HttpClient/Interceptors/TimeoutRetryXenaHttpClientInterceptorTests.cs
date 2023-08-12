using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xena.HttpClient.Interceptors;

namespace Xena.Tests.HttpClient.Interceptors;

public class TimeoutRetryXenaHttpClientInterceptorTests
{
    [Fact]
    public async Task Intercept_ShouldHandleTimeoutAndRetry()
    {
        // arrange
        var counter = 0;
        var loggerMock = new Mock<ILogger<TimeoutRetryXenaHttpClientInterceptor>>();

        var sut = new TimeoutRetryXenaHttpClientInterceptor(loggerMock.Object);

        var httpRequestMessage = new HttpRequestMessage();
        Func<HttpRequestMessage, Task<HttpResponseMessage>> func = r =>
        {
            if(counter == 0)
            {
                counter++;

                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.RequestTimeout
                });
            }

            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });
        };

        // act
        var result = await sut.Intercept(httpRequestMessage, func);

        // assert
        result.StatusCode.Should().Be(HttpStatusCode.OK); 
    }
}

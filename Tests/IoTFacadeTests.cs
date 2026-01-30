using Moq;
using System.Net;
using SmartAppMain.Services;

public class MockHttpMessageHandler : DelegatingHandler
{
    private readonly HttpResponseMessage _response;

    public MockHttpMessageHandler(HttpResponseMessage response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Method == HttpMethod.Post) return Task.FromResult(_response);

        if (request.Method == HttpMethod.Get) return Task.FromResult(_response);

        return base.SendAsync(request, cancellationToken);
    }
}

public class IoTFacadeTests
{
    private readonly Mock<IHttpClientFactory> _mockFactory;
    private readonly IoTFacade _facade;

    public IoTFacadeTests()
    {
        _mockFactory = new Mock<IHttpClientFactory>();
        _facade = new IoTFacade(_mockFactory.Object);
    }

    private HttpClient SetupHttpClient(HttpResponseMessage response, string clientName)
    {
        var handler = new MockHttpMessageHandler(response);
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://dummy-base-address.com/")
        };

        _mockFactory.Setup(f => f.CreateClient(clientName))
                    .Returns(httpClient);

        return httpClient;
    }

    [Fact]
    public void GetString_CallsCorrectClient_ReturnsExpectedContent()
    {
        const string clientName = "testClient";
        const string expectedContent = "{\"status\":\"ok\"}";

        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(expectedContent)
        };
        SetupHttpClient(httpResponse, clientName);

        var result = _facade.GetString(clientName, "api/status");

        Assert.Equal(expectedContent, result);
        _mockFactory.Verify(f => f.CreateClient(clientName), Times.Once);
    }

    [Fact]
    public void GetString_HttpClientReturnsNotFound_ThrowsHttpRequestException()
    {
        const string clientName = "testClient";

        var httpResponse = new HttpResponseMessage(HttpStatusCode.NotFound);
        SetupHttpClient(httpResponse, clientName);

        Assert.Throws<HttpRequestException>(() => _facade.GetString(clientName, "api/data"));
    }

    [Fact]
    public void Post_CallsCorrectClient_ReturnsSuccessMessage()
    {
        const string clientName = "light";

        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        SetupHttpClient(expectedResponse, clientName);

        var result = _facade.Post(clientName, "light/power/on");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.True(result.IsSuccessStatusCode);
        _mockFactory.Verify(f => f.CreateClient(clientName), Times.Once);
    }

    [Fact]
    public void Post_HttpClientReturnsServerError_ReturnsFailureMessage()
    {
        const string clientName = "curtains";

        var expectedResponse = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        SetupHttpClient(expectedResponse, clientName);

        var result = _facade.Post(clientName, "curtains/power/open");

        Assert.Equal(HttpStatusCode.ServiceUnavailable, result.StatusCode);
        Assert.False(result.IsSuccessStatusCode);
        _mockFactory.Verify(f => f.CreateClient(clientName), Times.Once);
    }
}
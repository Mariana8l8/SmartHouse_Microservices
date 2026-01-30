namespace SmartAppMain.Services
{
    public class IoTFacade
    {
        private readonly IHttpClientFactory _http;
        public IoTFacade(IHttpClientFactory http) => _http = http;

        public string GetString(string clientName, string url)
            => _http.CreateClient(clientName).GetStringAsync(url).GetAwaiter().GetResult();

        public HttpResponseMessage Post(string clientName, string url)
            => _http.CreateClient(clientName).PostAsync(url, null).GetAwaiter().GetResult();
    }
}

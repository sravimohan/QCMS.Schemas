namespace QCMS.Schemas.Api.Services
{
    internal abstract class HttpClientService
    {
        protected abstract string ClientName { get; }

        readonly IHttpClientFactory _httpClientFactory;

        HttpClient HttpClientInstance() => _httpClientFactory.CreateClient(ClientName);

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        internal async Task<T?> Get<T>(string url, CancellationToken cancellationToken = default)
        {
            //var response = await HttpClientInstance().GetFromJsonAsync<T>(url, cancellationToken: cancellationToken);
            var httpClient = HttpClientInstance();
            var response = await httpClient.GetAsync(url, cancellationToken: cancellationToken);
            var json = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

            return json;
        }

        internal async Task<TOut?> Post<TIn, TOut>(string url, TIn request, CancellationToken cancellationToken = default)
        {
            var response = await HttpClientInstance().PostAsJsonAsync(url, request, cancellationToken);
            if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                throw new Exception("Invalid Request");

            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken: cancellationToken);
        }
    }
}
 
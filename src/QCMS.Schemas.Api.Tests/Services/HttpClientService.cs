using QCMS.Security.Authorization;
using System.Net.Http.Json;

namespace QCMS.Schemas.Api.Tests.Services
{
    internal abstract class HttpClientService
    {
        readonly HttpClient _httpClient;

        protected abstract string ClientName { get; }

        public HttpClientService(IHttpClientFactory httpClientFactory, TenantScope tenantScope)
        {
            _httpClient = httpClientFactory.CreateClient(ClientName);
            _httpClient.DefaultRequestHeaders.Add("Authorization", tenantScope.AuthorizationToken);
        }

        internal async Task<T?> Get<T>(string url, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(url, cancellationToken: cancellationToken);
            var json = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);

            return json;
        }

        internal async Task<TOut?> Post<TIn, TOut>(string url, TIn request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
            if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                throw new Exception("Invalid Request");

            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken: cancellationToken);
        }

        internal async Task<TOut?> Put<TIn, TOut>(string url, TIn request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync(url, request, cancellationToken);
            if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                throw new Exception("Invalid Request");

            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken: cancellationToken);
        }
    }
}

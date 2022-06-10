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

        internal async Task<TOut?> PostAsJsonAsync<TIn, TOut>(string url, TIn request, CancellationToken cancellation = default)
        {
            var response = await _httpClient.PostAsJsonAsync(url, request, cancellation);
            if (response == null || !response.IsSuccessStatusCode || response.Content == null)
                throw new Exception("Invalid Request");

            return await response.Content.ReadFromJsonAsync<TOut>(cancellationToken: cancellation);
        }
    }
}

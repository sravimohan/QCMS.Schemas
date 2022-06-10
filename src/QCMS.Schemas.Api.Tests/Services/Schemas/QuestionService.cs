using QCMS.Security.Authorization;

namespace QCMS.Schemas.Api.Tests.Services.Schemas
{
    internal class SchemaService : HttpClientService
    {
        protected override string ClientName => HttpClientName.Questions;

        public SchemaService(IHttpClientFactory httpClientFactory, TenantScope tenantScope)
            : base(httpClientFactory, tenantScope)
        { }
    }
}
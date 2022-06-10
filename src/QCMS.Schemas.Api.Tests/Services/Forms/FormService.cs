using QCMS.Security.Authorization;

namespace QCMS.Schemas.Api.Tests.Services.Forms;

internal class FormService : HttpClientService
{
    protected override string ClientName => HttpClientName.Forms;

    public FormService(IHttpClientFactory httpClientFactory, TenantScope tenantScope)
        : base(httpClientFactory, tenantScope)
    { }
}
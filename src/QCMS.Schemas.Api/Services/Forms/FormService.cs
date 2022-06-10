using QCMS.Schemas.Api.Services.Forms.Contract;
using QCMS.Security.Authorization;

namespace QCMS.Schemas.Api.Services.Forms;

internal class FormService : HttpClientService
{
    protected override string ClientName => HttpClientName.Forms;

    public FormService(IHttpClientFactory httpClientFactory, TenantScope tenantScope)
        : base(httpClientFactory, tenantScope)
    { }

    public async Task<FormResponse> GetForm(string formId, long? version, CancellationToken cancellationToken = default)
    {
        var url = version.HasValue ? $"{formId}/{version}" : $"{formId}";
        var formResponse = await Get<FormResponse>(url, cancellationToken);
        return formResponse ?? throw new Exception("Invalid Form Id");
    }
}

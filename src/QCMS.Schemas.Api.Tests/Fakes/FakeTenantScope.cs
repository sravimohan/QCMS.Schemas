using QCMS.Security.Authorization;

namespace QCMS.Schemas.Api.Tests.Fakes;

internal class FakeTenantScope : TenantScope
{
    public FakeTenantScope(string tenantId, string username, string authorizationToken)
    {
        TenantId = tenantId;
        Username = username;
        AuthorizationToken = authorizationToken;
    }
}

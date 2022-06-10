using QCMS.Security.Authorization;

namespace QCMS.Schemas.Api.Tests.Services.Questions
{
    internal class QuestionService : HttpClientService
    {
        protected override string ClientName => HttpClientName.Questions;

        public QuestionService(IHttpClientFactory httpClientFactory, TenantScope tenantScope)
            : base(httpClientFactory, tenantScope)
        { }
    }
}
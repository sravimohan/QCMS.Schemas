using QCMS.Schemas.Api.Services.Questions.Contract;
using QCMS.Security.Authorization;

namespace QCMS.Schemas.Api.Services.Questions
{
    internal class QuestionService : HttpClientService
    {
        protected override string ClientName => HttpClientName.Questions;

        public QuestionService(IHttpClientFactory httpClientFactory)
            : base(httpClientFactory)
        { }

        public async Task<IEnumerable<Question>> GetQuestions(
            IEnumerable<QuestionKey> questionKeys,
            CancellationToken cancellationToken = default)
        {
            var request = new GetBatchRequest(questionKeys);
            var response = await Post<GetBatchRequest, GetBatchResponse>("get", request, cancellationToken);
            return response?.Questions ?? Enumerable.Empty<Question>();
        }
    }
}

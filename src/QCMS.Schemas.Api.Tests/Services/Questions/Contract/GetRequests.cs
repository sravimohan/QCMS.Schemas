namespace QCMS.Schemas.Api.Tests.Services.Questions.Contract
{
    public record QuestionKey(string QuestionId, long? Version = null);

    public record GetBatchRequest(QuestionKey[] GetRequests);

    public record GetBatchResponse(IEnumerable<Question> Questions);
}
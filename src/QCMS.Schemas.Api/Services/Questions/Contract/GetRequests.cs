namespace QCMS.Schemas.Api.Services.Questions.Contract
{
    public record QuestionKey(string QuestionId, long? Version = null);

    public record GetBatchRequest(IEnumerable<QuestionKey> GetRequests);

    public record GetBatchResponse(IEnumerable<Question> Questions);
}

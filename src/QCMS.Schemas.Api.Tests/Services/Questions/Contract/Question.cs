namespace QCMS.Schemas.Api.Tests.Services.Questions.Contract;

public record BatchPutRequest(IEnumerable<Question> Questions);

public record BatchPutResponse(IEnumerable<string> QuestionIds);

public record Question
{
    public string QuestionId { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string Type { get; init; } = default!;

    public string Format { get; init; } = default!;

    public string? Default { get; init; } = default!;

    public IEnumerable<EnumValue>? EnumValues { get; init; } = default!;
}

public record QuestionResponse : Question
{
    public long Version { get; init; }

    public AuditDetails AuditDetails { get; init; } = default!;

    public Question AsQuestion() =>
        new()
        {
            QuestionId = QuestionId,
            Title = Title,
            Type = Type,
            Format = Format,
            Default = Default,
            EnumValues = EnumValues
        };
}

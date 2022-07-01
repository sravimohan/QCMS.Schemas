using QCMS.Schemas.Api.Services.Questions.Contract;

namespace QCMS.Schemas.Api.UseCases.BuildFormObject.Contract;

public class Property
{
    public string Id { get; init; } = default!;
    public long Version { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string Type { get; init; } = default!;

    public string Format { get; init; } = default!;

    public string? Default { get; init; }

    public string[]? Enum { get; init; }
    public string[]? EnumNames { get; init; }

    public Property() { }

    internal Property(Question question)
    {
        Id = question.QuestionId;
        Title = question.Title;
        Type = question.Type;
        Format = question.Format;
        Default = question.Default;
        Enum = question.EnumValues?.Select(e => e.Enum).ToArray();
        EnumNames = question.EnumValues?.Select(e => e.EnumName).ToArray();
        Version = Version;
    }
}

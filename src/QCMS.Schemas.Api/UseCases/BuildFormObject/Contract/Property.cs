using QCMS.Schemas.Api.Services.Questions.Contract;

namespace QCMS.Schemas.Api.UseCases.BuildFormObject.Contract;

internal class Property
{
    internal string Id { get; init; } = default!;
    internal long Version { get; init; } = default!;

    internal string Title { get; init; } = default!;

    internal string Type { get; init; } = default!;

    internal string Format { get; init; } = default!;

    internal string? Default { get; init; }

    internal string[]? Enum { get; init; }
    internal string[]? EnumNames { get; init; }

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

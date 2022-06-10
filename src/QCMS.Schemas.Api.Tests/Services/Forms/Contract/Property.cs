namespace QCMS.Schemas.Api.Tests.Services.Forms.Contract;

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
}
namespace QCMS.Schemas.Api.Tests.Services.Forms.Contract;

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
}
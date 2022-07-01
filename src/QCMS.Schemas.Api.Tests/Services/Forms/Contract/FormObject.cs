namespace QCMS.Schemas.Api.Tests.Services.Forms.Contract;

public record FormObject
{
    public string FormId { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string Type => "object";

    public List<string> Required { get; init; } = default!;

    public Dictionary<string, Property> Properties { get; init; } = default!;

    public long Version { get; init; } = default!;
}
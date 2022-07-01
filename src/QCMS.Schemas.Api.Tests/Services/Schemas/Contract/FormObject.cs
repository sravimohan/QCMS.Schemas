namespace QCMS.Schemas.Api.Tests.Services.Schemas.Contract;

internal record FormObject
{
    internal string FormId { get; init; } = default!;

    internal string Title { get; init; } = default!;

    internal string Type => "object";

    internal List<string> Required { get; init; } = default!;

    internal Dictionary<string, Property> Properties { get; init; } = default!;

    internal long Version { get; init; } = default!;
}

namespace QCMS.Schemas.Api.Services.Forms.Contract;

public record Form
{
    public string FormId { get; init; } = default!;

    public string Title { get; init; } = default!;

    internal List<FormItem> FormItems { get; init; } = default!;
}

public record FormItem
{
    public string Id { get; init; } = default!;
    public string Type { get; init; } = default!;
    public int Order { get; init; } = default!;
}

public record FormResponse : Form
{
    public long Version { get; init; }

    public AuditDetails AuditDetails { get; init; } = default!;

    public Form AsForm() =>
        new()
        {
            FormId = FormId,
            Title = Title,
            FormItems = FormItems
        };
}

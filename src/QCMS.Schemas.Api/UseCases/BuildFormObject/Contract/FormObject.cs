using QCMS.Schemas.Api.Services.Forms.Contract;

namespace QCMS.Schemas.Api.UseCases.BuildFormObject.Contract;

public record FormObject
{
    public string FormId { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string Type => "object";

    public List<string> Required { get; init; } = default!;

    public Dictionary<string, Property> Properties { get; init; } = default!;

    public long Version { get; init; } = default!;

    public FormObject() { }

    internal FormObject(FormResponse form)
    {
        FormId = form.FormId;
        Title = form.Title;
        Version = form.Version;
    }
}

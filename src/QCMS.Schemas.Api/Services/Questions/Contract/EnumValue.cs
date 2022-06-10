namespace QCMS.Schemas.Api.Services.Questions.Contract;

public record EnumValue
{
    public string Enum { get; init; } = default!;
    public string EnumName { get; init; } = default!;

    public EnumValue() { }

    public EnumValue(string @enum, string enumValue)
    {
        Enum = @enum;
        EnumName = enumValue;
    }
}
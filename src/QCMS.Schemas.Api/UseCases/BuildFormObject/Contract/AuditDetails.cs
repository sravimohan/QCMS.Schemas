namespace QCMS.Schemas.Api.UseCases.BuildFormObject.Contract;

public record AuditDetails(
    string CreatedBy,
    DateTime CreatedOn,
    string UpdatedBy,
    DateTime UpdatedOn
);

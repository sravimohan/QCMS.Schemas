namespace QCMS.Schemas.Api.Services.Questions.Contract;

public record AuditDetails(
    string CreatedBy,
    DateTime CreatedOn,
    string UpdatedBy,
    DateTime UpdatedOn
);
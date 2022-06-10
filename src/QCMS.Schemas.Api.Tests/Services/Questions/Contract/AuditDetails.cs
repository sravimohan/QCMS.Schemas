namespace QCMS.Schemas.Api.Tests.Services.Questions.Contract;

public record AuditDetails(
    string CreatedBy,
    DateTime CreatedOn,
    string UpdatedBy,
    DateTime UpdatedOn
);
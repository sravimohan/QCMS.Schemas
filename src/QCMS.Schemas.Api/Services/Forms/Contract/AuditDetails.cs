namespace QCMS.Schemas.Api.Services.Forms.Contract;

public record AuditDetails(
    string CreatedBy,
    DateTime CreatedOn,
    string UpdatedBy,
    DateTime UpdatedOn
);
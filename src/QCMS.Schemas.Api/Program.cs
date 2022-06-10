using QCMS.Schemas.Api.ErrorHandling;
using QCMS.Schemas.Api.Services;
using QCMS.Schemas.Api.Services.Forms;
using QCMS.Schemas.Api.Services.Questions;
using QCMS.Schemas.Api.UseCases.BuildFormObject;
using QCMS.Security.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddTenantAuthorization(builder.Configuration);
builder.Services.Configure<JsonOptions>(opt =>
{
    opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddHttpClient(HttpClientName.Questions, httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:6010/");
});

builder.Services.AddHttpClient(HttpClientName.Forms, httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:6020/");
});

// services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TenantScope, TenantScopeFromJwt>();
builder.Services.AddScoped<FormService>();
builder.Services.AddScoped<QuestionService>();
builder.Services.AddScoped<BuildFormObjectHandler>();

var app = builder.Build();
app.MapHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();


app.MapGet("/{formId}", async (BuildFormObjectHandler handler, string formId, CancellationToken cancellationToken) =>
{
    var form = await handler.BuildFormObject(formId, cancellationToken: cancellationToken);
    return Results.Ok(form);
});

app.MapGet("/{formId}/{version}", async (BuildFormObjectHandler handler, string formId, long version, CancellationToken cancellationToken) =>
{
    var form = await handler.BuildFormObject(formId, version, cancellationToken);
    return Results.Ok(form);
});

app.Run();

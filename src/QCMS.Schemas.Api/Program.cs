using Microsoft.AspNetCore.Authentication;
using QCMS.Schemas.Api.ErrorHandling;
using QCMS.Schemas.Api.Services;
using QCMS.Schemas.Api.Services.Forms;
using QCMS.Schemas.Api.Services.Questions;
using QCMS.Schemas.Api.UseCases.BuildFormObject;
using QCMS.Security.Authorization;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddTenantAuthorization(builder.Configuration);
builder.Services.Configure<JsonOptions>(opt =>
{
    opt.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddHttpClient(HttpClientName.Questions, httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:6000/");
}).AddHttpMessageHandler<AuthHeaderHandler>(); ;

builder.Services.AddHttpClient(HttpClientName.Forms, httpClient =>
{
    httpClient.BaseAddress = new Uri("http://localhost:6010/");
}).AddHttpMessageHandler<AuthHeaderHandler>(); ;

// services
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TenantScope, TenantScopeFromJwt>();
builder.Services.AddTransient<BuildFormObjectHandler>();

builder.Services.AddTransient<FormService>();
builder.Services.AddTransient<QuestionService>();
builder.Services.AddTransient<AuthHeaderHandler>();

var app = builder.Build();
app.MapHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<GlobalErrorHandlingMiddleware>();


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

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            string? accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}

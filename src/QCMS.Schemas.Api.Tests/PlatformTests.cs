using Microsoft.Extensions.DependencyInjection;
using QCMS.Schemas.Api.Tests.Fakes;
using QCMS.Schemas.Api.Tests.Services;
using QCMS.Schemas.Api.Tests.Services.Forms;
using QCMS.Schemas.Api.Tests.Services.Forms.Contract;
using QCMS.Schemas.Api.Tests.Services.Questions;
using QCMS.Schemas.Api.Tests.Services.Questions.Contract;
using System.Diagnostics;
using System.Text.Json;

namespace QCMS.Schemas.Api.Tests;

public class PlatformTests
{
    const string QuestionServiceBaseUrl = "http://localhost:6000";
    const string FormServiceBaseUrl = "http://localhost:6010";
    const string AuthorizationToken = "Bearer eyJraWQiOiJnQkJBSHE5VEVMSk5pQ0dZakQ3SVFyZ1FcL0YwZE9jVDcxeVwvOFJwdnZabkU9IiwiYWxnIjoiUlMyNTYifQ.eyJhdF9oYXNoIjoiVVRBVkZEU0RPb1FIQ2NWcmdFbklxQSIsInN1YiI6IjAzMDA2ZTIzLWRjMjctNDMyMi04Y2ZmLTY2Mzg1N2I2YTgyZCIsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC5hcC1zb3V0aGVhc3QtMi5hbWF6b25hd3MuY29tXC9hcC1zb3V0aGVhc3QtMl92MlFPZHNoeHIiLCJjb2duaXRvOnVzZXJuYW1lIjoiMDMwMDZlMjMtZGMyNy00MzIyLThjZmYtNjYzODU3YjZhODJkIiwib3JpZ2luX2p0aSI6IjU5MTI4MjFmLTY5NDMtNGZiYS1iZWIwLTBiNTUyZTM3NDVjNCIsImF1ZCI6IjZsaHE4bW1vazEzdG90ZGczam81YXNyZ2VjIiwidG9rZW5fdXNlIjoiaWQiLCJjdXN0b206VGVuYW50SWQiOiJiOGY5YmIwNC01ODQ2LTQ4MTItOGI1Ni02YTU0YjdiMDc4ZGQiLCJhdXRoX3RpbWUiOjE2NTQ3NTY4MjQsImV4cCI6MTY1NDkwNzkzNCwiaWF0IjoxNjU0OTA0MzM0LCJqdGkiOiJkZmJkOGNlZi00ZWU3LTQ1YzEtODNmOC01MGJmZDM5OWNjOTciLCJlbWFpbCI6InJhdmkubW9oYW5Acm9vbmdhLmNvbS5hdSJ9.UOsKnFSzDtdEXKQUsCLX7QeuHN73YfBFUaWbpau5XxP49SgXA9aXLjLE-Yzi8x0Ptvu7wpvNzB5-9p7kijNFVG5swU47btSMNwe0rPc7D2rKg6-nmILkUHR0w5yh-5aVKp3yUoPkCtcRkA_QuTrvLCU_N_DD_KRvZsiYWGIBinTo4ah9dEQbaiA9oI2scLv8a-OS15J2xGfbY_De0YQpUh5nIt_yZG1d9WhP2vdrw_cXUMSotOoyyYzd3hzbWR-wgqCb8_0cCgQ2O5hGMINB_o7iqL2AnYzSZk8d6mbQCOk3-AMCPlziWyU7DcjNRcWvic9ZFm14ZGxrd37y8xKFOw";

    [Fact]
     public async Task Can_create_schemaAsync()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddHttpClient(HttpClientName.Questions, httpClient =>
        {
            httpClient.BaseAddress = new Uri("http://localhost:6000/");
        });

        serviceCollection.AddHttpClient(HttpClientName.Forms, httpClient =>
        {
            httpClient.BaseAddress = new Uri("http://localhost:6010/");
        });

        string tenantId = "b8f9bb04-5846-4812-8b56-6a54b7b078dd";
        string username = $"test";

        var tenantScope = new FakeTenantScope(tenantId, username, AuthorizationToken);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        Debug.Assert(httpClientFactory != null);

        // create questions
        var questionService = new QuestionService(httpClientFactory, tenantScope);

        var newQuestion1 = new Question
        {
            QuestionId = Guid.NewGuid().ToString(),
            Title = "Name",
            Type = QuestionType.String,
        };

        var newQuestion2 = new Question
        {
            QuestionId = Guid.NewGuid().ToString(),
            Title = "State",
            Type = QuestionType.String,
            EnumValues = new List<EnumValue>
            {
                new EnumValue("NSW", "New South Wales"),
                new EnumValue("VIC", "Victoria"),
                new EnumValue("QLD", "Queensland"),
            }
        };

        var questionResponse1 = await questionService.PostAsJsonAsync<Question, QuestionResponse>(QuestionServiceBaseUrl, newQuestion1);
        AssertEqualObject(newQuestion1, questionResponse1?.AsQuestion());

        var questionResponse2 = await questionService.PostAsJsonAsync<Question, QuestionResponse>(QuestionServiceBaseUrl, newQuestion2);
        AssertEqualObject(newQuestion2, questionResponse2?.AsQuestion());

        // create form
        var newform = new Form
        {
            FormId = Guid.NewGuid().ToString(),
            Title = "Test Form",
            FormItems = new List<FormItem>
            {
                new FormItem{Id = newQuestion1.QuestionId, Type = "Question"},
                new FormItem{Id = newQuestion2.QuestionId, Type = "Question"}
            }
        };

        var formService = new FormService(httpClientFactory, tenantScope);
        var formResponse = await formService.PostAsJsonAsync<Form, FormResponse>(FormServiceBaseUrl, newform);
        AssertEqualObject(newform, formResponse?.AsForm());

        // get schema

    }

    static void AssertEqualObject<T>(T expected, T actual) =>
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
}

using Microsoft.Extensions.DependencyInjection;
using QCMS.Schemas.Api.Tests.Fakes;
using QCMS.Schemas.Api.Tests.Services;
using QCMS.Schemas.Api.Tests.Services.Forms;
using QCMS.Schemas.Api.Tests.Services.Forms.Contract;
using QCMS.Schemas.Api.Tests.Services.Questions;
using QCMS.Schemas.Api.Tests.Services.Questions.Contract;
using QCMS.Schemas.Api.Tests.Services.Schemas;
using System.Diagnostics;
using System.Text.Json;

namespace QCMS.Schemas.Api.Tests;

public class PlatformTests
{
    const string QuestionServiceBaseUrl = "http://localhost:6000";
    const string FormServiceBaseUrl = "http://localhost:6010";
    const string SchemaServiceBaseUrl = "http://localhost:6020";
    const string AuthorizationToken = "Bearer eyJraWQiOiJKMlJDa1pJckRIZmtha09JZVY1dCtnZVZEMUw4Y1lyNVY1VlRyZ3o4bXZvPSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJkNmIzOTM4OS00YjFiLTRmYzYtOTEzYS05MmZlMjQxNDVlNzQiLCJ6b25laW5mbyI6IkF1c3RyYWxpYVwvU3lkbmV5IiwiZW1haWxfdmVyaWZpZWQiOnRydWUsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC5hcC1zb3V0aGVhc3QtMi5hbWF6b25hd3MuY29tXC9hcC1zb3V0aGVhc3QtMl9tRUFwMW5EeG0iLCJjb2duaXRvOnVzZXJuYW1lIjoiZDZiMzkzODktNGIxYi00ZmM2LTkxM2EtOTJmZTI0MTQ1ZTc0IiwiZ2l2ZW5fbmFtZSI6IlJhdmkiLCJsb2NhbGUiOiJlbi1BVSIsIm9yaWdpbl9qdGkiOiJmMTk4NzVlYy02MmUzLTQ2NzQtOWQ2Mi03ZmJmYzJkNThjZmYiLCJhdWQiOiIzNm43bWtvNDY3ZHBtb3BjaW1uMTVrYnFyMyIsImV2ZW50X2lkIjoiZmY5NzcyODAtMmJlYi00MjM1LWFhYjgtMWNmNGFmYmI5MGZiIiwidG9rZW5fdXNlIjoiaWQiLCJjdXN0b206VGVuYW50SWQiOiJiOGY5YmIwNC01ODQ2LTQ4MTItOGI1Ni02YTU0YjdiMDc4ZGQiLCJhdXRoX3RpbWUiOjE2NTYyODIyNTAsImV4cCI6MTY1NjQ1MTAxOSwiaWF0IjoxNjU2NDQ3NDE5LCJmYW1pbHlfbmFtZSI6Ik1vaGFuIiwianRpIjoiYThhNDc1ZjQtMzhmMS00MGVmLTk2ODktMGZkZDA5MDU0N2JmIiwiZW1haWwiOiJzcmF2aW1vaGFuQGdtYWlsLmNvbSJ9.HScXHx_yF1gnNACXRfmltvmuo1DzoQXUi-DQ2cYjOlwfTkYi0ongJj9xyhcoyb4MI1anXjM2ygmF-KUE4tTmeM6U50HO4StMpksb6PjiKVZj0zmWeBuQinhGur6H1gqoAP7hsZqdUf0BzA5SdXQkwbEoYVu-eC0sdFWoDEUNtpwhYKbwVeGzD1RlhHseHffhSmxTgz1zStKAbUKq1_r7RmENNHID91UVAu6F7bXiUndKBTHM1APjqR2qs8lmNsa4yU4dc_qZgdiwwszbv6v8d-0X5jS9I6-1k_9lQoa88kxySFmhv8NZbwtB3OzFaqLn8tkcyot2s_V6l6AWsA0iGg";

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
            Title = "Name",
            Type = QuestionType.String,
        };

        var newQuestion2 = new Question
        {
            Title = "State",
            Type = QuestionType.String,
            EnumValues = new List<EnumValue>
            {
                new EnumValue("NSW", "New South Wales"),
                new EnumValue("VIC", "Victoria"),
                new EnumValue("QLD", "Queensland"),
            }
        };

        var batchPutRequest = new BatchPutRequest(new Question[] { newQuestion1, newQuestion2 });
        var batchPutResponse = await questionService.Put<BatchPutRequest, BatchPutResponse>(QuestionServiceBaseUrl, batchPutRequest);

        // create form
        var newform = new Form
        {
            FormId = Guid.NewGuid().ToString(),
            Title = "Test Form",
            FormItems = batchPutResponse?.QuestionIds.Select(qId => new FormItem
            {
                Id = qId,
                Type = "Question"
            }) ?? new List<FormItem>()
        };

        var formService = new FormService(httpClientFactory, tenantScope);
        var formResponse = await formService.Post<Form, FormResponse>(FormServiceBaseUrl, newform);
        AssertEqualObject(newform, formResponse?.AsForm());

        // get questions
        //var getQuestionResponse1 = await questionService.Get<QuestionResponse>($"{QuestionServiceBaseUrl}/{newQuestion1.QuestionId}");
        //AssertEqualObject(newQuestion1, getQuestionResponse1?.AsQuestion());

        //var getQuestionResponse2 = await questionService.Get<QuestionResponse>($"{QuestionServiceBaseUrl}/{newQuestion2.QuestionId}");
        //AssertEqualObject(newQuestion2, getQuestionResponse2?.AsQuestion());

        // get form
        var getFormResponse = await questionService.Get<FormResponse>($"{FormServiceBaseUrl}/{newform.FormId}");
        AssertEqualObject(newform, getFormResponse?.AsForm());

        // get schema
        var schemaService = new SchemaService(httpClientFactory, tenantScope);
        //failing here
        var schemaResponse = await schemaService.Get<FormObject>($"{SchemaServiceBaseUrl}/{newform.FormId}");
        Assert.NotNull(schemaResponse);
        Assert.Equal(newform.FormItems.Count(), schemaResponse?.Properties.Count);
    }

    static void AssertEqualObject<T>(T expected, T actual) =>
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
}

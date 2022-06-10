using QCMS.Schemas.Api.Services.Forms;
using QCMS.Schemas.Api.Services.Questions;
using QCMS.Schemas.Api.Services.Questions.Contract;
using QCMS.Schemas.Api.UseCases.BuildFormObject.Contract;

namespace QCMS.Schemas.Api.UseCases.BuildFormObject
{
    internal class BuildFormObjectHandler
    {
        readonly FormService _formService;
        readonly QuestionService _questionService;

        public BuildFormObjectHandler(FormService formService, QuestionService questionService)
        {
            _formService = formService;
            _questionService = questionService;
        }

        internal async Task<FormObject> BuildFormObject(string formId, long? version = null, CancellationToken cancellationToken = default)
        {
            var form = await _formService.GetForm(formId, version, cancellationToken);

            var questionKeys = form.FormItems.Select(i => new QuestionKey(i.Id));


            var formObject = new FormObject(form) with
            {
                Title = "Personal Details",
                Required = new List<string>() { },
                Properties = await GetFormProperties(questionKeys, cancellationToken)
            };

            return formObject;
        }

        private async Task<Dictionary<string, Property>> GetFormProperties(
            IEnumerable<QuestionKey> questionKeys, CancellationToken cancellationToken)
        {
            var properties = new Dictionary<string, Property>();
            if (questionKeys == null || !questionKeys.Any())
                return properties;

            var questions = await _questionService.GetQuestions(questionKeys, cancellationToken);
            foreach (var item in questions.Select(q => new Property(q)))
            {
                properties.Add($"p_{item.Id}_{item.Version}", item);
            }

            return properties;
        }
    }
}

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace EmployeeManagement
{
    public class UploadForm : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParameters = context.MethodInfo
                .GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile));

            if (!fileParameters.Any())
            {
                return;
            }

            if (operation.RequestBody == null)
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>()
                };
            }

            if (!operation.RequestBody.Content.ContainsKey("multipart/form-data"))
            {
                operation.RequestBody.Content["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>()
                    }
                };
            }

            var multipartSchema = operation.RequestBody.Content["multipart/form-data"].Schema;

            foreach (var parameter in fileParameters)
            {
                var parameterName = parameter.Name;

                if (operation.Parameters.Any(p => p.Name == parameterName))
                {
                    var param = operation.Parameters.First(p => p.Name == parameterName);
                    operation.Parameters.Remove(param);
                }

                multipartSchema.Properties[parameterName] = new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                };
                multipartSchema.Required.Add(parameterName);
            }
        }
    }
}

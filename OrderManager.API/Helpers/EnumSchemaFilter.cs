using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OrderManager.Shared.Helpers
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                // Add enum values as strings
                schema.Enum = Enum.GetNames(context.Type)
                    .Select(name => (IOpenApiAny)new OpenApiString(name))
                    .ToList();
                schema.Type = "string"; // Use string type in Swagger
            }
        }
    }
}

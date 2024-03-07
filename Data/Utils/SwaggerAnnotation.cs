using System;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace Data.Utils.SwaggerAnnotation {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Enum, AllowMultiple = false)]
    public class SwaggerSchemaExampleAttribute : Attribute {
        public object Example { get; }

        public SwaggerSchemaExampleAttribute(object example) {
            Example = example;
            }
        }

    public class SwaggerSchemaExampleFilter : ISchemaFilter {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context) {
            var schemaAttribute = context.MemberInfo?.GetCustomAttributes(true)
                .OfType<SwaggerSchemaExampleAttribute>()
                .FirstOrDefault();

            if (schemaAttribute != null) {
                schema.Example = ConvertToOpenApiType(schemaAttribute.Example);
                }
            }

        private IOpenApiAny ConvertToOpenApiType(object example) {
            if (example.GetType().IsEnum) {
                return new OpenApiString(example.ToString());
                }
            switch (example) {
                case int i:
                    return new OpenApiInteger(i);
                case bool b:
                    return new OpenApiBoolean(b);
                case string s:
                    return new OpenApiString(s);
                default:
                    throw new NotImplementedException($"Unsupported type: {example.GetType()}");
                }
            }
        }
    }

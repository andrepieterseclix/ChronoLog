using Swashbuckle.Swagger;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace ChronoLog.Host.WebApi.SwaggerOperationFilters
{
    /// <summary>
    /// Represents an operation filter to add the required authorization header parameters to operations on the Swagger UI page.
    /// </summary>
    /// <seealso cref="Swashbuckle.Swagger.IOperationFilter" />
    public class AuthHeadersOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the specified operation.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="schemaRegistry">The schema registry.</param>
        /// <param name="apiDescription">The API description.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId.EndsWith("Login"))
                return;

            operation.parameters = operation.parameters ?? new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "Set-Cookie",
                description = "Authorization token:  {user_name}/{session_id}/{session_key}",
                @in = "header",
                type = "string",
                required = true
            });
        }
    }
}
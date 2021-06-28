// Licence: Apache-2.0
// Author: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Lightwind.Core.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null)
                throw new System.ArgumentNullException(nameof(operation));
            if (context == null)
                throw new System.ArgumentNullException(nameof(context));

            var requiredScopes = context.MethodInfo
                  .GetCustomAttributes(true)
                  .OfType<AuthorizeAttribute>()
                  //https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1507
                  .Union(context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>())
                  .Select(attr => attr.Policy)
                  .Distinct();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = requiredScopes.ToList()
                    }
                };
            }
        }
    }
}

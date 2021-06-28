// Licence: Apache-2.0
// Author: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Lightwind.Core.Cors
{
    public static class CorsServiceCollectionCorsExtensions
    {
        public static IServiceCollection AddAllowAnyCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(_defaultPolicyName, policy =>
                {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            return services;
        }
        private const string _defaultPolicyName = "AllowAny";

        public static IApplicationBuilder UseAllowAnyCors(this IApplicationBuilder app)
        {
            app.UseCors(_defaultPolicyName);
            return app;
        }
    }
}

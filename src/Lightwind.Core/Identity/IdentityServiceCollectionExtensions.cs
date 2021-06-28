// Licence: Apache-2.0
// Author: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Lightwind.Core.Identity
{
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, string identityUrl, string audience)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });
            return services;
        }

        public static void AddIdentityServerAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJwtBearerAuthentication(configuration["IdentityServer:Url"], configuration["IdentityServer:Audience"]);
            services.AddIdentityHelper();
        }

        public static IServiceCollection AddIdentityHelper(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<IIdentityHelper, IdentityHelper>();
            return services;
        }
    }
}

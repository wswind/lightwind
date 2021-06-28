// Licence: Apache-2.0
// Author: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using Lightwind.Core.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lightwind.Core.Swagger
{
    /// <summary>
    /// 用于加载swagger
    /// </summary>
    public static class SwaggerServiceCollectionExtensions
    {
        /// <summary>
        /// add swagger
        /// </summary>
        /// <param name="services"></param>
        /// <param name="title">swagger标题</param>
        /// <param name="identityUrl">identityserver地址</param>
        /// <param name="scopes">scopes</param>
        /// <param name="assemblyNames">项目分拆后，如要显示其他程序集中定义的注释，
        /// 则需通过assemblyNames传入多个项目名。</param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {

            var identityServerUrl = configuration["IdentityServer:Url"];
            var useIdentity = false;
            if (!string.IsNullOrWhiteSpace(identityServerUrl))
                useIdentity = true;

            var isEnabled = IsSwaggerEnabled(configuration);
            if (!isEnabled)
                return services;
            var swaggerConf = GetSwaggerConfig(configuration);
            var title = swaggerConf["Title"];
            var version = swaggerConf["Version"];
            var description = swaggerConf["Description"];
            var assemblyNames = swaggerConf.GetSection("AssemblyNames").Get<List<string>>();
            var scope = swaggerConf["Scope"];

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = title,
                    Version = version,
                    Description = description
                });

                if (useIdentity)
                {
                    var authorizationUrl = $"{identityServerUrl}/connect/authorize";
                    var tokenUrl = $"{identityServerUrl}/connect/token";


                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Password = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri(authorizationUrl),
                                TokenUrl = new Uri(tokenUrl)
                            }
                        }
                    });
                    options.OperationFilter<SecurityRequirementsOperationFilter>();
                }
                if (assemblyNames?.Count > 0)
                    foreach (var assemblyName in assemblyNames)
                    {
                        if (string.IsNullOrEmpty(assemblyName))
                            continue;
                        var xmlFile = Path.Combine(AppContext.BaseDirectory, assemblyName + ".xml");
                        options.IncludeXmlComments(xmlFile);
                    }
            });
            return services;
        }

        private static IConfigurationSection GetSwaggerConfig(IConfiguration configuration)
        {
            if (configuration == null)
                return null;
            return configuration.GetSection("Swagger");
        }

        private static bool IsSwaggerEnabled(IConfiguration configuration)
        {
            var swaggerConf = GetSwaggerConfig(configuration);
            var title = swaggerConf["Title"];
            if (title == null)
                return false;
            return true;
        }

        /// <summary>
        /// use swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="swaggerName">swagger名称</param>
        /// <param name="clientId">identity client id</param>
        /// <param name="clientName">identity client name</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var isEnabled = IsSwaggerEnabled(configuration);
            if (!isEnabled)
                return app;

            var swaggerConf = GetSwaggerConfig(configuration);
            var clientId = swaggerConf["Identity:ClientId"];
            var clientName = swaggerConf["Identity:ClientName"];
            var clientSecret = swaggerConf["Identity:ClientSecret"];
            var name = swaggerConf["Name"];
            var version = swaggerConf["Version"];

            app.UseSwagger(c =>
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" } };
                }))
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{version}/swagger.json", name);
                    c.OAuthClientId(clientId);
                    c.OAuthClientSecret(clientSecret);
                    c.OAuthAppName(clientName);
                });
            return app;
        }



    }
}

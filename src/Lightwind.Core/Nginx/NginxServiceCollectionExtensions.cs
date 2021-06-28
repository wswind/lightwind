// Licence: Apache-2.0
// Author: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Lightwind.Core.Nginx
{
    public static class NginxServiceCollectionExtensions
    {
        public static IServiceCollection AddNginx(this IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            return services;
        }
        public static IApplicationBuilder UseNginx(this IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UseForwardedPrefix();
            return app;
        }
        public static IApplicationBuilder UseForwardedPrefix(this IApplicationBuilder app)
        {
            app.Use(async (ctx, next) =>
            {
                string prefix = ctx.Request.Headers["X-Forwarded-Prefix"];
                if (!string.IsNullOrWhiteSpace(prefix))
                {
                    var host = ctx.Request.Host.Value;
                    ctx.Request.Host = new HostString($"{host}/{prefix}");
                }
                await next();
            });
            return app;
        }
    }
}

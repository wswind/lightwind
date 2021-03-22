using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Lightwind.DynamicProxyExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace castlecoresample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "castlecoresample", Version = "v1" });
            });

            //without proxy:
            //services.AddTransient<IHelloRobot,HelloRobot>();

            //with proxy:
            // services.AddSingleton<ProxyGenerator>();
            // services.AddTransient<IHelloRobot>(sp =>
            // {
            //     var pg = sp.GetRequiredService<ProxyGenerator>();
            //     var actual = new HelloRobot();
            //     var serv = pg.CreateInterfaceProxyWithTarget<IHelloRobot>(new HelloRobot(), new MyInterceptor());
            //     return serv;
            // });

            //use extension:
            services.AddDynamicProxyService<IHelloRobot>(sp => {
                var actual = new HelloRobot();
                return actual;
            }, ServiceLifetime.Transient , new MyInterceptor());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "castlecoresample v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

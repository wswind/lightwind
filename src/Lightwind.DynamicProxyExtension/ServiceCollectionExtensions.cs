// Licence: MIT
// Author: Vincent Wang
// Email: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Lightwind.DynamicProxyExtension
{
    public static class ServiceCollectionDynamicProxyExtensions
    {
        public static IServiceCollection AddDynamicProxyService<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime serviceLifetime,
            params IInterceptor[] interceptors) where TService : class
        {
            services.TryAddSingleton<ProxyGenerator>();

            Func<IServiceProvider, TService> proxyFactory = sp =>
            {
                var pg = sp.GetRequiredService<ProxyGenerator>();
                var implementation = implementationFactory(sp);
                var serv = pg.CreateInterfaceProxyWithTargetInterface<TService>(implementation, interceptors);
                return serv;
            };

            ServiceDescriptor serviceDescriptor = new ServiceDescriptor(typeof(TService), proxyFactory, serviceLifetime);
            services.TryAdd(serviceDescriptor);
            return services;
        }
    }
}

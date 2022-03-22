/*
 https://autofac.readthedocs.io/en/latest/advanced/interceptors.html
 this is a demo for autofac interceptors with lightwind asyncinterceptorbase
 */


using Autofac;
using Autofac.Extras.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace AutofacAsyncInterceptor
{
    class Program
    {
        async static Task Main(string[] args)
        {
            // create builder
            var builder = new ContainerBuilder();

            builder.RegisterType<SomeType>()
              .As<ISomeType>()
              .EnableInterfaceInterceptors()
              .InterceptedBy(typeof(CallLoggerAsyncInterceptor));
             
            //register async interceptor
            builder.Register(c => new CallLoggerAsyncInterceptor(Console.Out));

            var container = builder.Build();
            var willBeIntercepted = container.Resolve<ISomeType>();
            //sync
            willBeIntercepted.ShowSynchronous("1.test ShowSynchronous");
            //async without return value
            await willBeIntercepted.ShowAsync("2.test ShowAsync");
            //async with return value
            var result = await willBeIntercepted.ShowAsyncWithReturnValue("3.test ShowAsyncWithReturnValue");
            Console.WriteLine($"ShowAsync Return Value Is {result}");
            //value task without return value
            await willBeIntercepted.ShowValueTask("4.test ShowValueTask");
            //value task with return value
            var result2 = await willBeIntercepted.ShowValueTaskWithReturnValue("5.test ShowValueTaskWithReturnValue");
            Console.WriteLine($"ShowValueTaskWithReturnValue Return Value Is {result2}");
        }
    }
}

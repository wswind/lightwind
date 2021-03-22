using System;
using Castle.DynamicProxy;

namespace castlecoresample
{
    public class MyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Interceptor before");
            invocation.Proceed();
            Console.WriteLine("Interceptor after");
        }
    }

}
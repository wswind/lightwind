﻿// Licence: Apache-2.0
// Author: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Lightwind.AsyncInterceptor
{
    //inspired by : https://stackoverflow.com/a/39784559/7726468
    public abstract class AsyncInterceptorBase : IInterceptor
    {
        public AsyncInterceptorBase()
        { 
        }
        
        public void Intercept(IInvocation invocation)
        {
            BeforeProceed(invocation);
            invocation.Proceed();
            if (IsAsyncMethod(invocation.MethodInvocationTarget))
            {
                invocation.ReturnValue = InterceptAsync((dynamic)invocation.ReturnValue, invocation);
            }
            else
            {
                AfterProceedSync(invocation);
            }
        }

        //didn't support ValueTask yet
        protected virtual bool IsAsyncMethod(MethodInfo method)
        {
            var attr = method.GetCustomAttributes<AsyncStateMachineAttribute>(false);
            bool isAsync = (attr != null) && typeof(Task).IsAssignableFrom(method.ReturnType);
            return isAsync;
        }

        private async Task InterceptAsync(Task task, IInvocation invocation)
        {
            await task.ConfigureAwait(false);
            await AfterProceedAsync(invocation, false);
        }

        protected object ProceedAsyncResult { get; set; }

        private async Task<TResult> InterceptAsync<TResult>(Task<TResult> task, IInvocation invocation)
        {
            TResult result = await task.ConfigureAwait(false);
            ProceedAsyncResult = result;
            await AfterProceedAsync(invocation,true);
            return (TResult)ProceedAsyncResult;
        }

        protected virtual void BeforeProceed(IInvocation invocation) {}

        protected virtual void AfterProceedSync(IInvocation invocation) {}

        protected virtual Task AfterProceedAsync(IInvocation invocation,bool hasAsynResult)
        {
            return Task.CompletedTask;
        }
    }
}

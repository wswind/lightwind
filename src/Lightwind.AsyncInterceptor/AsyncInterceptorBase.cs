// Licence: MIT
// Author: Vincent Wang
// Email: ws_dev@163.com
// ProjectUrl: https://github.com/wswind/lightwind

using System;
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

        private bool CheckMethodReturnTypeIsTaskType(MethodInfo method)
        {
            var methodReturnType = method.ReturnType;
            if(methodReturnType.IsGenericType)
            {
                if (methodReturnType.GetGenericTypeDefinition() == typeof(Task<>) ||
                    methodReturnType.GetGenericTypeDefinition() == typeof(ValueTask<>))
                    return true;
            }
            else
            {
                if (methodReturnType == typeof(Task) ||
                    methodReturnType == typeof(ValueTask))
                    return true;
            }
            return false;
        }

        private bool IsAsyncMethod(MethodInfo method)
        {
            bool isDefAsync = Attribute.IsDefined(method, typeof(AsyncStateMachineAttribute), false);
            bool isTaskType = CheckMethodReturnTypeIsTaskType(method);
            bool isAsync = isDefAsync && isTaskType;

            return isAsync;
        }

        protected object ProceedAsyncResult { get; set; }


        private async Task InterceptAsync(Task task, IInvocation invocation)
        {
            await task.ConfigureAwait(false);
            await AfterProceedAsync(invocation, false);
        }

        private async Task<TResult> InterceptAsync<TResult>(Task<TResult> task, IInvocation invocation)
        {
            ProceedAsyncResult = await task.ConfigureAwait(false);
            await AfterProceedAsync(invocation, true);
            return (TResult)ProceedAsyncResult;
        }

        private async ValueTask InterceptAsync(ValueTask task, IInvocation invocation)
        {
            await task.ConfigureAwait(false);
            await AfterProceedAsync(invocation, false);
        }

        private async ValueTask<TResult> InterceptAsync<TResult>(ValueTask<TResult> task, IInvocation invocation)
        {
            ProceedAsyncResult = await task.ConfigureAwait(false);
            await AfterProceedAsync(invocation, true);
            return (TResult)ProceedAsyncResult;
        }

        protected virtual void BeforeProceed(IInvocation invocation) { }

        protected virtual void AfterProceedSync(IInvocation invocation) { }

        protected virtual Task AfterProceedAsync(IInvocation invocation, bool hasAsynResult)
        {
            return Task.CompletedTask;
        }
    }
}

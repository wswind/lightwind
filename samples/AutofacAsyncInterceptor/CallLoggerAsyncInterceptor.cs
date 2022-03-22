using Castle.DynamicProxy;
using Lightwind.AsyncInterceptor;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;


namespace AutofacAsyncInterceptor
{
    public class CallLoggerAsyncInterceptor : AsyncInterceptorBase
    {
        TextWriter _output;

        public CallLoggerAsyncInterceptor(TextWriter output)
        {
            _output = output;
        }

        protected override void BeforeProceed(IInvocation invocation)
        {
            _output.WriteLine("----");
            _output.WriteLine("Intercept Before");
        }

        protected override Task AfterProceedAsync(IInvocation invocation, bool hasAsynResult)
        {
            _output.WriteLine("InterceptAsync After");
            if(hasAsynResult)
            {
                ProceedAsyncResult = "a changed value";
            }
            return Task.CompletedTask;
        }


        protected override void AfterProceedSync(IInvocation invocation)
        {
            _output.WriteLine("InterceptSynchronous After");
        }
    }
}

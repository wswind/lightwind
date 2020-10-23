using System;
using System.Threading.Tasks;

namespace AutofacAsyncInterceptor
{
    public class SomeType : ISomeType
    {
        public async Task<string> ShowAsyncWithReturnValue(string input)
        {
            Console.WriteLine("Run ShowAsyncWithReturnValue Before Await");
            await Task.Delay(1000);
            Console.WriteLine("Run ShowAsyncWithReturnValue After Await");
            return "some type shows";
        }

        public async Task ShowAsync(string input)
        {
            Console.WriteLine("Run ShowAsync Before Await");
            await Task.Delay(1000);
            Console.WriteLine("Run ShowAsync After Await");
        }

        public void ShowSynchronous(string input)
        {
            Console.WriteLine("Run ShowSynchronous");
        }

    
    }
}

using System;
using System.Threading.Tasks;

namespace AutofacAsyncInterceptor
{
    public class SomeType : ISomeType
    {
        public async Task<string> ShowAsyncWithReturnValue(string input)
        {
            Console.WriteLine("Run ShowAsyncWithReturnValue Before Await");
            Console.WriteLine(input);
            await Task.Delay(1000);
            Console.WriteLine("Run ShowAsyncWithReturnValue After Await");
            return "some type shows";
        }

        public async Task ShowAsync(string input)
        {
            Console.WriteLine("Run ShowAsync Before Await");
            Console.WriteLine(input);
            await Task.Delay(1000);
            Console.WriteLine("Run ShowAsync After Await");
        }

        public void ShowSynchronous(string input)
        {
            Console.WriteLine("Run ShowSynchronous");
            Console.WriteLine(input);
        }

        public async ValueTask ShowValueTask(string input)
        {
            Console.WriteLine("Run ShowValueTask Before Await");
            Console.WriteLine(input);
            await Task.Delay(1000);
            Console.WriteLine("Run ShowValueTask After Await");
        }

        public async ValueTask<string> ShowValueTaskWithReturnValue(string input)
        {
            Console.WriteLine("Run ShowValueTaskWithReturnValue Before Await");
            Console.WriteLine(input);
            await Task.Delay(1000);
            Console.WriteLine("Run ShowValueTaskWithReturnValue After Await");
            return "ShowValueTaskWithReturnValue";
        }
    }
}

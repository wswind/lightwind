using System;
using System.Threading.Tasks;

namespace castlecoresample
{
    public class HelloRobot : IHelloRobot
    {
        public void Hello()
        {
            Console.WriteLine("Hello");
        }

        public async Task<int> Hello2()
        {
            Console.WriteLine("Hello2");
            return await Task.FromResult(0);
        }

        public async Task Hello3()
        {
            Console.WriteLine("Hello3");
            await Task.CompletedTask;
        }
    }

}
using System.Threading.Tasks;

namespace castlecoresample
{
    public interface IHelloRobot
    {
        void Hello();
        Task<int> Hello2();
        Task Hello3();
    }

}
using NetworkShareLib;
using System;
using System.Threading;

namespace sendit
{
    class Program
    {
        static void Main(string[] args)
        {
            var Broadcaster  = new Broadcaster(54000);//54001
            Broadcaster.SayHello(54000);

        }
    }
}

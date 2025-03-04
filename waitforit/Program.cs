using NetworkShareLib;
using System;
using System.Threading;

namespace waitfotit
{
    class Program
    {
        static void Main(string[] args)
        {
            //wait for connection
            //add connection to list
            //show list
            var broadcaster = new Broadcaster();
            broadcaster.MessageRecieved += Message_Recieved;
            broadcaster.Listen();
            while (true)
            {
                Thread.Sleep(1000);
            }


        }
        static void Message_Recieved(object sender, BroadcastPayload payload)
        {
            Console.WriteLine($"{payload.Message}-{payload.Client}");

        }
    }
}

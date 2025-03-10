using NetworkShareLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace recieveFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var recieveFile = new RecieveFile(54000);
            recieveFile.Listen();

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}

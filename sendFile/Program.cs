﻿using NetworkShareLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sendFile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var hostname = args[0];
            var file = args[1];

            var transferFile = new TransferFile(file, hostname); 
            transferFile.Start();
           Console.WriteLine("press any key");
            Console.ReadLine();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShareLib
{
    public class BroadcastPayload :EventArgs
    {
        public BroadcastMessage Message { get;}

        public IPEndPoint Client { get; }
        public string Hostname { get; set; }
        public string Filename { get; set; }

        public BroadcastPayload(BroadcastMessage message , IPEndPoint client,string hostname,string filename)
        {
            Message = message;
            Client = client;
            Hostname = hostname;
            Filename = filename;




           
        }
    }
}

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
        
        public BroadcastPayload(BroadcastMessage message , IPEndPoint client)
        {
            Message = message;
            Client = client;
           
        }
    }
}

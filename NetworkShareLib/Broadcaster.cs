using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShareLib
{
    public class Broadcaster
    {
        public const string HEL = nameof(HEL);
        public const string INI = nameof(INI);
        public const string ACK = nameof(ACK);
        public const string SND = nameof(SND);
        public const string SOK = nameof(SOK);
        private readonly UdpClient _client;
        private readonly int _port;

        public EventHandler<BroadcastPayload> MessageRecieved;//eventHandler 
        public Broadcaster(int port = 54000)
        {
            _port = port;
            _client = new UdpClient(_port);

            
        }
       
        public void SayHello()
        {
           // using (var sender = new UdpClient())
            //{
                 var helloString = Encoding.ASCII.GetBytes(HEL);
               _client.Send(helloString,
                       helloString.Length,
                       new IPEndPoint(IPAddress.Broadcast, _port));


            //}
        }
        // summary:
        // it sends a hello message to all clients on the network 
        public void Listen()
        {
            //client Listen on port 54000
            _client.BeginReceive(Client_MessageRecieved, _client);
        }

        public void Acknowledge(IPEndPoint client)
        {
            _client.Send(Encoding.ASCII.GetBytes(ACK),ACK.Length,client);

        }

      
        public void InitiatingTransfer(IPEndPoint client)
        {
            _client.Send(Encoding.ASCII.GetBytes(INI),INI.Length,client);

        }
        //summary:
        //takes an Endpoint to which you send a message

        public void SendFileRequest(IPEndPoint client,string hostAndUser,string filename)
        {
            string trimmedFilename = Path.GetFileName(filename);

            string msg = $"{SND}\r\n{hostAndUser}\r\n{trimmedFilename}";
            _client.Send(Encoding.ASCII.GetBytes(msg), msg.Length, client);

        }
        public void SendFileAcknowledge(IPEndPoint client,string filename)
        {
            string msg = $"{SOK}\r\n{filename}";
            _client.Send(Encoding.ASCII.GetBytes(msg), msg.Length, client);
        }

        private void Client_MessageRecieved(IAsyncResult result)
        {

            if (result.IsCompleted)
            {
                
                var sender = new IPEndPoint(IPAddress.Any, 0);
                var client = result.AsyncState as UdpClient;
                var recieved = client.EndReceive(result, ref sender);//sender: takes the Endpoint of the client who sent this packet
                Console.WriteLine(sender.Address);
              

                _client.BeginReceive(Client_MessageRecieved, _client);//ensuring that the client will listen before the message is sent 
                if (recieved.Length > 0)
                {
                    var msg = Encoding.ASCII.GetString(recieved);
                    var msgSplit = msg.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    switch (msgSplit[0])
                    {
                        case INI:
                            OnMessageRecieved(BroadcastMessage.Initiate,sender);//invokes the MessageRecieve Event inside of it

                            break;                        
                        case ACK:
                            OnMessageRecieved(BroadcastMessage.HelloAcknowledge, sender);//invokes the MessageRecieve Event inside of it
                            break;
                        case SND:
                            OnMessageRecieved(BroadcastMessage.SendRequest, sender, msgSplit[1], msgSplit[2]);//invokes the MessageRecieve Event inside of 
                            break;
                        case SOK:
                            OnMessageRecieved(BroadcastMessage.SendRequest, sender,"", msgSplit[1]);//invokes the MessageRecieve Event inside of 
                            break;
                        default:
                            OnMessageRecieved(BroadcastMessage.Hello, sender);//invokes the MessageRecieve Event inside of 
                            break;
                    }
                }

            }
        }
        private void OnMessageRecieved(BroadcastMessage message,
                                        IPEndPoint client,
                                        string hostname="",
                                        string filename="")
        {
            //if the MessageRecieved has any subscriber they will get invoked and the payload will be passed to them
            //tells the subscribers the kind of message the client recieved and the name of the sender
            MessageRecieved?.Invoke(this,
                                    new BroadcastPayload(message, client,hostname,filename));
        }
    }
}

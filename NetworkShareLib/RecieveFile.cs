using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkShareLib
{
    public class RecieveFile
    {
        private bool _processedHeader;
        private MemoryStream _ms = new MemoryStream();  
        private byte[] _buffer = new byte[65536];
        private long _length;
        private string _filename;
        private  TcpListener _listener;
        private readonly int _port;

        public RecieveFile(int port) 
        {
            _port = port;
            
        }

        public void Listen()
        {
            //endpoint IP and port number to listen on
            var endpoint = new IPEndPoint(IPAddress.Any, _port);
            _listener = new TcpListener(endpoint);
            //start Listening for incoming connection requests
            //start queue the incoming connection requests
            _listener.Start();
            //Accepting the incoming requests
            _listener.BeginAcceptTcpClient(Client_Connected, _listener);

        }

        private void Client_Connected(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                var listener = result.AsyncState as TcpListener;
                var client = listener.EndAcceptTcpClient(result);
                //intiate MemoryStream
                _ms = new MemoryStream();
                //read data from incoming network stream and store it into buffer
                client.GetStream().BeginRead(_buffer,0,_buffer.Length,Client_Recieved,client);

            }
        }

        private void Client_Recieved(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                //if the read done succefully then end reading
                var client = result.AsyncState as TcpClient;
                //save the read bytes count into bytesrecieced variable
                var bytesRecieved = client.GetStream().EndRead(result);

                //check if the header processed
                if (!_processedHeader)
                {
                    var headerSize = GetHeaderSize(_buffer);
                    if (headerSize > 0)
                    {
                        //decoding buffer to string 
                        var raw = Encoding.ASCII.GetString(_buffer, 0, headerSize);
                        //split on \r\n {filename -> newline -> fileLength->newline}
                        var split = raw.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        _filename = split[0];
                        //Data (without Header)
                        _length = long.Parse(split[1]);



                    }
                    
                    _processedHeader = true;
                    var lengthOfData = bytesRecieved - headerSize;
                    //write data into the MemoryStream from the buffer
                    _ms.Write(_buffer, headerSize + 1, lengthOfData);
                }

                //if memorystream data < length of recieved data
                else if(_ms.Length < _length)
                {
                    _ms.Write(_buffer, 0, bytesRecieved);//?  lengthOfData

                }

                if (_ms.Length < _length)
                {
                    Array.Clear(_buffer,0,_buffer.Length);
                    client.GetStream().BeginRead(_buffer, 0, _buffer.Length, Client_Recieved, client);

                }
                else
                {
                    client.Close();
                    File.WriteAllBytes(_filename,_ms.ToArray());
                    _ms=null;
                }

            }
        }

        private int GetHeaderSize(byte[] buffer)
        {
            //Incoming request form
            //header
                //filename
                //filesize
                // \r\n\r\n
            //end header
            //DATA

            var pos = -1;
            //search for "\r\n\r\n" :indication of Header End
            for (int i = 0; i < buffer.Length-4; i++)
            {
                char c1 = (char)buffer[i];
                char c2 = (char)buffer[i+1];
                char c3 = (char)buffer[i+2];
                char c4 = (char)buffer[i+3];

                if(c1 == '\r'&&c2=='n'&&c3 == '\r' && c4== 'n') 
                {
                    // boundry of = header
                    pos = i+3; 
                    break;   
                }
            }

          //return header Boundry
            return pos;
        }

    }
}

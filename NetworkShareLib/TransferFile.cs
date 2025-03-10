using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkShareLib
{
    //transfer a file between your local machine and a client
     public class TransferFile
    {
        public static readonly string  VersionInfo = "NetworkShareing 1.0" ;

        private TcpClient _client;
        private readonly string _hostname;
        private readonly string _filename;
        private readonly int _port;

        public EventHandler TransferComplete;
        

        public TransferFile(string filename,string hostname,int port = 54000)
        {
            _hostname = hostname;
            _filename = filename;
            _port = port;   
        }

        public void Start()
        {
            //create the client to whome you send the file
            _client = new TcpClient();  
            //connect to the client on the given hostname and port number
            _client.Connect(_hostname, _port);

            //create buffer to pass it to the NetworkStream
            var buffer= CreateBuffer();
            
            //takes a buffer and writes it to the NetworkStream 
            _client.GetStream().BeginWrite(buffer, 0, buffer.Length, Write_Result, _client);
        }

        //summary:
        //gets the result of the asyncronys operation BEGINWRITE()
        private void Write_Result(IAsyncResult result)
        {
            if(result.IsCompleted)
            {
                var client = result.AsyncState as TcpClient;
                client.GetStream().EndWrite(result);
                client.Close();
                TransferComplete?.Invoke(this,EventArgs.Empty); 
            }

        }

        //summary:
        //create a buffer 
        //fill the buffer with a MemoryStream {file name ,file size, actual data}
        private byte[] CreateBuffer()
        {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                FileInfo fi = new FileInfo(_filename);
                WriteString(ms, $"{VersionInfo} \r\n");
                WriteString(ms, Path.GetFileName(_filename) + "\r\n");
                WriteString(ms, fi.Length.ToString()+"\r\n\r\n");

                //File.ReadAllBytes and store in RAM :: not the best practice --> read the file into chuncks
                var fileContents = File.ReadAllBytes(_filename);//i think btakhod el path alatol
                ms.Write(fileContents, 0, fileContents.Length);
                buffer = ms.ToArray();

            }

                return buffer;
        }
        private void WriteString(MemoryStream ms,string s)
        {
            var bytes = Encoding.ASCII.GetBytes(s);
            ms.Write(bytes,0, bytes.Length); 
        }
    }
}

﻿using NetworkShareLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkShareGUI
{
    public partial class MainForm : Form
    {

        private Broadcaster _broadcaster ;
        public MainForm()
        {
            InitializeComponent();
            InitBroadcaster();
        }

        private void InitBroadcaster()
        {
            _broadcaster = new Broadcaster();
            _broadcaster.SayHello();
            _broadcaster.Listen();
            _broadcaster.MessageRecieved += Broadcaster_MessageRecieved;//publish subscribe
                                                                        //it will get the payload as soon as a message: containing the message recieved and the endPoint of the sender
                                                                        // += :the method Broadcaster_MessageRecieved is subscriebed to the event MessageRecieved 
        }

        private void Broadcaster_MessageRecieved(object sender, BroadcastPayload e)
        {
            var broadcaster = sender as Broadcaster;
            switch(e.Message)
            {
                case BroadcastMessage.Hello:
                    //in case of getting Hello MSG 
                    //send Acknowledge to the sender of the hello msg
                    broadcaster.Acknowledge(e.Client);
                    break;
                case BroadcastMessage.Acknowledge:
                    //add client to list
                    lstNodes.Items.Add(e.Client);
                    break;

                case BroadcastMessage.Initiate:
                    var reciever = new RecieveFile(54000);
                    reciever.TransferComplete += FileRecieved_Complete;
                    reciever.Listen();  
                    break;
                    
            }
        }

        private void mmuSendFile_Click(object sender, EventArgs e)
        {
            if(lstNodes.SelectedItem==null) 
            {
                MessageBox.Show("Please select an item");
            }
            else
            {
                var ofd = new OpenFileDialog();
                if(ofd.ShowDialog() == DialogResult.OK) 
                {
                    var client = lstNodes.SelectedItem as IPEndPoint;
                    var hostname = client.Address.ToString();
                    
                    _broadcaster.InitiatingTransfer(client);

                    var transfer = new TransferFile(ofd.FileName, hostname);
                    transfer.TransferComplete += Transfer_Complete;
                    transfer.Start();


                }
            }

        }

        private void FileRecieved_Complete(object sender, EventArgs e)
        {
            var reciveFile = sender as RecieveFile;
            reciveFile.Stop();
            MessageBox.Show("Transfer Complete!");

        }
        private void Transfer_Complete(object sender, EventArgs e)
        {
            MessageBox.Show("Transfer Complete!");
        }
    }

}

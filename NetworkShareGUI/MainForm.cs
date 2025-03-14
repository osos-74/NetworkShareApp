﻿using NetworkShareLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkShareGUI
{
    public partial class MainForm : Form
    {
        public EventHandler AcceptTransfer;

        private string _fileToTransfer = "";
        private Broadcaster _broadcaster;

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
            _broadcaster.MessageRecieved += Broadcaster_MessageReceived;
        }

        private void AddToClientList(IPEndPoint client)
        {
            if (!lstNodes.Items.Contains(client))
            {
                lstNodes.Items.Add(client);
            }
        }

        private void Broadcaster_MessageReceived(object sender, BroadcastPayload e)
        {
            var broadcaster = sender as Broadcaster;

            switch (e.Message)
            {
                case BroadcastMessage.Hello:
                    // Send Acknowldge message
                    broadcaster.Acknowledge(e.Client);
                    CheckAndAdd(e.Client);
                    break;
                case BroadcastMessage.HelloAcknowledge:
                    // Add client to lis t
                    CheckAndAdd(e.Client);
                    break;
                case BroadcastMessage.Initiate:
                    var receiver = new RecieveFile(54000);
                    receiver.TransferComplete += FileReceived_Complete;
                    receiver.Listen();
                    break;
                case BroadcastMessage.SendRequest:
                    if(MessageBox.Show($"{e.Filename} from {e.Hostname}","Recieve File?",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    {
                        broadcaster.SendFileAcknowledge(e.Client, e.Filename);
                    }
                    break;
                case BroadcastMessage.SendAcknowledge:
                    _broadcaster.InitiatingTransfer(e.Client);
                    var transfer = new TransferFile(_fileToTransfer,e.Client.Address.ToString());
                    transfer.TransferComplete += Transfer_Complete;
                    transfer.Start();
                    break;

                    
            }
        }

        private void CheckAndAdd(IPEndPoint client)
        {
            var infs = NetworkInterface.GetAllNetworkInterfaces();
            bool found = false;
            foreach (var i in infs)
            {
                var addrs = i.GetIPProperties();
                foreach (var ip in addrs.UnicastAddresses)
                {
                    if (ip.Address.Equals(client.Address))
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                    break;
            }

            if (!found)
            {
                Invoke((Action)(() => AddToClientList(client)));
            }
        }

   private void mmuSendFile_Click(object sender, EventArgs e)
        {
            if (lstNodes.SelectedItem == null)
            {
                MessageBox.Show("Please select an item");
            }
            else
            {


                var ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var client = lstNodes.SelectedItem as IPEndPoint;
                    //var hostname = client.Address.ToString();
                    //_broadcaster.InitiatingTransfer(client);
                    _fileToTransfer = ofd.FileName;
                    var hostName = $"{Environment.UserName}@{Environment.MachineName}";
                    _broadcaster.SendFileRequest(client, hostName, _fileToTransfer);

                    //var transfer = new TransferFile(ofd.FileName, hostname);
                    //transfer.TransferComplete += Tranfer_Complete;
                    //transfer.Start();
                }
            }

            }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle the selection change here
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Your logic for handling the context menu opening
        }
        private void FileReceived_Complete(object sender, EventArgs e)
        {
            var receiveFile = sender as RecieveFile;
            receiveFile.Stop();
            MessageBox.Show("Transfer complete!");
        }

        private void Transfer_Complete(object sender, EventArgs e)
        {
            MessageBox.Show("Transfer complete!");
        }
    }
}
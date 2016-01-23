using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace WinTrail
{

    // http://www.codeproject.com/Articles/17031/A-Network-Sniffer-in-C
    public partial class Form1 : Form
    {
        private byte[] byteData = new byte[4096];
        string interfaceIPtoListen;
        private Socket mainSocket;

        public Form1()
        {
            InitializeComponent();
        
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void imzalarıGüncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // maltrail kaynaklarından toplayıp > imzalar.txt'ye yazacak :)
        }

        private void Form1_Load(object sender, EventArgs e)
        {  

            interfaceIPtoListen = Interaction.InputBox("Interface to listen", "Interface to listen", "X.X.X.X", 0, 0);

            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
            mainSocket.Bind(new IPEndPoint(IPAddress.Parse(interfaceIPtoListen), 0));
            mainSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
            byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
            byte[] byOut = new byte[4] { 1, 0, 0, 0 }; 
            mainSocket.IOControl(IOControlCode.ReceiveAll, byTrue, byOut);
            mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(ar);
                              
                ParseData(byteData, nReceived);
                    
                byteData = new byte[4096];

                mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                        new AsyncCallback(OnReceive), null);

            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ParseData(byte[] byteData, int nReceived)
        {

            IPHeader ipHeader = new IPHeader(byteData, nReceived);

            var lines = File.ReadAllLines(@".\imzalar.txt");

            for (int index = 0; index < lines.Length; index++)
            {
                if ( lines[index].Contains(ipHeader.DestinationAddress.ToString()))
                {
                    if (!listBox1.Items.Contains(ipHeader.DestinationAddress.ToString()))
                    {
                        listBox1.Items.Add(ipHeader.DestinationAddress.ToString());

                    }
                }
         
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Furkan Çalışkan, 2016", "Hakkında");
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }


    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameStructure
{
    class Connection
    {
        private TcpClient tcpclnt;//to talk to the server
        private NetworkStream outgoingStream; //Stream - outgoing
        private BinaryWriter writer; //To write to the clients

        private NetworkStream incommingStream; //Stream - incoming        
        private TcpListener listener; //To listen to the clinets        
        public string reply = ""; //The message to be written
        public static String ServerIP = "";

        public Connection()
        {
            try
            {
                tcpclnt = new TcpClient();
                tcpclnt.Connect(ServerIP, 6000);
                this.outgoingStream = tcpclnt.GetStream();
                this.writer = new BinaryWriter(outgoingStream);
                Byte[] tempStr = Encoding.ASCII.GetBytes("JOIN#");
                this.writer.Write(tempStr);
                this.writer.Close();
                this.outgoingStream.Close();
                Console.WriteLine("Connected");
                tcpclnt.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //Thread listenThread = new Thread(listentoServer);
            //listenThread.Start();
        }

        public void sendDatatoServer(String str)
        {
            try
            {
                tcpclnt = new TcpClient();
                //tcpclnt.Connect("127.0.0.1", 6000);
                tcpclnt.Connect(ServerIP, 6000);
                this.outgoingStream = tcpclnt.GetStream();
                this.writer = new BinaryWriter(outgoingStream);
                Byte[] tempStr = Encoding.ASCII.GetBytes(str);
                this.writer.Write(tempStr);
                this.writer.Close();
                this.outgoingStream.Close();
                tcpclnt.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public void listentoServer()
        {
            try
            {

                this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
                this.listener.Start();

                while (true)
                {
                    TcpClient cln = listener.AcceptTcpClient();
                    incommingStream = cln.GetStream();
                    byte[] bytesToRead = new byte[cln.ReceiveBufferSize];
                    int bytesRead = incommingStream.Read(bytesToRead, 0, cln.ReceiveBufferSize);
                    Console.WriteLine(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    String reply = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}

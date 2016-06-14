using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class TCPClient
    {
        Socket s;
        int SIZE=1024;

        public TCPClient(string server, int port)
        {
            s = ConnectSocket(server,port);
            if(s==null)
            {
                throw new SocketException();
            }
        }
        private  Socket  ConnectSocket(string server, int port)
        {
           
            Socket s = null;
            IPHostEntry hostEntry = null;

            hostEntry = Dns.Resolve(server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket =
                    new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }
        public void Send(string message)
        {
            Byte[] bytesSent = Encoding.UTF8.GetBytes(message);
            s.Send(bytesSent, bytesSent.Length, 0);
        }
        public string Receive()
        {
            string dataReciever = String.Empty;
            while (true)
            {
                byte[] byteRecieve = new byte[SIZE];

                int lengthRecieve = s.Receive(byteRecieve);
                dataReciever += Encoding.UTF8.GetString(byteRecieve, 0, lengthRecieve);

                if (dataReciever.IndexOf("<TheEnd>") > -1)
                {
                    dataReciever = dataReciever.Substring(0, dataReciever.IndexOf("<TheEnd>"));
                    break;
                }
            }
            return dataReciever;
        }
    }
}


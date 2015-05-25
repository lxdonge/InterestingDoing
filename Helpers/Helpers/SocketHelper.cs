using CommonLib.Helpers;
using CommonLib.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CommonLib.Helpers
{
    public  class SocketHelper
    {
        public SendOrRcvInfo SendInfo
        {
            set;
            get;
        }

        public IPAddress Ipaddress
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public IPAddress TargetIpaddress
        {
            get;
            set;
        }

        public int TargetPort
        {
            get;
            set;
        }

        public string validateKey
        {
            set;
            get;
        }

        public Socket udpSender
        {
            set;
            get;
        }

        public SocketHelper()
        {
        }



        public SocketHelper(SendOrRcvInfo msg)
        {
            this.SendInfo = msg;
        }

        private void UDPSender() {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(SendInfo.IpAddress), int.Parse(SendInfo.Port));
                UdpClient udp = new UdpClient();
                byte[] data = Encoding.Unicode.GetBytes(JsonHelper.ObjectToJSON<MsgModule>(SendInfo.Msg));
                udp.Send(data, data.Length, endpoint);
                udp.Close();
            }
            catch (Exception e) { 
            
            }
        }


        private void UdpSender()
        {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(SendInfo.IpAddress), int.Parse(SendInfo.Port));
                byte[] data = Encoding.Unicode.GetBytes(JsonHelper.ObjectToJSON<MsgModule>(SendInfo.Msg));
                udpSender.SendTo(data, endpoint);
            }
            catch (Exception e) { 
            
            }
        }


        //udp发送
        public static void UdpSenderStartNonAnonymous(Socket udpSend, SendOrRcvInfo sendInfo)
        {
            SocketHelper udpSender = new SocketHelper(sendInfo);
            udpSender.udpSender = udpSend;
            Thread send = new Thread(udpSender.UdpSender);
            send.Start();
        }


        //udp匿名发送
        public static void StartUdpSendThread(SendOrRcvInfo sendInfo)
        {
            SocketHelper udpSender = new SocketHelper(sendInfo);
            Thread send = new Thread(udpSender.UDPSender);
            send.Start();
        }
     

        //tcp连接 ，接收数据
        public   string  DownloadDataByTCP() {
            string data="";
            IPEndPoint ipendp = new IPEndPoint(Ipaddress,Port) ;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(ipendp);
            IPEndPoint remt = new IPEndPoint(TargetIpaddress, TargetPort);
            socket.Connect(remt);
            byte[] bytes = new byte[validateKey.Length+1];
            bytes = Encoding.Unicode.GetBytes(validateKey);
            socket.Send(bytes);

            bytes = new byte[1024];
            int l = socket.Receive(bytes);
            while (l>0) {
                string tmp = Encoding.Unicode.GetString(bytes, 0, l);
                data=data.Insert(data.Length, tmp);
                bytes = new byte[1024];
                l = socket.Receive(bytes);
            }
            socket.Close();
            return data;
        }

    }
}

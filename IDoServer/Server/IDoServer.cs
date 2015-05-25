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
using CommonLib.GloableVariable;
namespace IDoServer
{
    public class IDoUDPServer
    {
        public static Socket srvSocket;

        public IDoUDPServer()
        {
            srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipendp = new IPEndPoint(IPAddress.Parse(CommonLib.GloableVariable.Globle.serverAddress), CommonLib.GloableVariable.Globle.serverUdpPort);
            srvSocket.Bind(ipendp);
        }
        public void UdpListen() {
            IPEndPoint rcvendp = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remote=(EndPoint)rcvendp;
            int recv;
            while (true) {
                byte[] data = new byte[1024];
                try
                {
                    recv = srvSocket.ReceiveFrom(data, ref  remote);
                    SendOrRcvInfo recvClient = new SendOrRcvInfo();
                    recvClient.IpAddress = ((IPEndPoint)remote).Address.ToString();
                    recvClient.Port = ((IPEndPoint)remote).Port.ToString();
                    string gets = Encoding.Unicode.GetString(data,0,recv);
     
                    recvClient.Msg = JsonHelper.JSONToObject<MsgModule>(gets);
                    MsgTranslate mtrs = new MsgTranslate(recvClient);
                    Thread t = new Thread(mtrs.Translate);
                    t.Start();
                }
                catch (Exception e) {
                    continue;
                }
            }
        } 
       
    }

    public class IDoTcpServer {

        public void TcpListen()
        {
            IPEndPoint ipendp = new IPEndPoint(IPAddress.Parse(CommonLib.GloableVariable.Globle.serverAddress), CommonLib.GloableVariable.Globle.serverTcpPort);
            Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            srvSocket.Bind(ipendp);
            srvSocket.Listen(5);
            while (true) {
                try
                {
                    Socket client = srvSocket.Accept();
                    //validate first. use dynamic key.
                    byte[] bytes = new byte[1024];
                    int l=client.Receive(bytes);
                    string data = Encoding.Unicode.GetString(bytes,0,l);
                    if(Validation.validateKey(data))
                    {
                        SendUserListThread sender = new SendUserListThread(client);
                        Thread th = new Thread(sender.Send);
                        th.Start();
                    }
                }
                catch (Exception e) { 
                
                }
            }
        }
    }

    public class SendUserListThread {
        public SendUserListThread(Socket s)
        {
            this.sender = s;
        }

        public Socket sender;
        public void Send(){
            byte[] userlistData = Encoding.Unicode.GetBytes(DBBiz.User.GetUserListToString());
            sender.Send(userlistData, userlistData.Length,SocketFlags.None);
            this.sender.Close();
        }

        ~SendUserListThread() {
            
        }
        
    }
}

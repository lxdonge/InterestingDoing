using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using CommonLib.GloableVariable;
using CommonLib.Module;
using CommonLib.Helpers;
using IDoClient.Helpers;
using System.Threading;
namespace IDoClient.ClientServer
{
    public class ClientCommunicationSrvr
    {

        public static Socket socket; 

        public ClientCommunicationSrvr() {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            bind:
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, CommonLib.GloableVariable.Globle.clientUdpPort);
           try
            {
                socket.Bind(ipep);
           }
           catch (System.Net.Sockets.SocketException e)
           {
               Console.WriteLine("绑定套接字异常" + e.ToString());
               CommonLib.GloableVariable.Globle.clientUdpPort += 1;
               if (CommonLib.GloableVariable.Globle.clientUdpPort >= 9520)
                   throw e;
               goto bind;

          }
        }

        public   void ClientCommunicationUdpListen() {
            IPEndPoint rcvendp = new IPEndPoint(IPAddress.Any, 0);
            EndPoint remote = (EndPoint)rcvendp;
            while (true) {
                try
                {
                    byte[] data = new byte[16000];
                    int recv = socket.ReceiveFrom(data, ref remote);
                    SendOrRcvInfo rcvinfo = new SendOrRcvInfo();
                    rcvinfo.IpAddress = ((IPEndPoint)remote).Address.ToString();
                    rcvinfo.Port = ((IPEndPoint)remote).Port.ToString();
                    string get = Encoding.Unicode.GetString(data, 0, recv);
                    rcvinfo.Msg = JsonHelper.JSONToObject<MsgModule>(get);
                    MsgTranslate mtrs = new MsgTranslate(rcvinfo);
                    Thread t = new Thread(mtrs.Translate);
                    t.Start();
                }
                catch (Exception e) { 
                    
                }
            }

            
        }
    }
}

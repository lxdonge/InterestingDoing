using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IDoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入本地服务器IP");
            string ip = Console.ReadLine();
            CommonLib.GloableVariable.Globle.serverAddress = ip;
            DBBiz.User.InitUserDB();

            string s = DBBiz.User.GetUserListToString();
            IDoUDPServer serverUDP = new IDoUDPServer();
            Thread udpThread = new Thread(serverUDP.UdpListen);
            udpThread.Start();

            IDoTcpServer serverTCP = new  IDoTcpServer();
            Thread tcpThread = new Thread(serverTCP.TcpListen);
            tcpThread.Start();


            Console.WriteLine("UDP服务已经启动");
            Console.Read();
        }
    }
}

using IDoClient.ClientServer;
using IDoClient.MainLogic;
using IDoClient.UIInform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IDoClient
{
    class Program
    {
        public static  void Menu() {
            Console.WriteLine("1.login");
            Console.WriteLine("2.show user list");
            Console.WriteLine("3.chat to who ");
            Console.WriteLine("4.logout");
        }

        static void Main(string[] args)
        {
            //binding UI handler
            ChatClientUIs.LoginSucceed.OnUpdateUI += TestUI.OnLoginSucceed;
            ChatClientUIs.LoginFailed.OnUpdateUI += TestUI.OnLoginFailed;
            ChatClientUIs.ChatWindowOpen.OnUpdateUI += TestUI.OnChatWindowOpen;
            ChatClientUIs.ChatMsgCome.OnUpdateUI += TestUI.OnMsgCome;


            ClientCommunicationSrvr clientSrv = new ClientCommunicationSrvr();
            Thread CommunicationUdpListen = new Thread(clientSrv.ClientCommunicationUdpListen);
            CommunicationUdpListen.Start();

         //   User me = new User();
         //   me.DoLoginByName("test01", "test01");

            loop:

            Menu();
            int a = Console.Read();
            switch (a) {
                case '1': { 
                    Console.WriteLine("input name and password");
                    Console.ReadLine();
                    string name = Console.ReadLine();
                    string pass = Console.ReadLine();
                    User me = new User();
                    me.DoLoginByName(name, pass);
                    break;
                }
                case '2':
                    {
                        TestUI.OnLoginSucceed("");
                        break;
                    }
                case '3':
                    {
                        Console.WriteLine("who do you want to chat with,input the name ");
                        Console.ReadLine();
                        string name = Console.ReadLine();
                        Chat.TryChatTo(name);
                        break;
                    }
                default:{
                    Console.WriteLine("wrong input");
                    break;
                }
            }
            goto loop;

        }
    }
}

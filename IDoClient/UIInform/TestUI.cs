using CommonLib.Helpers;
using CommonLib.Module;
using Helpers.Moduls;
using IDoClient.ClientServer;
using IDoClient.MainLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDoClient.UIInform
{
    public class TestUI
    {
        public static void OnLoginSucceed(string arg){
           // Console.WriteLine("arg is :"+arg);
            Console.WriteLine("User list:" );
            foreach (UserInfo u in User.FriendsList) {
                Console.WriteLine("UserName: "+u.userName+","+"UserID :"+u.userID);
            }
        }

        public static void OnLoginFailed(string arg) {
            Console.WriteLine("Login in Fialed");
        }

        public static void OnChatWindowOpen(string arg) {
            Console.WriteLine("you can chat with you frends now :");
            string msg = Console.ReadLine();
            MessageInfo c = new MessageInfo(User.MyInfo.userName,"cc",msg,DateTime.Now.ToString());
           

            MsgModule ms = new MsgModule();
            ms.Code = InstructionSet.Instructions.CHAT;
            ms.msg1 = JsonHelper.ObjectToJSON<MessageInfo>(c);

            SendOrRcvInfo send = new SendOrRcvInfo(ms);
            send.IpAddress = arg.Split('#')[0];
            send.Port = arg.Split('#')[1];
            SocketHelper.UdpSenderStartNonAnonymous(ClientCommunicationSrvr.socket,send);

        }

        public static void OnMsgCome(string arg)
        {
            Console.WriteLine("you friend say:");
            MessageInfo msg = JsonHelper.JSONToObject<MessageInfo>(arg);
            Console.WriteLine(msg.time);
            Console.WriteLine(msg.myname + ":" + msg.msg);
        }

    }
}

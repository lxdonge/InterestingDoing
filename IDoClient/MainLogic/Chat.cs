using CommonLib.Helpers;
using CommonLib.Module;
using Helpers.Moduls;
using IDoClient.ClientServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDoClient.MainLogic
{
    public class Chat
    {
        public static Dictionary<string ,ChatInfo> chatList = new Dictionary<string ,ChatInfo>();

        //send msg to name
        public static  void TryChatTo(string name) {

            MsgModule ChatRequestMsg = new MsgModule();
            ChatRequestMsg.Code = InstructionSet.Instructions.CHATFORNATINFO;
            ChatRequestMsg.key = "IDoingClient";
            ChatRequestMsg.Extra = name;
            SendOrRcvInfo ChatRequestInfo = new SendOrRcvInfo(ChatRequestMsg);
            ChatRequestInfo.Port = CommonLib.GloableVariable.Globle.serverUdpPort.ToString();
            ChatRequestInfo.IpAddress = CommonLib.GloableVariable.Globle.serverAddress;
            SocketHelper.UdpSenderStartNonAnonymous(ClientServer.ClientCommunicationSrvr.socket, ChatRequestInfo);

        }


        public static void  SendChatMsgTo(MessageInfo _msg,UserInfo chatToNETInfo)
        {
            try
            {
                MsgModule ms = new MsgModule();
                ms.Code = InstructionSet.Instructions.CHAT;
                ms.msg1 = JsonHelper.ObjectToJSON<MessageInfo>(_msg);
                SendOrRcvInfo send = new SendOrRcvInfo(ms);

                send.IpAddress = chatToNETInfo.ip;
                send.Port = chatToNETInfo.port;
                SocketHelper.UdpSenderStartNonAnonymous(ClientCommunicationSrvr.socket, send);
            }
            catch (Exception e)
            { 

            }
        }


    }
}

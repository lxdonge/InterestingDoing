using CommonLib.Helpers;
using CommonLib.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using IDoClient.MainLogic;
using IDoClient.UIInform;
using System.Timers;
using System.Threading;
namespace IDoClient.Helpers
{
    public  class MsgTranslate
    {
        public SendOrRcvInfo serverInfo;

        public void DownloadUserList() {
            SocketHelper sockethelper = new SocketHelper();
            sockethelper.Ipaddress = IPAddress.Any;
            sockethelper.Port = CommonLib.GloableVariable.Globle.clientTcpPort;
            sockethelper.TargetIpaddress = IPAddress.Parse(serverInfo.Msg.msg1);
            sockethelper.TargetPort = int.Parse(serverInfo.Msg.msg2);
            sockethelper.validateKey = serverInfo.Msg.Extra;
            IDoClient.MainLogic.User.FriendsList = JsonHelper.JSONToObject<List<UserInfo>>(sockethelper.DownloadDataByTCP());
        }

        public MsgTranslate(SendOrRcvInfo s)
        {
            this.serverInfo=s;
        }

        private void timerFunction()
        {
        loop:
            long count = 0;
            try
            {
                while (true)
                {
                    count++;
                    MsgModule msg = new MsgModule(InstructionSet.Instructions.HEARTBEAT, User.MyInfo.userName);
                    if (count % 10 == 0)
                        msg.msg2 = "UPDATEMYINFO";
                    SendOrRcvInfo sendinfo = new SendOrRcvInfo(msg, CommonLib.GloableVariable.Globle.serverAddress, CommonLib.GloableVariable.Globle.serverUdpPort.ToString());
                    SocketHelper.UdpSenderStartNonAnonymous(ClientServer.ClientCommunicationSrvr.socket, sendinfo);
                    Thread.Sleep(30000);
                }
            }
            catch (Exception e) {
                goto loop;
            }
        }

        public void Translate() {
            if (Validation.validateKey(serverInfo.Msg.key))
            {
                switch (serverInfo.Msg.Code)
                {
                    case InstructionSet.Instructions.RESPONSE:
                        { 
                            
                        }
                        break;
                    case InstructionSet.Instructions.REGIST_SUCCESS:
                        {
                            ChatClientUIs.RegisterSucceed.DoUpdateUI(serverInfo.Msg.Extra);
                        break;
                        }
                    case InstructionSet.Instructions.REGIST_FAIL:
                        {
                            ChatClientUIs.RegisterFailed.DoUpdateUI(serverInfo.Msg.Extra);
                        break;
                        }

                    case InstructionSet.Instructions.ACCEPT:
                        {
                            DownloadUserList();
                            //infrom interface login compelte use try catch
                            ChatClientUIs.LoginSucceed.DoUpdateUI("");
                            //开始心跳更新
                            Thread th = new Thread(timerFunction);
                            th.Start();
                        }
                        break;
                    case InstructionSet.Instructions.REJECT:
                        {
                            ChatClientUIs.LoginFailed.DoUpdateUI("");
                        }
                        break;
                    case InstructionSet.Instructions.REFRESHUSERLIST:
                        {
                            DownloadUserList();
                            //infrom interface refresh compelte use try catch
                            ChatClientUIs.UerListUpdate.DoUpdateUI("");
                        }
                        break;
                    case InstructionSet.Instructions.SOMEONEONLIEN:
                        {
                            String who=serverInfo.Msg.msg1;
                            ChatClientUIs.SomeoneOnline.DoUpdateUI(who);
                            break;
                        }
                    case InstructionSet.Instructions.SOMEONEOFFLINE:
                        {
                            String who = serverInfo.Msg.msg1;
                            ChatClientUIs.SomeoneOffline.DoUpdateUI(who);
                            break;
                        }
                    case InstructionSet.Instructions.CHAT_NAT_SERVERTELLYOURTARGETINFO:
                        {
                            string args = serverInfo.Msg.msg1 + "#" + serverInfo.Msg.msg2+"#"+serverInfo.Msg.Extra;
                            ChatClientUIs.ReceiveChattoNetInfo.DoUpdateUI(args);
                        }
                        break;
                    case InstructionSet.Instructions.CHAT_NAT_ASK:
                        { 
                            //possible revceid ,start chat
                            MsgModule msg = new MsgModule();
                            msg.key = CommonLib.GloableVariable.Globle.appServerKey;
                            msg.Code = InstructionSet.Instructions.CHAT_NAT_SUCCESS;
                            msg.Extra = "both ask ,may be received";
                            UserInfo user = new UserInfo();

                            SendOrRcvInfo sendInfo1 = new SendOrRcvInfo(msg);
                            sendInfo1.IpAddress = serverInfo.IpAddress;
                            sendInfo1.Port = serverInfo.Port;
                            SocketHelper.UdpSenderStartNonAnonymous(ClientServer.ClientCommunicationSrvr.socket, sendInfo1);
                            //if chatlist has the name ,then begin chat 
                            if (Chat.chatList[serverInfo.Msg.msg1].isConnected == true)
                                break;
                            if (Chat.chatList.ContainsKey(serverInfo.Msg.msg1))           
                            { 
                                //inform ui begin chat
                                string ipandport = serverInfo.IpAddress + "#" + serverInfo.Port;
                                ChatClientUIs.ChatWindowOpen.DoUpdateUI(ipandport);
                                Chat.chatList[serverInfo.Msg.msg1].isConnected = true;
                            }

                        }
                        break;
                    case InstructionSet.Instructions.CHAT_NAT_SUCCESS:
                        {
                            //success  inform  ui and popup chat window =
                            if (Chat.chatList[serverInfo.Msg.msg1].isConnected == true)
                                break;
                            if (Chat.chatList.ContainsKey(serverInfo.Msg.msg1))
                            {
                                string ipandport = serverInfo.IpAddress + "#" + serverInfo.Port;
                                ChatClientUIs.ChatWindowOpen.DoUpdateUI(ipandport);
                            }
                        }
                        break;
                    case InstructionSet.Instructions.CHAT:
                        { 
                            //dealwith msg inform ui
                            ChatClientUIs.ChatMsgCome.DoUpdateUI(serverInfo.Msg.msg1);
                        }
                        break;
                    default:
                        break;

                }
                
            }
        
        
        }
    }
}

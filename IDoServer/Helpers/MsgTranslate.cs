using IDoServer.Authentication;
using CommonLib.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonLib.Helpers;
namespace IDoServer
{
    public class MsgTranslate
    {
        public SendOrRcvInfo clientInfo;

        public MsgTranslate(SendOrRcvInfo clientInfo)
        {
            this.clientInfo = clientInfo;
        }

        public void Translate()
        {
            MsgModule msg = new MsgModule();
            msg.key = CommonLib.GloableVariable.Globle.appServerKey;           

            if (Validation.validateKey(clientInfo.Msg.key))
            {
                switch (clientInfo.Msg.Code)
                {
                    case InstructionSet.Instructions.REGISTER:
                        {
                             string retID=DBBiz.User.UserRegisterWithName(clientInfo.Msg.msg1,clientInfo.Msg.msg2);
                             if (retID != "-1")
                                 msg.Code = InstructionSet.Instructions.REGIST_SUCCESS;
                             else
                                 msg.Code = InstructionSet.Instructions.REGIST_FAIL;
                            msg.Extra = retID;
                        }
                        break;
                    case InstructionSet.Instructions.LOGIN:
                        {
                            int index=Authentication.Authorize.userLoginByName(clientInfo.Msg.msg1, clientInfo.Msg.msg2);
                            if (index!=-1)
                            {

                                msg.msg1 = CommonLib.GloableVariable.Globle.serverAddress;
                                msg.msg2 = CommonLib.GloableVariable.Globle.serverTcpPort.ToString();
                                msg.Code = InstructionSet.Instructions.ACCEPT;
                                msg.Extra = Validation.generateKey("allowConnect");
                                DBBiz.User.SetUserOnlineInfo(index,clientInfo.IpAddress,clientInfo.Port);
                                //broadcast login
                                MsgModule bmsg = new MsgModule(InstructionSet.Instructions.SOMEONEONLIEN, clientInfo.Msg.msg1);
                                foreach (UserInfo u in DBBiz.User.UserDB) {
                                    SendOrRcvInfo s = new SendOrRcvInfo(bmsg, u.ip, u.port);
                                    SocketHelper.StartUdpSendThread(s);
                                }

                            }
                            else
                            {
                                msg.Code = InstructionSet.Instructions.REJECT;
                            }
                        }
                        break;

                    case InstructionSet.Instructions.LOGOUT:
                        {
                            //use name to logout 
                            DBBiz.User.SetUserOffline(clientInfo.Msg.msg1);
                            msg.Code = InstructionSet.Instructions.RESPONSE;
                            msg.Extra = "logout succeed";

                            //broadcast logout
                            MsgModule bmsg = new MsgModule(InstructionSet.Instructions.SOMEONEOFFLINE, clientInfo.Msg.msg1);
                            foreach (UserInfo u in DBBiz.User.UserDB)
                            {
                                SendOrRcvInfo s = new SendOrRcvInfo(bmsg, u.ip, u.port);
                                SocketHelper.StartUdpSendThread(s);
                            }

                        }
                        break;

                    case InstructionSet.Instructions.REFRESHUSERLIST:
                        {
                            msg.msg1 = CommonLib.GloableVariable.Globle.serverAddress;
                            msg.msg2 = CommonLib.GloableVariable.Globle.serverTcpPort.ToString();
                            msg.Code = InstructionSet.Instructions.REFRESHUSERLIST;
                            msg.Extra = Validation.generateKey("allowRefresh");
                        }
                        break;

                    case InstructionSet.Instructions.CHATFORNATINFO:
                        {
                            UserInfo user=new UserInfo();
                            user = DBBiz.User.GetUserInfoByName(clientInfo.Msg.Extra);
                            msg.Code = InstructionSet.Instructions.CHAT_NAT_SERVERTELLYOURTARGETINFO;
                            msg.msg1 = user.ip;
                            msg.msg2 = user.port;
                            msg.Extra = user.userName;
                        }
                        break;
                    case InstructionSet.Instructions.HEARTBEAT: {
                        if (clientInfo.Msg.msg2 != "")
                        {   
                            int index=Authentication.Authorize.userLoginByName(clientInfo.Msg.msg1, clientInfo.Msg.msg2);
                            if (index != -1)
                            {
                                DBBiz.User.SetUserOnlineInfo(index, clientInfo.IpAddress, clientInfo.Port);
                            }
                        
                        }
                            break;
                        }

                    default: break;
                }

                SendOrRcvInfo sendInfo = new SendOrRcvInfo(msg);
                sendInfo.IpAddress =clientInfo.IpAddress;
          //    sendInfo.Port = CommonLib.GloableVariable.Globle.clientUdpPort.ToString();
                //client use the same port send msg
                sendInfo.Port = clientInfo.Port;


                SocketHelper.StartUdpSendThread(sendInfo);
            }

        }

    }
}

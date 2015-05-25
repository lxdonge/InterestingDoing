using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Module;
using CommonLib.Helpers;
using System.Threading;
using CommonLib.GloableVariable;
using IDoClient.ClientServer;
namespace IDoClient.MainLogic
{
    public class User
    {
        public static  UserInfo MyInfo{
            set;
            get;
        }

        public User() {
            MyInfo = new UserInfo();
            FriendsList = new List<UserInfo>();
        }

        public static   List<UserInfo> FriendsList
        {
            set;
            get;
        }
        public static void SetUserOnline(string username)
        {
            try
            {
                int index = FriendsList.FindIndex(
                  delegate(UserInfo u)
                  {
                      if (u.userName == username)
                          return true;
                      else return false;
                  });
                if (index != -1)
                    FriendsList[index].IsOnline = "true";
                else
                {

                    FriendsList.Add(new UserInfo(username));
                }
            }
            catch (Exception e) { }
        }

        public static void SetUserOffline(string username)
        {
            try
            {
                int index = FriendsList.FindIndex(
                     delegate(UserInfo u)
                     {
                         if (u.userName == username)
                             return true;
                         else return false;
                     });
                if(index!=-1)
                FriendsList[index].IsOnline = "false";
            }
            catch (Exception e) { }
        }
        public   void DoLoginById(string id, string psw)
        {
            MyInfo.userID = id;
            
            MsgModule loginMsg = new MsgModule();
            loginMsg.Code = InstructionSet.Instructions.LOGIN;
            loginMsg.key = "IDoingClient";
            loginMsg.msg1 = id;
            loginMsg.msg2 = psw;
            SendOrRcvInfo loginInfo = new SendOrRcvInfo(loginMsg);
            loginInfo.Port = CommonLib.GloableVariable.Globle.serverUdpPort.ToString();
            loginInfo.IpAddress = CommonLib.GloableVariable.Globle.serverAddress;
           // SocketHelper.StartUdpSendThread(loginInfo);
            SocketHelper.UdpSenderStartNonAnonymous(ClientCommunicationSrvr.socket, loginInfo);
        }

        public  void DoLoginByName(string name, string psw)
        {
            MyInfo.userName = name;

            MsgModule loginMsg = new MsgModule();
            loginMsg.Code = InstructionSet.Instructions.LOGIN;
            loginMsg.key = CommonLib.GloableVariable.Globle.appClientKey;
            loginMsg.msg1 = name;
            loginMsg.msg2 = psw;
            SendOrRcvInfo loginInfo = new SendOrRcvInfo(loginMsg);
            loginInfo.Port = CommonLib.GloableVariable.Globle.serverUdpPort.ToString();
            loginInfo.IpAddress = CommonLib.GloableVariable.Globle.serverAddress;
   
            SocketHelper.UdpSenderStartNonAnonymous(ClientCommunicationSrvr.socket, loginInfo);
        }

        public static void DoLogoff(string name)
        {
            MsgModule logoffMsg = new MsgModule();
            logoffMsg.Code = InstructionSet.Instructions.LOGOUT;
            logoffMsg.key = CommonLib.GloableVariable.Globle.appClientKey;
            logoffMsg.msg1 = name;
            SendOrRcvInfo logoffInfo = new SendOrRcvInfo(logoffMsg,CommonLib.GloableVariable.Globle.serverAddress,CommonLib.GloableVariable.Globle.serverUdpPort.ToString());
            SocketHelper.UdpSenderStartNonAnonymous(ClientCommunicationSrvr.socket, logoffInfo);

        }

        public void Register(string name,string psw) {
            MsgModule registerMsg = new MsgModule(InstructionSet.Instructions.REGISTER,name,psw);
            SendOrRcvInfo registerInfo = new SendOrRcvInfo(registerMsg,CommonLib.GloableVariable.Globle.serverAddress,CommonLib.GloableVariable.Globle.serverUdpPort.ToString());
            SocketHelper.UdpSenderStartNonAnonymous(ClientCommunicationSrvr.socket, registerInfo);
        }

        public static  UserInfo GetUserInfoByName(string name)
        {
            try
            {
                int index = FriendsList.FindIndex(delegate(UserInfo u)
                {
                    if (u.userName == name)
                        return true;
                    else
                        return false;
                });
                return FriendsList[index];
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

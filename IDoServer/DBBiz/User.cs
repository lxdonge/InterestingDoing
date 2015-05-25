using CommonLib.Helpers;
using CommonLib.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDoServer.DBBiz
{
    public class User
    {
        public  static List<UserInfo> UserDB = new List<UserInfo>();

        private static  long NowUserCount = 10000;

        public static void InitUserDB() {
            UserDB.Add(new UserInfo("lixiao", "aaaaaa"));
            UserDB.Add(new UserInfo("xiaoli", "aaaaaa"));
        }


        public static void SetUserOnlineInfo(int index,string ip,string port) {
            UserDB[index].IsOnline ="true";
            UserDB[index].ip = ip;
            UserDB[index].port = port;
        }

        public static void SetUserOnline(string username) {
            int index = UserDB.FindIndex(
              delegate(UserInfo u)
              {
                  if (u.userName == username)
                      return true;
                  else return false;
              });
            if (index != -1)
            UserDB[index].IsOnline = "true";
        }

        public static void SetUserOffline(string username) {
            int index = UserDB.FindIndex(
                 delegate(UserInfo u)
                 {
                     if (u.userName == username)
                         return true;
                     else return false;
                 });
            if (index != -1)
            UserDB[index].IsOnline = "false";
        }

        public static int UserValidateLoginById(string userId, string userPsw) {
            userPsw = Encryption.DoEncryption(userPsw);

            int index=UserDB.FindIndex(
                delegate(UserInfo u) 
                { 
                    if (u.userID == userId && u.userPassword == userPsw)
                        return true;
                    else return false;
                }) ;
            return index;
        }

        public static int UserValidateLoginByName(string userName, string userPsw)
        {
            userPsw = Encryption.DoEncryption(userPsw);

            int index=UserDB.FindIndex(
                delegate(UserInfo u)
                {
                    if (u.userName == userName && u.userPassword == userPsw)
                        return true;
                    else return false;
                }) ;

            return index;
        }


        public static string UserRegisterWithName(string name, string psw) {
            if (UserDB.FindIndex(delegate(UserInfo u) {
                if (u.userName == name)
                    return true;
                else
                    return false;
                    })!=-1)
            {
                return "-1";
            }
            UserInfo t = new UserInfo(name, psw);
            NowUserCount++;
            t.userID = NowUserCount.ToString() ;
            UserDB.Add(t);
            return t.userID;
        }

        public static string GetUserListToString() {
            return JsonHelper.ObjectToJSON<List<UserInfo>>(User.UserDB);
        }
 

        public static string DoDelUser(string UserName) {
            int index=UserDB.FindIndex(delegate(UserInfo u)
            {
                if (u.userName == UserName)
                    return true;
                else
                    return false;
            });
            if(index>-1)
             UserDB.Remove(UserDB[index]);
            return UserName;
        }

        public static UserInfo GetUserInfoByName(string name) {
            try
            {
                int index = UserDB.FindIndex(delegate(UserInfo u)
                {
                    if (u.userName == name)
                        return true;
                    else
                        return false;
                });
                return UserDB[index];
            }
            catch (Exception e) {
                return null;
            }
        }

    }
}

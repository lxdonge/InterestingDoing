using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDoServer.Authentication
{
    public class Authorize
    {
        public static  int userLoginByID(string userID,string userPsw) {

            return DBBiz.User.UserValidateLoginById(userID, userPsw);
        }
        public static int userLoginByName(string userID, string userPsw)
        {

            return DBBiz.User.UserValidateLoginByName(userID, userPsw);
        }


        public static string userRegister(string userName, string userPsw)
        {
            return DBBiz.User.UserRegisterWithName(userName, userPsw);     
        }
      
    }
}

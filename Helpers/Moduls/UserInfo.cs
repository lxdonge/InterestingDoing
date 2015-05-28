using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Module
{
    public class UserInfo
    {
        public UserInfo(string userName, string userPsw)
        {
            this.userName = userName;
            this.userPassword = userPsw;
            this.IsOnline = "false";
        }
        public UserInfo(string userName)
        {
            this.userName = userName;

        }
      
        public UserInfo() { }

        public string IsOnline { set; get; }
        public string ip { set; get; }
        public string port { set; get; }

        public string userID { set; get; }
        public string userPassword {set;get;}

        public string userName { set; get; }
        
        public string userSign { set; get; }
        public string userAddress { set; get; }
        public string userEmail { set; get; }
        public string userBirth { set; get; }
        public string userMobilePhone { set; get; }

        public string userDynamicKey { set; get; }

    }
}

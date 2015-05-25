using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Module
{
    public class InstructionSet
    {
        public  enum Instructions { 
            //RESPONSE
            REGIST_SUCCESS=0X10,
            REGIST_FAIL=0X11,
            RESPONSE=0X12,
            //SERVER
            LOGIN=0X01,
            LOGOUT=0X02,
            REFRESHUSERLIST=0X03,
            CHATFORNATINFO=0X04,
            CHAT_NAT_SERVERTELLYOURTARGETINFO=0X05,

            //CLIENT
            ACCEPT=0X51,
            REJECT=0X52,
            USERLSIT=0X53,
            CHAT_CHATFORNATINFO_SUCCESS = 0X54,  
            REGISTER = 0X55,
            CHAT_NAT_SUCCESS=0X56,//target let client know success
            CHAT_NAT_ASK=0X57,//server let target do
            CHAT=0X58,
            HEARTBEAT=0X59,
            SOMEONEONLIEN=0X60,
            SOMEONEOFFLINE=0X61
          
        }
        
    }
}

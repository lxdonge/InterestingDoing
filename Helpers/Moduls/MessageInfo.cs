using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Moduls
{
    public class MessageInfo
    {
        public MessageInfo(string n,string tn ,string m, string t)
        {
            myname = n;
            toname = tn;
            msg = m;
            time=t;
        }
        public MessageInfo() { }
        public string myname{set;get;}
        public string toname { set; get; }
        public string msg{set;get;}
        public string time { set; get; }
     
    }
}

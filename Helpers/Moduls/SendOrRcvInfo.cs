using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Module
{
    public class SendOrRcvInfo
    {
        public SendOrRcvInfo() {
            Msg = new MsgModule();
        }
        public SendOrRcvInfo(MsgModule m,string ip,string port)
        {
            this.Msg = m;
            this.IpAddress =ip;
            this.Port = port;
        }
        public SendOrRcvInfo(MsgModule m)
        {
            this.Msg = m;
            this.IpAddress = "";
            this.Port = "";
        }

        public string IpAddress { set; get; }
        public string Port { set; get; }
        public MsgModule Msg { set; get; }
    }
}

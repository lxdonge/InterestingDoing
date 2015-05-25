using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Moduls
{
    public class ChatInfo
    {
        public ChatInfo() {
            ChaMsg = new List<MessageInfo>();
        }

        public bool isConnected { set; get; }
        public List<MessageInfo> ChaMsg { set; get; }
    }
}

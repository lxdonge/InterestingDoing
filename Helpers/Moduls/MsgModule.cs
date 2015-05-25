using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Module
{
    public class MsgModule
    {

        public MsgModule(Module.InstructionSet.Instructions Code , string msg1 = "", string msg2 = "", string ext = "")
        {
            this.Code = Code;
            this.msg1 = msg1;
            this.msg2 = msg2;
            this.Extra = ext;
        }


        public MsgModule() {
            key = "";
            msg1 = "";
            msg2 = "";
            Extra = "";
        }

        public Module.InstructionSet.Instructions Code { set; get; }
        public string key { set; get; }
        public string msg1 { set; get; }
        public string msg2 { set; get; }
        public string Extra { set; get; }
    }
}

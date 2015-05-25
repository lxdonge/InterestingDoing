using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDoClient.UIInform
{
    public class UI
    {
        public delegate void UpdateUI(string arg);

        public event UpdateUI OnUpdateUI;

        public void DoUpdateUI(string arg) {
            if (OnUpdateUI != null) {
                OnUpdateUI(arg);
            }
        }
        
    }
}

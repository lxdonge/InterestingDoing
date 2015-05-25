using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Helpers
{
    public class Win32Functions
    {
        [DllImport("user32.dll")]
        public static extern Int32 SendMessage(IntPtr hwnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public UInt32 dwData;
        public UInt32 cbData;
        public string lpData;
    }

    public class Win32Consts
    {
        public const int WM_COPYDATA = 0x004A;
    }
}

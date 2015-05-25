using CommonLib.Helpers;
using IDoingClientUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IDoClientUI
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
          
        }

        private void RegisterLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            //hook winproc, 须在Windows.Loaded事件响应中调用
            HwndSource hs = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            hs.AddHook(new HwndSourceHook(TestHwndSourceHook));

        }
        private IntPtr TestHwndSourceHook(
                                       IntPtr hwnd,
                                       int msg,
                                       IntPtr wParam,
                                       IntPtr lParam,
                                       ref bool handled
                                       )
        {
            switch (msg)
            {
                case Win32Consts.WM_COPYDATA:
                    COPYDATASTRUCT copydata = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
                    MessageBox.Show(msg.ToString());
                    handled = true;
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        private void RegisterClick(object sender, MouseButtonEventArgs e)
        {
            string username = null;
            string psw = null;
            string rpsw = null;
            try
            {
                username = this.UserName.Text;
                psw = this.Psw.Password;
                rpsw = this.RPsw.Password;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Regex Pattern = new Regex(@"^[0-9a-zA-Z\u4e00-\u9fa5\$]+$");
            if (username != null && psw != null && psw == rpsw && Pattern.IsMatch(username))
            {
                //注册
                MainWindow.Me.Register(username, psw);
            }
        }
        private void Close(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        private void Focus(object sender, MouseEventArgs e)
        {
            ((Control)sender).Focus();
        }
        private void DragMove(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

    }
}

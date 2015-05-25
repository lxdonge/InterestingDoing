using CommonLib.Module;
using Helpers.Moduls;
using IDoClient.MainLogic;
using IDoingClientUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IDoClientUI.Dialogs
{
    /// <summary>
    /// ChatForm.xaml 的交互逻辑
    /// </summary>
    public partial class ChatForm : Window
    {
        public string chatToIP = null;
        public string chatToPort = null;
        public string chatToName = null;

        public ChatForm()
        {
            InitializeComponent();
        }
        private void DragMove(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void Close(object sendee, MouseEventArgs e)
        {
            if (MainWindow.ChatWindows.ContainsKey(this.ChatWithWho.Text))
            {
                MainWindow.ChatWindows.Remove(this.ChatWithWho.Text);
            }
            this.Close();
        }

        private void SendMsgClick(object sender, MouseButtonEventArgs e)
        {
            TextRange textRange = new TextRange(MyMsg.Document.ContentStart, MyMsg.Document.ContentEnd);
            // MessageBox.Show(textRange.Text);

            MessageInfo _msg = new MessageInfo(User.MyInfo.userName, this.ChatWithWho.Text, textRange.Text, DateTime.Now.ToString());
            if (chatToPort != null && chatToIP != null)
            {
                UserInfo uinfo = new UserInfo();
                uinfo.ip = this.chatToIP;
                uinfo.port = this.chatToPort;
                this.AppendMsgToChatBox(_msg);
                Chat.SendChatMsgTo(_msg, uinfo);
                this.count = 0;
                this.Timer.Close();
            }
            else
            {
                Chat.TryChatTo(this.ChatWithWho.Text);
                Timer.Interval = 1000;
                Timer.Elapsed += new ElapsedEventHandler((s, eg) => TrySendMsgTo(s, _msg));
                Timer.Start();
            }
        }

        System.Timers.Timer Timer = new System.Timers.Timer(); //从threadpool启动 private 
        private int count = 0;
        private void TrySendMsgTo(object sender, MessageInfo e)
        {
            count++;
            MessageInfo msg = (MessageInfo)(object)e;
            if (chatToPort != null && chatToIP != null)
            {
                UserInfo uinfo = new UserInfo();
                uinfo.ip = this.chatToIP;
                uinfo.port = this.chatToPort;
                this.AppendMsgToChatBox(msg);
                Chat.SendChatMsgTo(msg, uinfo);
                App.Current.Dispatcher.Invoke((Action)(()=>{
                    this.MyMsg.Document.Blocks.Clear();
                }));
                
                this.count = 0;
                this.Timer.Stop();
            }
            else if (count > 5)
            {
                MessageBox.Show("无法连接到对方", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                this.count = 0;
                this.Timer.Stop();
                return;
            }
        }


        public void AppendMsgToChatBox(MessageInfo _msg)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
                   {
                       TextRange rangeOfText1 = new TextRange(this.ChatMsgs.Document.ContentEnd, this.ChatMsgs.Document.ContentEnd);
                       rangeOfText1.Text = _msg.myname + "  " + _msg.time + "\r\n";
                       rangeOfText1.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2980b9")));
                       rangeOfText1.ApplyPropertyValue(TextElement.FontSizeProperty, "12");
                       TextRange rangeOfText2 = new TextRange(this.ChatMsgs.Document.ContentEnd, this.ChatMsgs.Document.ContentEnd);
                       rangeOfText2.Text = _msg.msg;
                       rangeOfText2.ApplyPropertyValue(TextElement.FontSizeProperty, "14");
                       this.ChatMsgs.ScrollToEnd();
                   }));
        }
    }
}

using CommonLib.Helpers;
using CommonLib.Module;
using Helpers.Moduls;
using IDoClient.MainLogic;
using IDoClientUI.Dialogs;
using IDoingClientUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace IDoClientUI.UIInform
{
    public class UIUpdate
    {
        public static void OnLoginSucceed(string arg)
        {
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                System.Windows.Window mainwin = System.Windows.Application.Current.MainWindow;
                mainwin.IsEnabled = false;
                MainWindow.UserlistFrm  = new UserListForm();
                MainWindow.UserlistFrm.UserList.ItemsSource = User.FriendsList;
                MainWindow.UserlistFrm.MyName.Text = User.MyInfo.userName;
                MainWindow.UserlistFrm.Show();
              
            }));
        }

        public static void OnLoginFailed(string arg)
        {
            MessageBox.Show("用户名或密码错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void OnMsgCome(string arg)
        {
            MessageInfo Msg = JsonHelper.JSONToObject<MessageInfo>(arg);
            try
            {
                if (MainWindow.ChatWindows.ContainsKey(Msg.myname))
                {
                    App.Current.Dispatcher.Invoke((Action)(() =>
                      {
                          ((ChatForm)MainWindow.ChatWindows[Msg.myname]).AppendMsgToChatBox(Msg);
                           MainWindow.ChatWindows[Msg.myname].Show();
                      }));
                }
                else
                {
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        ChatForm cf = new ChatForm();
                        cf.ChatWithWho.Text = Msg.myname;
                        cf.AppendMsgToChatBox(Msg);
                        cf.Show();
                        MainWindow.ChatWindows[Msg.myname] = cf;
                    }));
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void OnReceiveChattoNETInfo(string arg) {
            string[] info = arg.Split('#');
            try
            {
            if (MainWindow.ChatWindows.ContainsKey(info[2]))
                {
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        ((ChatForm)MainWindow.ChatWindows[info[2]]).chatToIP = info[0];
                        ((ChatForm)MainWindow.ChatWindows[info[2]]).chatToPort = info[1];
                        ((ChatForm)MainWindow.ChatWindows[info[2]]).chatToName = info[2];
                      // MainWindow.ChatWindows[info[2]].Show();
                       //  MessageBox.Show("拿到信息");
                    }));
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void OnRegisterFailed(string arg) {
            MessageBox.Show("登记失败，用户已经存在","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);

        }

        public static void OnRegisterSucceed(string arg) {
            MessageBox.Show("登记成功，请登录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                if (MainWindow.RegisterFrm != null)
                    MainWindow.RegisterFrm.Close();
            }));
         
         
        }

        public static void OnSomeOneOnline(string arg)
        {

            User.SetUserOnline(arg);
            Console.WriteLine(arg + "上线了");

            Thread.Sleep(300);
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                MainWindow.UserlistFrm.UserList.ItemsSource = null;
                MainWindow.UserlistFrm.UserList.ItemsSource = User.FriendsList;
            }));
        }

        public static void OnSomeOneOffline(string arg) {
            User.SetUserOffline(arg);
            Console.WriteLine(arg + "下线了");

            Thread.Sleep(300);
            App.Current.Dispatcher.Invoke((Action)(() =>
            {
                MainWindow.UserlistFrm.UserList.ItemsSource = null;
                MainWindow.UserlistFrm.UserList.ItemsSource = User.FriendsList;
            }));
        }
    }
}

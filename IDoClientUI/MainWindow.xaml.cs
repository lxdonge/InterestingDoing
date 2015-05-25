using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommonLib.Module;
using IDoClient.MainLogic;
using IDoClient.UIInform;
using IDoClient.ClientServer;
using System.Threading;
using IDoClientUI.UIInform;
using IDoClientUI.Dialogs;
using Helpers.Moduls;
using IDoClientUI;

namespace IDoingClientUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public static UserInfo myInfo;
        public static User Me = new User();
        public static Dictionary<string, Window> ChatWindows;

        public static Register RegisterFrm=null;
        public static UserListForm UserlistFrm = null;
        public MainWindow()
        {
            this.IsEnabledChanged += LoginWindow_IsEnabledChanged;  


            ChatClientUIs.LoginSucceed.OnUpdateUI += UIUpdate.OnLoginSucceed;
            ChatClientUIs.LoginFailed.OnUpdateUI += UIUpdate.OnLoginFailed;
            ChatClientUIs.ChatWindowOpen.OnUpdateUI += TestUI.OnChatWindowOpen;
            ChatClientUIs.ChatMsgCome.OnUpdateUI += UIUpdate.OnMsgCome;
            ChatClientUIs.RegisterSucceed.OnUpdateUI += UIUpdate.OnRegisterSucceed;
            ChatClientUIs.RegisterFailed.OnUpdateUI += UIUpdate.OnRegisterFailed;
            ChatClientUIs.ReceiveChattoNetInfo.OnUpdateUI += UIUpdate.OnReceiveChattoNETInfo;
            ChatClientUIs.SomeoneOnline.OnUpdateUI += UIUpdate.OnSomeOneOnline;
            ChatClientUIs.SomeoneOffline.OnUpdateUI += UIUpdate.OnSomeOneOffline;

            ClientCommunicationSrvr clientSrv = new ClientCommunicationSrvr();
            Thread CommunicationUdpListen = new Thread(clientSrv.ClientCommunicationUdpListen);
            CommunicationUdpListen.Start();
            ChatWindows = new Dictionary<string, Window>();

            InitializeComponent();
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(this);
        }

        private void LoginWindow_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == false)
            {
               this.Hide();
            }
        }  


        private void UserNameTxbOnFocus(object sender, MouseEventArgs e)
        {
            this.UserNameTxb.Focus();
        }

        private void PswTxbOnFocus(object sender, MouseEventArgs e)
        {
            this.PswTxb.Focus();
        }
        private void RememberMe(object sender, MouseEventArgs e)
        {
            if (this.RememberMeChkBx.IsChecked == true)
            {
                this.RememberMeChkBx.IsChecked = false;
            }
            else
            {
                this.RememberMeChkBx.IsChecked = true;
            }
        }
        private void AutoLogin(object sender, MouseEventArgs e)
        {
            if (this.AutoLoginChkBx.IsChecked == true)
            {
                this.AutoLoginChkBx.IsChecked = false;
            }
            else
            {
                this.AutoLoginChkBx.IsChecked = true;
            }
        }

        private void LoginBtnChangeBg(object sender, MouseEventArgs e)
        {
            Color c = (Color)ColorConverter.ConvertFromString("#16A085");
            this.LoginBtn.Background = new SolidColorBrush(c);

        }
        private void Login(object sender, MouseEventArgs e)
        {
            string username = this.UserNameTxb.Text.ToString();
            string pswd = this.PswTxb.Password.ToString();
            try
            {
                myInfo = new UserInfo(username, pswd);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入格式有误", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Me.DoLoginByName(myInfo.userName, myInfo.userPassword);

            //ChatForm cf = new ChatForm();
            //MessageInfo _msg = new MessageInfo("小张","测试测试",DateTime.Now.ToString());
            //cf.AppendMsgToChatBox( _msg);
            //cf.Show();

            //ChatWindows[_msg.name] = cf;
        }

      

        private void DragMove(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }
        private void Close(object sendee, MouseEventArgs e)
        {
         //   this.Close();
            //Application.Current.Shutdown();
            Environment.Exit(0);
        }

        private void RegisterClick(object sender, MouseButtonEventArgs e)
        {
            RegisterFrm = new Register();
            RegisterFrm.Show();

        }

    }
}

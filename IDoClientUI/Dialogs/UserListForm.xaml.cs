using CommonLib.Module;
using IDoClient.MainLogic;
using IDoingClientUI;
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
using System.Windows.Shapes;

namespace IDoClientUI.Dialogs
{
    /// <summary>
    /// UserListForm.xaml 的交互逻辑
    /// </summary>
    /// 
   
    public partial class UserListForm : Window
    {
        public UserListForm()
        {
            
            InitializeComponent();
        }
      
        private void DragMove(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void Close(object sender, MouseEventArgs e)
        {

            this.Hide();
            
        }

        public void ChatToClick(object sender, MouseEventArgs e)
        {
            string toWho=((UserInfo)this.UserList.SelectedItem).userName;
            if (MainWindow.ChatWindows.ContainsKey(toWho))
            {
                App.Current.Dispatcher.Invoke((Action)(() =>
                {
                    MainWindow.ChatWindows[toWho].Show();
                }));
            }
            else { 
                 App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        ChatForm cf = new ChatForm();
                        cf.ChatWithWho.Text = toWho;
                        cf.Show();
                        MainWindow.ChatWindows[toWho] = cf;
                    }));
            }
           // Chat.SendChatMsgTo(toWho, "fun");
        }
    }
}

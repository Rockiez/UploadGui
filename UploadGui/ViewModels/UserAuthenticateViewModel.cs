using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UploadGui.Commands;

namespace UploadGui.ViewModels
{
    public class UserAuthenticateViewModel : NotificationObject
    {
        private Page _CurrentPage;
        public Page CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }


        public DelegateCommand NextCommand { get; set; }
        private void Next(object sender)
        {
          
            UpLoadWin w2 = new UpLoadWin();
            Application.Current.MainWindow?.Close();
            Application.Current.MainWindow = w2;
            w2.Show();
            
        }

        public DelegateCommand LoginCommand { get; set; }
        private void Login(object sender)
        {
            CurrentPage = new TitleSettingPage(this);
        }


        public UserAuthenticateViewModel()
        {
            CurrentPage = new LoginPage(this);
            LoginCommand = new DelegateCommand
            {
                ExecuteAction = Login
            };
            NextCommand = new DelegateCommand
            {
                ExecuteAction = Next
            };
        }
    }
}

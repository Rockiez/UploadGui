using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using UploadGui.Commands;
using Dapper;
using UploadGui.Models;
using UploadGui.Services;

namespace UploadGui.ViewModels
{
    public class UserAuthenticateViewModel : NotificationObject
    {
        public UserAuthenticateViewModel()
        {
            CurrentPage = new LoginPage(this);
            LoginButtonEnable = true;
            Username = "renhe@wicresoft.com";
            Password = "199658Renhe";
            LoginCommand = new DelegateCommand
            {
                ExecuteAction = Login
            };
            NextCommand = new DelegateCommand
            {
                ExecuteAction = Next
            };
        }

        #region Content of control
        private Page _currentPage;
        public Page CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                NotifyPropertyChanged("CurrentPage");
            }
        }

        private string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                NotifyPropertyChanged("Username");
            }
        }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                NotifyPropertyChanged("Password");
            }
        }
        private bool _loginButtonEnable;

        public bool LoginButtonEnable
        {
            get { return _loginButtonEnable; }
            set
            {
                _loginButtonEnable = value;
                NotifyPropertyChanged("LoginButtonEnable");
            }
        }

        private List<Studio> _studioList;
        public List<Studio> StudioList
        {
            get { return _studioList; }
            set
            {
                _studioList = value;
                NotifyPropertyChanged("StudioList");
            }
        }



        #endregion



        public DelegateCommand NextCommand { get; set; }
        private void Next(object sender)
        {
          
            UpLoadWin w2 = new UpLoadWin();
            Application.Current.MainWindow?.Close();
            Application.Current.MainWindow = w2;
            w2.Show();
            using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=User_info"))
            {
            }
            
        }


        public bool EmailValidation(string email)
        {
            if (email != null && email is string)
            {

                if (email != String.Empty)
                {
                    string emailFormartRegex =
                        @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|" +
                        @"(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

                    return Regex.IsMatch(email, emailFormartRegex);
                }

            }
            return false;
        }


        #region helper methods

        private string _devAccountToken;
        public DelegateCommand LoginCommand { get; set; }

        private async void Login(object sender)
        {
            if (EmailValidation(Username))
            {
                LoginButtonEnable = false;
                await UserAuthenticateApiService.Login(new LoginRequest()
                {
                    DeveloperToolProductName = "PlayFab_EditorExtensions",
                    DeveloperToolProductVersion = "2.53.181001",
                    Email = Username,
                    Password = this.Password
                }, async (result) =>
                {
                    _devAccountToken = result.DeveloperClientToken;
                    await GetStudiosList();
                });
                LoginButtonEnable = true;
            }
            else
            {
                MessageBox.Show("Input string not match Email Format");
            }
            CurrentPage = new TitleSettingPage(this);
        }

        public async Task GetStudiosList()
        {
            if (string.IsNullOrEmpty(_devAccountToken))
                return;

            if (StudioList != null)
                StudioList.Clear();
            await UserAuthenticateApiService.GetStudios(new GetStudiosRequest(), (getStudioResult) =>
            {
                if (StudioList == null)
                    StudioList = new List<Studio>();
                foreach (var eachStudio in getStudioResult.Studios)
                    StudioList.Add(eachStudio);
                StudioList.Add(Studio.OVERRIDE);
            }, _devAccountToken);
        }


        #endregion

    }
}

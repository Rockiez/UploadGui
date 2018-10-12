using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        private IList<string> _studioComboList;
        private string _studioSelected;

        public IList<string> StudioComboList
        {
            get { return _studioComboList; }
            set
            {
                _studioComboList = value;
                NotifyPropertyChanged("StudioComboList");
            }
        }
        public string StudioSelected
        {
            get
            {
                return _studioSelected;
            }
            set
            {
                _studioSelected = value;
                NotifyPropertyChanged("StudioSelected");
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





        #region helper methods
        
        public static bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(PlayFabEditorPrefsSO.Instance.DevAccountToken);
        }

        public DelegateCommand LoginCommand { get; set; }

        private void Login(object sender)
        {
            PlayFabEditorApi.Login(new LoginRequest()
            {
                DeveloperToolProductName = "PlayFab_EditorExtensions",
                DeveloperToolProductVersion = "2.53.181001",
                Email = Username,
                Password = this.Password
            }, (result) =>
            {
                PlayFabEditorPrefsSO.Instance.DevAccountToken = result.DeveloperClientToken;
                PlayFabEditorPrefsSO.Instance.DevAccountEmail = Username;
                PlayFabEditorDataService.RefreshStudiosList();

            });

            RefreshTitleData();
            CurrentPage = new TitleSettingPage(this);
        }


        private  void OnStudioChange(Studio newStudio)
        {
            var newTitleId = (newStudio.Titles == null || newStudio.Titles.Length == 0) ? "" : newStudio.Titles[0].Id;
            OnTitleIdChange(newTitleId);
        }

        private  void OnTitleChange(Title newTitle)
        {
            OnTitleIdChange(newTitle.Id);
        }

        private void OnTitleIdChange(string newTitleId)
        {
            var studio = GetStudioForTitleId(newTitleId);
            PlayFabEditorPrefsSO.Instance.SelectedStudio = studio.Name;
            PlayFabEditorDataService.SharedSettings.TitleId = newTitleId;
#if ENABLE_PLAYFABADMIN_API || ENABLE_PLAYFABSERVER_API || UNITY_EDITOR
            PlayFabEditorDataService.SharedSettings.DeveloperSecretKey = studio.GetTitleSecretKey(newTitleId);
#endif
            PlayFabEditorPrefsSO.Instance.TitleDataCache.Clear();
        }
        private Studio GetStudioForTitleId(string titleId)
        {
            if (PlayFabEditorPrefsSO.Instance.StudioList == null)
                return Studio.OVERRIDE;
            foreach (var eachStudio in PlayFabEditorPrefsSO.Instance.StudioList)
                if (eachStudio.Titles != null)
                    foreach (var eachTitle in eachStudio.Titles)
                        if (eachTitle.Id == titleId)
                            return eachStudio;
            return Studio.OVERRIDE;
        }

        public readonly List<KvpItem> items = new List<KvpItem>();

        public void RefreshTitleData()
        {
            Action<GetTitleDataResult> dataRequest = (result) =>
            {
                items.Clear();
                foreach (var kvp in result.Data)
                    items.Add(new KvpItem(kvp.Key, kvp.Value));

                PlayFabEditorPrefsSO.Instance.TitleDataCache.Clear();
                foreach (var pair in result.Data)
                    PlayFabEditorPrefsSO.Instance.TitleDataCache.Add(pair.Key, pair.Value);
            };

            PlayFabEditorApi.GetTitleData(dataRequest);
        }

        #endregion
        public UserAuthenticateViewModel()
        {
            CurrentPage = new LoginPage(this);
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
    }
}

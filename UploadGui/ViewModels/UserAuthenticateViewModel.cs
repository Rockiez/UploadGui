using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UploadGui.Commands;
using Dapper;
using UploadGui.Models;
using UploadGui.Services;

namespace UploadGui.ViewModels
{
    public class UserAuthenticateViewModel : NotificationObject
    {

        #region Content of control
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

        #region menu and helper methods

        private static string _userEmail = string.Empty;
        private static string _userPass = string.Empty;
        public static bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(PlayFabEditorPrefsSO.Instance.DevAccountToken);
        }
        
        
        private static void OnLoginButtonClicked()
        {
            PlayFabEditorApi.Login(new LoginRequest()
            {
                DeveloperToolProductName = "PlayFab_EditorExtensions",
                DeveloperToolProductVersion = "2.53.181001",
                Email = _userEmail,
                Password = _userPass
            }, (result) =>
            {
                PlayFabEditorPrefsSO.Instance.DevAccountToken = result.DeveloperClientToken;
                PlayFabEditorPrefsSO.Instance.DevAccountEmail = _userEmail;
                PlayFabEditorDataService.RefreshStudiosList();

            });
        }


        private static void OnStudioChange(Studio newStudio)
        {
            var newTitleId = (newStudio.Titles == null || newStudio.Titles.Length == 0) ? "" : newStudio.Titles[0].Id;
            OnTitleIdChange(newTitleId);
        }

        private static void OnTitleChange(Title newTitle)
        {
            OnTitleIdChange(newTitle.Id);
        }

        private static void OnTitleIdChange(string newTitleId)
        {
            var studio = GetStudioForTitleId(newTitleId);
            PlayFabEditorPrefsSO.Instance.SelectedStudio = studio.Name;
            PlayFabEditorDataService.SharedSettings.TitleId = newTitleId;
#if ENABLE_PLAYFABADMIN_API || ENABLE_PLAYFABSERVER_API || UNITY_EDITOR
            PlayFabEditorDataService.SharedSettings.DeveloperSecretKey = studio.GetTitleSecretKey(newTitleId);
#endif
            PlayFabEditorPrefsSO.Instance.TitleDataCache.Clear();
        }
        private static Studio GetStudioForTitleId(string titleId)
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

        #endregion
    }
}

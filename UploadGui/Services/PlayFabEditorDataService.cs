using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PlayFab.Json;
using UploadGui.Models;

namespace UploadGui.Services
{

    public class PlayFabEditorDataService 
    {
        #region EditorPref data classes
        public class PlayFab_SharedSettingsProxy
        {
            private readonly Dictionary<string, PropertyInfo> _settingProps = new Dictionary<string, PropertyInfo>();
            private readonly string[] expectedProps = new[] { "titleid", "developersecretkey", "requesttype", "compressapidata", "requestkeepalive", "requesttimeout" };

            public string TitleId { get { return Get<string>("titleid"); } set { Set("titleid", value); } }
            public string DeveloperSecretKey { get { return Get<string>("developersecretkey"); } set { Set("developersecretkey", value); } }



            private T Get<T>(string name)
            {
                PropertyInfo propInfo;
                var success = _settingProps.TryGetValue(name.ToLowerInvariant(), out propInfo);
                T output = !success ? default(T) : (T)propInfo.GetValue(null, null);
                return output;
            }

            private void Set<T>(string name, T value)
            {
                PropertyInfo propInfo;
                if (!_settingProps.TryGetValue(name.ToLowerInvariant(), out propInfo))
                    propInfo = LoadProps(name);
                if (propInfo != null)
                    propInfo.SetValue(null, value, null);
            }

            private PropertyInfo LoadProps(string name = null)
            {
                var playFabSettingsType = GetPlayFabSettings();
                if (playFabSettingsType == null)
                    return null;

                if (string.IsNullOrEmpty(name))
                {
                    for (var i = 0; i < expectedProps.Length; i++)
                        LoadProps(expectedProps[i]);
                    return null;
                }
                else
                {
                    var eachProperty = playFabSettingsType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static);
                    if (eachProperty != null)
                        _settingProps[name.ToLowerInvariant()] = eachProperty;
                    return eachProperty;
                }
            }
        }
        #endregion EditorPref data classes

        public static PlayFab_SharedSettingsProxy SharedSettings = new PlayFab_SharedSettingsProxy();

        

        private static Type playFabSettingsType = null;

        public static Type GetPlayFabSettings()
        {
            if (playFabSettingsType == typeof(object))
                return null; // Sentinel value to indicate that PlayFabSettings doesn't exist
            if (playFabSettingsType != null)
                return playFabSettingsType;

            playFabSettingsType = typeof(object); // Sentinel value to indicate that PlayFabSettings doesn't exist
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in allAssemblies)
            foreach (var eachType in assembly.GetTypes())
                if (eachType.Name == "PlayFabSettings")
                    playFabSettingsType = eachType;
            //if (playFabSettingsType == typeof(object))
            //    Debug.LogWarning("Should not have gotten here: "  + allAssemblies.Length);
            //else
            //    Debug.Log("Found Settings: " + allAssemblies.Length + ", " + playFabSettingsType.Assembly.FullName);
            return playFabSettingsType == typeof(object) ? null : playFabSettingsType;
        }

        public static bool IsDataLoaded = false;

        public static Title ActiveTitle
        {
            get
            {
                if (PlayFabEditorPrefsSO.Instance.StudioList != null && PlayFabEditorPrefsSO.Instance.StudioList.Count > 0)
                {
                    if (string.IsNullOrEmpty(PlayFabEditorPrefsSO.Instance.SelectedStudio) || PlayFabEditorPrefsSO.Instance.SelectedStudio == "_OVERRIDE_")
                        return new Title { Id = SharedSettings.TitleId, SecretKey = SharedSettings.DeveloperSecretKey, GameManagerUrl = "https://developer.playfab.com" };

                    if (string.IsNullOrEmpty(PlayFabEditorPrefsSO.Instance.SelectedStudio) || string.IsNullOrEmpty(SharedSettings.TitleId))
                        return null;

                    int studioIndex; int titleIndex;
                    if (DoesTitleExistInStudios(SharedSettings.TitleId, out studioIndex, out titleIndex))
                        return PlayFabEditorPrefsSO.Instance.StudioList[studioIndex].Titles[titleIndex];
                }
                return null;
            }
        }

  


        public static bool DoesTitleExistInStudios(string searchFor) //out Studio studio
        {
            if (PlayFabEditorPrefsSO.Instance.StudioList == null)
                return false;
            searchFor = searchFor.ToLower();
            foreach (var studio in PlayFabEditorPrefsSO.Instance.StudioList)
                if (studio.Titles != null)
                    foreach (var title in studio.Titles)
                        if (title.Id.ToLower() == searchFor)
                            return true;
            return false;
        }

        private static bool DoesTitleExistInStudios(string searchFor, out int studioIndex, out int titleIndex) //out Studio studio
        {
            studioIndex = 0; // corresponds to our _OVERRIDE_
            titleIndex = -1;

            if (PlayFabEditorPrefsSO.Instance.StudioList == null)
                return false;

            for (var studioIdx = 0; studioIdx < PlayFabEditorPrefsSO.Instance.StudioList.Count; studioIdx++)
            {
                for (var titleIdx = 0; titleIdx < PlayFabEditorPrefsSO.Instance.StudioList[studioIdx].Titles.Length; titleIdx++)
                {
                    if (PlayFabEditorPrefsSO.Instance.StudioList[studioIdx].Titles[titleIdx].Id.ToLower() == searchFor.ToLower())
                    {
                        studioIndex = studioIdx;
                        titleIndex = titleIdx;
                        return true;
                    }
                }
            }

            return false;
        }

        public static void RefreshStudiosList(bool onlyIfNull = false)
        {
            if (string.IsNullOrEmpty(PlayFabEditorPrefsSO.Instance.DevAccountToken))
                return; // Can't load studios when not logged in
            if (onlyIfNull && PlayFabEditorPrefsSO.Instance.StudioList != null)
                return; // Don't spam load this, only load it the first time

            if (PlayFabEditorPrefsSO.Instance.StudioList != null)
                PlayFabEditorPrefsSO.Instance.StudioList.Clear();
            PlayFabEditorApi.GetStudios(new GetStudiosRequest(), (getStudioResult) =>
            {
                if (PlayFabEditorPrefsSO.Instance.StudioList == null)
                    PlayFabEditorPrefsSO.Instance.StudioList = new List<Studio>();
                foreach (var eachStudio in getStudioResult.Studios)
                    PlayFabEditorPrefsSO.Instance.StudioList.Add(eachStudio);
                PlayFabEditorPrefsSO.Instance.StudioList.Add(Studio.OVERRIDE);
            });
        }
    }
}

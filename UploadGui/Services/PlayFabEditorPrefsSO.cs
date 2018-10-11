using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UploadGui.Models;

namespace UploadGui.Services
{

    public class PlayFabEditorPrefsSO
    {
        private static PlayFabEditorPrefsSO _instance;
        public static PlayFabEditorPrefsSO Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                
                if (_instance != null)
                    return _instance;

                _instance = new PlayFabEditorPrefsSO();
               
                return _instance;
            }
        }
       

        public string DevAccountEmail;
        public string DevAccountToken;

        public List<Studio> StudioList = null; // Null means not fetched, empty is a possible return result from GetStudios
        public string SelectedStudio;

        public readonly Dictionary<string, string> TitleDataCache = new Dictionary<string, string>();
        public readonly Dictionary<string, string> InternalTitleDataCache = new Dictionary<string, string>();

        public string SdkPath;
        public string EdExPath;
        public string LocalCloudScriptPath;

        private string _latestSdkVersion;
        private string _latestEdExVersion;
        private DateTime _lastSdkVersionCheck;
        private DateTime _lastEdExVersionCheck;
        public bool PanelIsShown;
        public string EdSet_latestSdkVersion { get { return _latestSdkVersion; } set { _latestSdkVersion = value; _lastSdkVersionCheck = DateTime.UtcNow; } }
        public string EdSet_latestEdExVersion { get { return _latestEdExVersion; } set { _latestEdExVersion = value; _lastEdExVersionCheck = DateTime.UtcNow; } }
        public DateTime EdSet_lastSdkVersionCheck { get { return _lastSdkVersionCheck; } }
        public DateTime EdSet_lastEdExVersionCheck { get { return _lastEdExVersionCheck; } }

        public int curMainMenuIdx;
        public int curSubMenuIdx;
    }
}

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
        

        private string _latestSdkVersion;
        private string _latestEdExVersion;
    }
}

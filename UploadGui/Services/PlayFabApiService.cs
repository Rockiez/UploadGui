using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadGui.Models;

namespace UploadGui.Services
{

    public class PlayFabEditorApi
    {
        #region FROM EDITOR API SETS 

        public static void Login(LoginRequest request, Action<LoginResult> resultCallback)
        {
            PlayFabEditorHttpService.MakeApiCall("/DeveloperTools/User/Login","https://editor.playfabapi.com", request, resultCallback);
        }

        public static void GetStudios(GetStudiosRequest request, Action<GetStudiosResult> resultCallback)
        {
            var token = PlayFabEditorPrefsSO.Instance.DevAccountToken;
            request.DeveloperClientToken = token;
            PlayFabEditorHttpService.MakeApiCall("/DeveloperTools/User/GetStudios", "https://editor.playfabapi.com", request, resultCallback);
        }


        #region FROM ADMIN / SERVER API SETS ----------------------------------------------------------------------------------------------------------------------------------------
        public static void GetTitleData(Action<GetTitleDataResult> resultCb)
        {
            var titleId = PlayFabEditorDataService.SharedSettings.TitleId;
            var apiEndpoint = "https://" + titleId + ".playfabapi.com";
            PlayFabEditorHttpService.MakeApiCall("/Admin/GetTitleData", apiEndpoint, new GetTitleDataRequest(), resultCb);
        }



        #endregion
    }
#endregion
}

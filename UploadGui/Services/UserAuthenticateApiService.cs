using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UploadGui.Models;

namespace UploadGui.Services
{

    public class UserAuthenticateApiService
    {
        #region FROM EDITOR API SETS 

        public static async Task Login(LoginRequest request, Action<LoginResult> resultCallback)
        {
            await UserAuthenticateHttpService.MakeApiCall("/DeveloperTools/User/Login","https://editor.playfabapi.com", request, resultCallback);
        }

        public static async Task GetStudios(GetStudiosRequest request, Action<GetStudiosResult> resultCallback,string DevAccountToken)
        {
            var token = DevAccountToken;
            request.DeveloperClientToken = token;
            await UserAuthenticateHttpService.MakeApiCall("/DeveloperTools/User/GetStudios", "https://editor.playfabapi.com", request, resultCallback);
        }



        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PlayFab.Json;
using UploadGui.Models;

namespace UploadGui.Services
{

    public class UserAuthenticateApiService
    {
        public static async Task Login(LoginRequest request, Action<LoginResult> resultCallback)
        {
            await MakeApiCall("/DeveloperTools/User/Login","https://editor.playfabapi.com", request, resultCallback);
        }

        public static async Task GetStudios(GetStudiosRequest request, Action<GetStudiosResult> resultCallback,string DevAccountToken)
        {
            var token = DevAccountToken;
            request.DeveloperClientToken = token;
            await MakeApiCall("/DeveloperTools/User/GetStudios", "https://editor.playfabapi.com", request, resultCallback);
        }

        internal static async Task MakeApiCall<TRequestType, TResultType>(string api, string apiEndpoint, TRequestType request, Action<TResultType> resultCallback) where TResultType : class
        {
            var req = JsonWrapper.SerializeObject(request, UserAuthenticateJsonSerializerStrategyService.ApiSerializerStrategy);

            //Set headers
            var headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"},
                {"X-ReportErrorAsSuccess", "true"},
                {"X-PlayFabSDK", "PlayFab_EditorExtensions" + "_" + "2.53.181001"}
            };


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiEndpoint);
                var result = await client.PostAsync(api, new StringContent(
                    req.Trim(), Encoding.UTF8, "application/json"));
                string resultContent = await result.Content.ReadAsStringAsync();
                DeserializeResult(api, resultCallback, resultContent);
            }
        }



        private static void DeserializeResult<TResultType>(string api, Action<TResultType> resultCallback, string response) where TResultType : class
        {
            var httpResult = JsonWrapper.DeserializeObject<HttpResponseObject>(response, UserAuthenticateJsonSerializerStrategyService.ApiSerializerStrategy);
            if (httpResult.code != 200)
            {
                return;
            }

            if (resultCallback == null)
                return;

            TResultType result = null;

            var dataJson = JsonWrapper.SerializeObject(httpResult.data, UserAuthenticateJsonSerializerStrategyService.ApiSerializerStrategy);
            result = JsonWrapper.DeserializeObject<TResultType>(dataJson, UserAuthenticateJsonSerializerStrategyService.ApiSerializerStrategy);

            resultCallback(result);
        }

    }
}

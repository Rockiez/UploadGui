using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PlayFab.Json;
using UploadGui.Models;

namespace UploadGui.Services
{
    class PlayFabEditorHttpService
    {

        internal static async Task MakeApiCall<TRequestType, TResultType>(string api, string apiEndpoint, TRequestType request, Action<TResultType> resultCallback) where TResultType : class
        {
            var url = apiEndpoint + api;
            var req = JsonWrapper.SerializeObject(request, PlayFabEditorUtil.ApiSerializerStrategy);

            //Set headers
            var headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"},
                {"X-ReportErrorAsSuccess", "true"},
                {"X-PlayFabSDK", "PlayFab_EditorExtensions" + "_" + "2.53.181001"}
            };

            if (api.Contains("/Server/") || api.Contains("/Admin/"))
            {
                if (PlayFabEditorDataService.ActiveTitle == null || string.IsNullOrEmpty(PlayFabEditorDataService.ActiveTitle.SecretKey))
                {
                    PlayFabEditorDataService.RefreshStudiosList();
                    return;
                }

                headers.Add("X-SecretKey", PlayFabEditorDataService.ActiveTitle.SecretKey);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiEndpoint);
                var result = await client.PostAsync(api, new StringContent(
                    req.Trim(), Encoding.UTF8, "application/json"));
                string resultContent = await result.Content.ReadAsStringAsync();
                OnWwwSuccess(api, resultCallback, resultContent);
            }




        }



        private static void OnWwwSuccess<TResultType>(string api, Action<TResultType> resultCallback,string response) where TResultType : class
        {
            var httpResult = JsonWrapper.DeserializeObject<HttpResponseObject>(response, PlayFabEditorUtil.ApiSerializerStrategy);
            if (httpResult.code != 200)
            {
                return;
            }

            if (resultCallback == null)
                return;

            TResultType result = null;
            var resultAssigned = false;

            var dataJson = JsonWrapper.SerializeObject(httpResult.data, PlayFabEditorUtil.ApiSerializerStrategy);
            result = JsonWrapper.DeserializeObject<TResultType>(dataJson, PlayFabEditorUtil.ApiSerializerStrategy);

            resultCallback(result);
        }


    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayFab.Json;
using UnityEngine;
using UploadGui.Models;

namespace UploadGui.Services
{
    class PlayFabEditorHttpService
    {

        internal static void MakeApiCall<TRequestType, TResultType>(string api, string apiEndpoint, TRequestType request, Action<TResultType> resultCallback) where TResultType : class
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

            //Encode Payload
            var payload = System.Text.Encoding.UTF8.GetBytes(req.Trim());
            var www = new WWW(url, payload, headers);


        }


    }
}

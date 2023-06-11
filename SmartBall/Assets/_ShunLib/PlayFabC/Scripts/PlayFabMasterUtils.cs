using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

namespace ShunLib.PlayFab.Utils.Master
{
    public class PlayFabMasterUtils : PlayFabBaseUtils
    {
        // マスタデータの取得
        public static async Task GetMasterDataAsync(
            List<string> keyList ,Action<GetTitleDataResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new GetTitleDataRequest()
            {
                Keys = keyList,
            };
            var response = await PlayFabClientAPI.GetTitleDataAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("MasterData取得完了");
                successCallback?.Invoke(response.Result);
            }
        }
    }
}


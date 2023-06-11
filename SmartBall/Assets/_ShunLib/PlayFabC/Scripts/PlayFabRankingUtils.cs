using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using ShunLib.PlayFab.Utils;

namespace ShunLib.PlayFab.Utils.Item
{
    public class PlayFabRankingUtils : PlayFabBaseUtils
    {
        // スコア送信API
        public static async Task UpdatePlayerStatisticsAsync(
            List<StatisticUpdate> statisticUpdateList, Action<UpdatePlayerStatisticsResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = statisticUpdateList
            };
            var response = await PlayFabClientAPI.UpdatePlayerStatisticsAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("統計情報（ランキング）のスコア送信成功");
                successCallback?.Invoke(response.Result);
            }
        }

        // スコア送信
        public static async Task<List<StatisticUpdate>> SendScoreAsync(
            string statisticName, int value, uint version
        )
        {
            List<StatisticUpdate> list = new List<StatisticUpdate>();
            list.Add(CreateStatisticUpdate(statisticName, value, version));
            return list;
        }

        // スコア送信用インスタンスクラスの生成
        public static StatisticUpdate CreateStatisticUpdate(
            string statisticName, int value, uint version
        )
        {
            return new StatisticUpdate{
                StatisticName = statisticName,
                Value = value,
                Version = version
            };
        }

        // 統計情報（ランキング）の取得
        public static async Task GetLeaderboardAsync(
            string statisticName, int maxResultsCount = 10, int startPosition = 0,
            Action<GetLeaderboardResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = statisticName,
                StartPosition = startPosition,
                MaxResultsCount = maxResultsCount,
                ProfileConstraints = new PlayerProfileViewConstraints{
                    ShowDisplayName = true
                }
            };
            var response = await PlayFabClientAPI.GetLeaderboardAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("統計情報（ランキング）の取得成功");
                successCallback?.Invoke(response.Result);
            }
        }
    }
}


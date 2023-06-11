using System;
using UnityEngine;

using PlayFab;
using PlayFab.Internal;
using PlayFab.ClientModels;

namespace ShunLib.PlayFab.Utils
{
    public class PlayFabBaseUtils
    {
        // ログ出力フラグ
        public static bool isLog;

        // 通常ログ出力
        protected static void Log(string str)
        {
            if (isLog) Debug.Log("<color=black>[PlayFab]</color>" + str);
        }

        // 成功ログ出力
        protected static void LogSuccess(string str)
        {
            if (isLog) Debug.Log("<color=black>[PlayFab]</color><color=green>" + str + "</color>");
        }

        // エラーログ出力
        protected static void LogError(string str)
        {
            if (isLog) Debug.Log("<color=black>[PlayFab]</color><color=red>" + str + "</color>");
        }

        // APIの結果を判定する。エラーならコールバックを実行してFalseを返す。正常ならTrue
        protected static bool CheckResponse(PlayFabError error = null, Action<PlayFabError> errorCallback = null)
        {
            if (error != null)
            {
                LogError(error.ErrorMessage);
                errorCallback?.Invoke(error);
                return false;
            }
            else return true;
        }
    }
}



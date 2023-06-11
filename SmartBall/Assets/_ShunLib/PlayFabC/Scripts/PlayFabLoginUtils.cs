using System;
using System.Threading.Tasks;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using ShunLib.Manager.Game;
using ShunLib.Utils.Common;

namespace ShunLib.PlayFab.Utils.Login
{
    public class PlayFabLoginUtils : PlayFabBaseUtils
    {
        // 初めてのログインならIDを生成する。初めてでないなら所持IDでログイン
        public static async Task LoginAsync(
            Func<LoginResult, Task> successCallbackAsync = null, Action<PlayFabError> errorCallback = null
        )
        {
            if (
                GameManager.Instance.dataManager.Data.User.UserAccountInfo.CustomIdInfo == null || 
                GameManager.Instance.dataManager.Data.User.UserAccountInfo.CustomIdInfo.CustomId == null || 
                GameManager.Instance.dataManager.Data.User.UserAccountInfo.CustomIdInfo.CustomId == ""
            )
            {
                Log("新規のCustomIdを生成して匿名ログイン");
                string customId = CommonUtils.CreateGUID();
                await LoginAsync(customId, successCallbackAsync, errorCallback);
            }
            else
            {
                Log("セーブデータのCustomIDで匿名ログイン");
                await LoginAsync(
                    GameManager.Instance.dataManager.Data.User.UserAccountInfo.CustomIdInfo.CustomId,
                    successCallbackAsync,
                    errorCallback
                );
            }
        }

        // 匿名ログイン
        public static async Task LoginAsync(
            string customId, Func<LoginResult, Task> successCallbackAsync = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new LoginWithCustomIDRequest
            {
                CustomId = customId,
                CreateAccount = true,
                // ログインと同時にサーバから各データを取得するようにフラグを立てる
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
                {
                    GetCharacterInventories = true,
                    GetCharacterList = true,
                    GetPlayerProfile = true,
                    GetPlayerStatistics = true,
                    GetTitleData = true,
                    GetUserAccountInfo = true,
                    GetUserData = true,
                    GetUserInventory = true,
                    GetUserReadOnlyData = true,
                    GetUserVirtualCurrency = true
                }
            };
            var response = await PlayFabClientAPI.LoginWithCustomIDAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("匿名ログイン成功");
                await successCallbackAsync?.Invoke(response.Result);
            }
        }

        // メールアドレスとパスワードでログイン
        public static async Task LoginMailAndPasswordAsync(
            string mail, string password, Action<LoginResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = mail,
                Password = password,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetUserAccountInfo = true
                }
            };
            var response = await PlayFabClientAPI.LoginWithEmailAddressAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("メールアドレスとパスワードによるログイン成功");
                GameManager.Instance.dataManager.Data.User.Mailaddress = mail;
                await GameManager.Instance.dataManager.Save();
                successCallback?.Invoke(response.Result);
            }
        }
        // アカウントにメールアドレスとパスワードを紐づける
        public static async Task SetMailAndPasswordAsync(
            string mail, string password, Action<AddUsernamePasswordResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new AddUsernamePasswordRequest
            {
                Username = GameManager.Instance.dataManager.Data.User.UserAccountInfo.PlayFabId,
                Email = mail,
                Password = password
            };
            var response = await PlayFabClientAPI.AddUsernamePasswordAsync(request);
            void ErrorCallback(PlayFabError error)
            {
                ErrorMailAndPassword(error);
                errorCallback?.Invoke(error);
            }
            if (CheckResponse(response.Error, ErrorCallback))
            {
                LogSuccess("メールアドレスとパスワードの紐づけ完了");
                GameManager.Instance.dataManager.Data.User.Mailaddress = mail;
                await GameManager.Instance.dataManager.Save();
                successCallback?.Invoke(response.Result);
            }
        }

        // メールアドレスとパスワード関連のエラー処理
        public static void ErrorMailAndPassword(PlayFabError error)
        {
            if (error != null)
            {
                switch (error.Error)
                {
                    case PlayFabErrorCode.InvalidParams:
                        LogError("有効なメールアドレスと6~100文字以内のパスワードを入力してください");
                        break;
                    case PlayFabErrorCode.EmailAddressNotAvailable:
                        LogError("このメールアドレスは既に使用されています");
                        break;
                    case PlayFabErrorCode.InvalidEmailAddress:
                        LogError("このメールアドレスは使用できません");
                        break;
                    case PlayFabErrorCode.InvalidPassword:
                        LogError("このパスワードは無効です");
                        break;
                    default:
                        LogError(error.ErrorMessage);
                        break;
                }
            }
        }
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using ShunLib.Manager.Game;

namespace ShunLib.PlayFab.Utils.User
{
    public class PlayFabUserUtils : PlayFabBaseUtils
    {
        // PlayerDataの作成・更新
        public static async Task UpdateUserDataAsync(
            Action<UpdateUserDataResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>()
                {
                    {"SaveData", GameManager.Instance.dataManager.GetDataStr()}
                },
            };
            var response = await PlayFabClientAPI.UpdateUserDataAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("PlayerDataの作成OR更新完了");
                successCallback?.Invoke(response.Result);
            }
        }

        // PlayerDataの削除
        public static async Task DeleteUserDataAsync(
            List<string> removeKeyList, Action<UpdateUserDataResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new UpdateUserDataRequest()
            {
                // KeysToRemove = new List<string>(){"SaveData"}
                KeysToRemove = removeKeyList
            };
            var response = await PlayFabClientAPI.UpdateUserDataAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("PlayerDataの削除完了");
                successCallback?.Invoke(response.Result);
            }
        }

        // PlayerDataの取得
        public static async Task GetUserDataAsync(
            string playFabId, Action<GetUserDataResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new GetUserDataRequest()
            {
                PlayFabId = playFabId,
            };
            var response = await PlayFabClientAPI.GetUserDataAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("PlayerDataの取得完了[" + response.Result.Data["SaveData"].Value + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // Playerの表示名を変更
        public static async Task UpdateDisplayNameAsync(
            string newName, Action<UpdateUserTitleDisplayNameResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = newName,
            };
            var response = await PlayFabClientAPI.UpdateUserTitleDisplayNameAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("Playerの表示名変更完了[" + newName + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // 他のユーザをフレンド追加(PlayFabId)
        public static async Task AddFriendAsync(
            string playFabId, Action<AddFriendResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new AddFriendRequest()
            {
                FriendPlayFabId = playFabId,
            };
            var response = await PlayFabClientAPI.AddFriendAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("フレンド追加完了[" + playFabId + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // 他のユーザをフレンド追加(メールアドレス)
        public static async Task AddFriendByMailAsync(
            string mail, Action<AddFriendResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new AddFriendRequest()
            {
                FriendEmail = mail,
            };
            var response = await PlayFabClientAPI.AddFriendAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("フレンド追加完了[" + mail + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // 他のユーザをフレンド追加(表示名)
        public static async Task AddFriendByDisplayNameAsync(
            string displayName, Action<AddFriendResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new AddFriendRequest()
            {
                FriendTitleDisplayName = displayName,
            };
            var response = await PlayFabClientAPI.AddFriendAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("フレンド追加完了[" + displayName + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // 他のユーザをフレンド追加(プレイヤー名)
        public static async Task AddFriendByUserNameAsync(
            string userName, Action<AddFriendResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new AddFriendRequest()
            {
                FriendUsername = userName,
            };
            var response = await PlayFabClientAPI.AddFriendAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("フレンド追加完了[" + userName + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // フレンド一覧の取得
        public static async Task GetFriendListAsync(
            Action<GetFriendsListResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new GetFriendsListRequest()
            {
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowContactEmailAddresses = true,   // メールアドレス
                    ShowCreated = true,                 // アカウント作成日
                    ShowDisplayName = true,             // 表示名
                    ShowLastLogin = true,               // 最終ログイン日時
                }
            };
            var response = await PlayFabClientAPI.GetFriendsListAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                string list = "";
                foreach(FriendInfo friend in response.Result.Friends)
                {
                    list += friend.TitleDisplayName + ",";
                }
                LogSuccess("フレンド一覧取得完了[" + list + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // フレンド削除
        public static async Task DeleteFriendAsync(
            string playFabId, Action<RemoveFriendResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new RemoveFriendRequest()
            {
                FriendPlayFabId = playFabId,
            };
            var response = await PlayFabClientAPI.RemoveFriendAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("フレンド削除完了[" + playFabId + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // 仮想通貨の増加
        public static async Task AddUserVirtualCurrencyAsync(
            int value, string virtualCurrency, Action<ModifyUserVirtualCurrencyResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new AddUserVirtualCurrencyRequest()
            {
                Amount = value,
                VirtualCurrency = virtualCurrency
            };
            var response = await PlayFabClientAPI.AddUserVirtualCurrencyAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("仮想通貨増加完了[+" + value + "]");
                successCallback?.Invoke(response.Result);
            }
        }

        // 仮想通貨の減少
        public static async Task SubtractUserVirtualCurrencyAsync(
            int value, string virtualCurrency, Action<ModifyUserVirtualCurrencyResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new SubtractUserVirtualCurrencyRequest()
            {
                Amount = value,
                VirtualCurrency = virtualCurrency
            };
            var response = await PlayFabClientAPI.SubtractUserVirtualCurrencyAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("仮想通貨減少完了[-" + value + "]");
                successCallback?.Invoke(response.Result);
            }
        }
    }
}
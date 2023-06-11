using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using ShunLib.Singleton;
using ShunLib.Manager.Game;
using ShunLib.PlayFab.Utils;
using ShunLib.PlayFab.Utils.Login;
using ShunLib.PlayFab.Utils.User;

namespace ShunLib.PlayFab.Manager
{
    public class PlayFabManager : SingletonMonoBehaviour<PlayFabManager>
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("タイトルID")]
        [SerializeField] protected string _titleId = default;
        [Header("ログ出力")]
        [SerializeField] protected bool _isLog = false;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // ログインフラグ
        public bool IsLogin { get; set; }

        // ログイン成功時コールバック
        public Action LoginSuccesuCallback { get; set; }
        // ログイン失敗時コールバック
        public Action<PlayFabError> LoginErrorCallback { get; set; }
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private Dictionary<string, string> _masterData = default;
        private Dictionary<string, VirtualCurrencyRechargeTime> _rechargeTimeInfo = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public async Task Initialize()
        {
            await InitializePlayFab();
        }

        // ---------- Private関数 ----------

        // 初期化
        private async Task InitializePlayFab()
        {
            IsLogin = false;
            PlayFabSettings.staticSettings.TitleId = _titleId;
            PlayFabBaseUtils.isLog = _isLog;
            _masterData = new Dictionary<string, string>();
            _rechargeTimeInfo = new Dictionary<string, VirtualCurrencyRechargeTime>();
            await PlayFabLoginUtils.LoginAsync(async (result) => {
                await SetMasterData(result);
                await SetPlayerData(result);
                LoginSuccesuCallback?.Invoke();
                IsLogin = true;
            }, (error) => {
                LoginErrorCallback?.Invoke(error);
            });
        }

        // ログイン結果から取得したマスタデータを格納
        private Task SetMasterData(LoginResult result)
        {
            // サーバからマスタの取得(KeyValueでJSON文字列を受け取っている)
            _masterData = result.InfoResultPayload.TitleData;
            _rechargeTimeInfo = result.InfoResultPayload.UserVirtualCurrencyRechargeTimes;
            return Task.CompletedTask;
        }

        // ログイン結果から取得したプレイヤーデータを格納
        private async Task SetPlayerData(LoginResult result)
        {
            GameManager.Instance.dataManager.Data.User.UserAccountInfo = result.InfoResultPayload.AccountInfo;
            GameManager.Instance.dataManager.Data.User.InventoryList = result.InfoResultPayload.CharacterInventories;
            GameManager.Instance.dataManager.Data.User.CharacterList = result.InfoResultPayload.CharacterList;
            GameManager.Instance.dataManager.Data.User.Profile = result.InfoResultPayload.PlayerProfile;
            GameManager.Instance.dataManager.Data.User.UserInventory = result.InfoResultPayload.UserInventory;
            GameManager.Instance.dataManager.Data.User.VirtualCurrency = result.InfoResultPayload.UserVirtualCurrency;
            await GameManager.Instance.dataManager.Save();
        }

        // ---------- protected関数 ---------
    }
}
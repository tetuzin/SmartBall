using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using SoftGear.Strix.Net.Serialization;

using ShunLib.Singleton;
using ShunLib.Manager.Game;
using ShunLib.Strix.Manager.Room;

namespace ShunLib.Strix.Manager.Master
{
    public class StrixMasterManager : SingletonMonoBehaviour<StrixMasterManager>
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        
        [Header("ホスト")]
        [SerializeField] private string _host = default;
        [Header("ポート")]
        [SerializeField] private int _port = 9122;
        [Header("アプリケーションID")]
        [SerializeField] private string _applicationId = default;
        [Header("コンソールへのログ出力")]
        [SerializeField] private bool _isLogOutput = true;

        public string playerName = "TestPlayer";
        public string playerId = "TestPlayerId";

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public bool IsInitialize
        {
            get { return _isInitialize; }
        }

        // マスタサーバに接続中か
        public bool IsConnected
        {
            get { return StrixNetwork.instance.masterSession.IsConnected; }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool _isInitialize = default;
        private Action<StrixNetworkConnectEventArgs> _connectedEvent = default;
        private Action<StrixNetworkConnectFailedEventArgs> _connectFailedEvent = default;
        private Action<StrixNetworkCloseEventArgs> _closedEvent = default;
        private Action<StrixNetworkErrorEventArgs> _errorThrownEvent = default;
        private Action _onConnectStartCallback = default;
        private Action<StrixNetworkConnectEventArgs> _onConnectCallback = default;
        private Action<StrixNetworkConnectFailedEventArgs> _onConnectFailedCallback = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public async Task Initialize()
        {
            ObjectFactory.Instance.Register(typeof(ShunLib.Strix.Manager.Message.StrixMessageManager));
            ObjectFactory.Instance.Register(typeof(ShunLib.Strix.Model.Message.StrixMessageModel));
            ObjectFactory.Instance.Register(typeof(Pachinko.Machine.PachinkoMachine));
            ObjectFactory.Instance.Register(typeof(Pachinko.Manager.Pachinko.PachinkoManager));
            ObjectFactory.Instance.Register(typeof(Pachinko.Model.PlayDataModel));
            ObjectFactory.Instance.Register(typeof(Pachinko.Model.HoldModel));
            ObjectFactory.Instance.Register(typeof(Pachinko.Model.DirectionModel));
            // セッションイベント設定
            if (
                GameManager.Instance.dataManager.Data.User.UserAccountInfo.TitleInfo != null &&
                GameManager.Instance.dataManager.Data.User.UserAccountInfo.TitleInfo.DisplayName != null
            )
            {
                playerName = GameManager.Instance.dataManager.Data.User.UserAccountInfo.TitleInfo.DisplayName;
            }
            if (
                GameManager.Instance.dataManager.Data.User.UserAccountInfo.TitleInfo != null &&
                GameManager.Instance.dataManager.Data.User.UserAccountInfo.TitleInfo.TitlePlayerAccount.Id != null
            )
            {
                playerId = GameManager.Instance.dataManager.Data.User.UserAccountInfo.TitleInfo.TitlePlayerAccount.Id;
            }
            StrixNetwork.instance.masterSession.Connected += ConnectedEvent;
            StrixNetwork.instance.masterSession.ConnectFailed += ConnectFailedEvent;
            StrixNetwork.instance.masterSession.Closed += ClosedEvent;
            StrixNetwork.instance.masterSession.ErrorThrown += ErrorThrownEvent;
            _isInitialize = true;
            await Task.CompletedTask;
        }

        // マスタサーバへ接続
        public void Connect()
        {
            StrixNetwork.instance.applicationId = _applicationId;
            StrixNetwork.instance.playerName = name;
            StrixNetwork.instance.ConnectMasterServer(
                host: _host,
                port: _port,
                connectEventHandler: connectResult => OnConnectCallback(connectResult),
                errorEventHandler: connectError => OnConnectFailedCallback(connectError)
            );
        }

        // マスタサーバから切断
        public void DisConnect()
        {
            if(!IsConnected) return;
            StrixNetwork.instance.DisconnectMasterServer();
            Log("<color=blue>マスターサーバから切断しました。</color>");
        }

        // 接続確立時イベントの設定
        public void SetConnectedEvent(Action<StrixNetworkConnectEventArgs> action)
        {
            _connectedEvent = action;
        }

        // 接続失敗時イベントの設定
        public void SetConnectFailedEvent(Action<StrixNetworkConnectFailedEventArgs> action)
        {
            _connectFailedEvent = action;
        }

        // 接続終了時イベントの設定
        public void SetClosedEvent(Action<StrixNetworkCloseEventArgs> action)
        {
            _closedEvent = action;
        }

        // 接続エラー発生時イベントの設定
        public void SetErrorThrownEvent(Action<StrixNetworkErrorEventArgs> action)
        {
            _errorThrownEvent = action;
        }

        // マスタサーバ接続開始時コールバックの設定
        public void SetOnConnectStartCallbackEvent(Action action)
        {
            _onConnectStartCallback = action;
        }

        // マスタサーバ接続成功時コールバックの設定
        public void SetOnConnectCallbackEvent(Action<StrixNetworkConnectEventArgs> action)
        {
            _onConnectCallback = action;
        }

        // マスタサーバ接続失敗時コールバックの設定
        public void SetOnConnectFailedCallbackEvent(Action<StrixNetworkConnectFailedEventArgs> action)
        {
            _onConnectFailedCallback = action;
        }

        // ---------- Private関数 ----------

        // 接続確立時イベント
        private void ConnectedEvent(StrixNetworkConnectEventArgs args)
        {
            Log("<color=green>マスターセッション接続確立</color>");
            _connectedEvent?.Invoke(args);
        }

        // 接続失敗時イベント
        private void ConnectFailedEvent(StrixNetworkConnectFailedEventArgs args)
        {
            Log("<color=green>マスターセッション接続失敗</color>:" + args.cause);
            _connectFailedEvent?.Invoke(args);
        }

        // 接続終了時イベント
        private void ClosedEvent(StrixNetworkCloseEventArgs args)
        {
            Log("<color=green>マスターセッション接続終了</color>");
            _closedEvent?.Invoke(args);
        }

        // 接続エラー発生時イベント
        private void ErrorThrownEvent(StrixNetworkErrorEventArgs args)
        {
            Log("<color=green>マスターセッション接続エラー</color>:" + args.cause);
            _errorThrownEvent?.Invoke(args);
        }

        // マスタサーバ接続成功時コールバック
        private void OnConnectCallback(StrixNetworkConnectEventArgs args)
        {
            Log("<color=blue>マスターサーバに接続しました。</color>");
            _onConnectCallback?.Invoke(args);
        }

        // マスタサーバ接続失敗時コールバック
        private void OnConnectFailedCallback(StrixNetworkConnectFailedEventArgs args)
        {
            Log("<color=red>マスターサーバに接続できませんでした。</color>:" + args.cause);
            _onConnectFailedCallback?.Invoke(args);
        }

        // ログ出力
        private void Log(string str)
        {
            if (_isLogOutput) Debug.Log(str);
        }

        // ---------- protected関数 ---------
    }
}



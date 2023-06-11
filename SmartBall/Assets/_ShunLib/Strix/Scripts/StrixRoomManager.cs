using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using SoftGear.Strix.Client.Room.Model;
using SoftGear.Strix.Client.Room;
using SoftGear.Strix.Client.Room.Message;
using SoftGear.Strix.Client.Match;
using SoftGear.Strix.Client.Match.Room.Message;
using SoftGear.Strix.Client.Match.Room.Model;
using SoftGear.Strix.Client.Core;

using ShunLib.Singleton;

namespace ShunLib.Strix.Manager.Room
{
    public class StrixRoomManager : SingletonMonoBehaviour<StrixRoomManager>
    {
        // ---------- 定数宣言 ----------

        // ルームプロパティ用定数
        public const string MEMBER_LIST = "member_list";

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("コンソールへのログ出力")]
        [SerializeField] private bool _isLogOutput = true;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public bool IsInitialize
        {
            get { return _isInitialize; }
        }

        // ルームサーバに接続中か
        public bool IsConnected
        {
            get { return StrixNetwork.instance.roomSession.IsConnected; }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // ルームに参加可能な最大人数
        public int capacity = 4;
        // ルーム名
        public string roomName = "New Room";
        private bool _isInitialize = default;
        // 検索ルームリスト
        private List<RoomInfo> _roomInfoList = default;
        // ルームにいるメンバーのリスト
        private Dictionary<UID, MatchRoomMember> _memberList = default;

        // コールバックイベント
        private Action _onJoinRoomStartCallback = default;
        private Action<RoomJoinEventArgs> _onJoinRoomCallback = default;
        private Action<FailureEventArgs> _onJoinRoomFailedCallback = default;
        private Action _onSearchRoomStartCallback = default;
        private Action<RoomSearchEventArgs> _onSearchRoomCallback = default;
        private Action<FailureEventArgs> _onSearchRoomFailedCallback = default;
        private Action _onCreateRoomStartCallback = default;
        private Action<RoomCreateEventArgs> _onCreateRoomCallback = default;
        private Action<FailureEventArgs> _onCreateRoomFailedCallback = default;
        // セッションイベント
        private Action<StrixNetworkConnectEventArgs> _connectedEvent = default;
        private Action<StrixNetworkConnectFailedEventArgs> _connectFailedEvent = default;
        private Action<StrixNetworkCloseEventArgs> _closedEvent = default;
        private Action<StrixNetworkErrorEventArgs> _errorThrownEvent = default;
        // ルームイベント
        private Action<RoomCreatedEventArgs<CustomizableMatchRoom>> _roomCreatedEvent = default;
        private Action<RoomJoinedEventArgs<CustomizableMatchRoom, CustomizableMatchRoomMember>> _roomJoinedEvent = default;
        private Action<RoomLeftEventArgs<CustomizableMatchRoom, CustomizableMatchRoomMember>> _roomLeftEvent = default;
        private Action<RoomSetEventArgs<CustomizableMatchRoom>> _roomSetEvent = default;
        private Action<RoomDeletedEventArgs<CustomizableMatchRoom>> _roomDeletedEvent = default;
        private Action<NotificationEventArgs<RoomJoinNotification<CustomizableMatchRoom>>> _roomJoinNotifiedEvent = default;
        private Action<NotificationEventArgs<RoomLeaveNotification<CustomizableMatchRoom>>> _roomLeaveNotifiedEvent = default;
        private Action<NotificationEventArgs<RoomDeleteNotification<CustomizableMatchRoom>>> _roomDeleteNotifiedEvent = default;
        private Action<NotificationEventArgs<RoomSetNotification<CustomizableMatchRoom>>> _roomSetNotifiedEvent = default;
        private Action<NotificationEventArgs<RoomSetMemberNotification<CustomizableMatchRoomMember>>> _roomSetMemberNotifiedEvent = default;
        private Action<NotificationEventArgs<RoomDirectRelayNotification>> _roomDirectRelayNotifiedEvent = default;
        private Action<NotificationEventArgs<RoomRelayNotification>> _roomRelayNotifiedEvent = default;
        private Action<NotificationEventArgs<MatchRoomKickNotification<CustomizableMatchRoom>>> _matchRoomKickNotifiedEvent = default;
        private Action<MatchRoomClient<CustomizableMatchRoom, CustomizableMatchRoomMember>.RoomOwnerChangedEventArgs<CustomizableMatchRoomMember>> _roomOwnerChangedEvent = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public async Task Initialize()
        {
            _memberList = new Dictionary<UID, MatchRoomMember>();

            // セッションイベント設定
            StrixNetwork.instance.roomSession.Connected += ConnectedEvent;
            StrixNetwork.instance.roomSession.ConnectFailed += ConnectFailedEvent;
            StrixNetwork.instance.roomSession.Closed += ClosedEvent;
            StrixNetwork.instance.roomSession.ErrorThrown += ErrorThrownEvent;
            // ルームイベント設定
            StrixNetwork.instance.roomSession.roomClient.RoomCreated += RoomCreatedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomJoined += RoomJoinedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomLeft += RoomLeftEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomSet += RoomSetEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomDeleted += RoomDeletedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomJoinNotified += RoomJoinNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomLeaveNotified += RoomLeaveNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomDeleteNotified += RoomDeleteNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomSetNotified += RoomSetNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomSetMemberNotified += RoomSetMemberNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomDirectRelayNotified += RoomDirectRelayNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomRelayNotified += RoomRelayNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.MatchRoomKickNotified += MatchRoomKickNotifiedEvent;
            StrixNetwork.instance.roomSession.roomClient.RoomOwnerChanged += RoomOwnerChangedEvent;
            _isInitialize = true;
            await Task.CompletedTask;
        }

        // ルームに参加(指定)
        public void JoinRoom(RoomJoinArgs args)
        {
            _onJoinRoomStartCallback?.Invoke();
            StrixNetwork.instance.JoinRoom(
                args: args,
                handler: joinResult => OnJoinRoomCallback(joinResult),
                failureHandler: joinError => OnJoinRoomFailedCallback(joinError)
            );
        }

        // ルームに参加(ランダム)
        public void JoinRandomRoom()
        {
            _onJoinRoomStartCallback?.Invoke();
            StrixNetwork.instance.JoinRandomRoom(
                playerName: StrixNetwork.instance.playerName,
                handler: joinResult => OnJoinRoomCallback(joinResult),
                failureHandler: joinError => OnJoinRoomFailedCallback(joinError)
            );
        }

        // ルームから切断
        public void DisConnectRoom()
        {
            if (!IsConnected) return;
            StrixNetwork.instance.roomSession.Disconnect();
        }

        // ルームを検索
        public void SearchRoom(
            Action<RoomSearchEventArgs> successCallback = null,
            Action<FailureEventArgs> failedCallback = null
        )
        {
            _onSearchRoomStartCallback?.Invoke();
            StrixNetwork.instance.SearchRoom(
                limit: 10,
                offset: 0,
                handler: searchResults => {
                    OnSearchRoomCallback(searchResults);
                    successCallback?.Invoke(searchResults);
                },
                failureHandler: searchError => {
                    OnSearchRoomFailedCallback(searchError);
                    failedCallback?.Invoke(searchError);
                }
            );
        }

        // ルーム作成
        public void CreateRoom(
            int capacity, string roomName, string playerName,
            Action createRoomCallback = null, Action failedCallback = null
        )
        {
            _onCreateRoomStartCallback?.Invoke();

            this.roomName = roomName;

            // ルーム作成に必要なプロパティを生成
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add(MEMBER_LIST, _memberList);
            
            RoomProperties roomProperties = new RoomProperties {
                capacity = capacity,
                name = this.roomName,
                properties = properties
            };
            RoomMemberProperties memberProperties = new RoomMemberProperties {
                name = playerName
            };

            // ルームを作成し、そのまま入室する
            StrixNetwork.instance.CreateRoom(
                roomProperties: roomProperties,
                memberProperties: memberProperties,
                handler: (createResult) => {
                    OnCreateRoomCallback(createResult);
                    createRoomCallback?.Invoke();
                },
                failureHandler: (createError) => {
                    OnCreateRoomFailedCallback(createError);
                    failedCallback?.Invoke();
                }
            );
        }

        // ルーム参加情報の生成
        public static RoomJoinArgs CreateRoomJoinArgs(
            string host,
	        int port,
	        string protocol,
	        long roomId,
	        string playerName,
            Dictionary<string, object> properties = null)
        {
            RoomJoinArgs args = new RoomJoinArgs();
            args.host = host;
            args.port = port;
            args.protocol = protocol;
            args.roomId = roomId;
            RoomMemberProperties memberProperties = new RoomMemberProperties();
            memberProperties.name = playerName;
            memberProperties.properties = properties;
            args.memberProperties = memberProperties;
            return args;
        }

        // ルーム情報をルーム参加情報へ変換
        public static RoomJoinArgs ConvertRoomInfoToRoomJoinArgs(RoomInfo roomInfo, string playerName)
        {
            return CreateRoomJoinArgs(
                roomInfo.host,
                roomInfo.port,
                roomInfo.protocol,
                roomInfo.roomId,
                playerName
            );
        }

        // ルーム検索結果をリストとして設定
        public void SetRoomInfoList(RoomSearchEventArgs args)
        {
            _roomInfoList = new List<RoomInfo>();
            foreach (RoomInfo roomInfo in args.roomInfoCollection)
            {
                _roomInfoList.Add(roomInfo);
            }
        }

        // ルーム検索結果リストの取得
        public List<RoomInfo> GetRoomInfoList()
        {
            return _roomInfoList;
        }

        // UIDからメンバーを取得
        public CustomizableMatchRoomMember GetMember(UID uid)
        {
            return StrixNetwork.instance.roomMembers[uid];
        }

        // ルーム参加開始時コールバックの設定
        public void SetOnJoinRoomStartCallback(Action action)
        {
            _onJoinRoomStartCallback = action;
        }

        // ルーム参加成功時コールバックの設定
        public void SetOnJoinRoomCallback(Action<RoomJoinEventArgs> action)
        {
            _onJoinRoomCallback = action;
        }

        // ルーム参加失敗時コールバックの設定
        public void SetOnJoinRoomFailedCallback(Action<FailureEventArgs> action)
        {
            _onJoinRoomFailedCallback = action;
        }

        // ルーム検索開始時コールバックの設定
        public void SetOnSearchRoomStartCallback(Action action)
        {
            _onSearchRoomStartCallback = action;
        }

        // ルーム検索成功時コールバックの設定
        public void SetOnSearchRoomCallback(Action<RoomSearchEventArgs> action)
        {
            _onSearchRoomCallback = action;
        }

        // ルーム検索失敗時コールバックの設定
        public void SetOnSearchRoomFailedCallback(Action<FailureEventArgs> action)
        {
            _onSearchRoomFailedCallback = action;
        }

        // ルーム作成開始時コールバックの設定
        public void SetOnCreateRoomStartCallback(Action action)
        {
            _onCreateRoomStartCallback = action;
        }

        // ルーム作成成功時コールバックの設定
        public void SetOnCreateRoomCallback(Action<RoomCreateEventArgs> action)
        {
            _onCreateRoomCallback = action;
        }

        // ルーム作成失敗時コールバックの設定
        public void SetOnCreateRoomFailedCallback(Action<FailureEventArgs> action)
        {
            _onCreateRoomFailedCallback = action;
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

        // ルーム作成時イベント(クライアント)の設定
        public void SetRoomCreatedEvent(Action<RoomCreatedEventArgs<CustomizableMatchRoom>> action)
        {
            _roomCreatedEvent = action;
        }

        // ルーム参加時イベント(クライアント)の設定
        public void SetRoomJoinedEvent(
            Action<RoomJoinedEventArgs<CustomizableMatchRoom, CustomizableMatchRoomMember>> action
        )
        {
            _roomJoinedEvent = action;
        }

        // ルーム退出イベント(クライアント)の設定
        public void SetRoomLeftEvent(
            Action<RoomLeftEventArgs<CustomizableMatchRoom, CustomizableMatchRoomMember>> action
        )
        {
            _roomLeftEvent = action;
        }

        // ルームプロパティ変更時イベント(クライアント＆他クライアント)の設定
        public void SetRoomSetEvent(
            Action<RoomSetEventArgs<CustomizableMatchRoom>> action
        )
        {
            _roomSetEvent = action;
        }

        // ルーム削除時イベント(クライアント＆他クライアント)の設定
        public void SetRoomDeletedEvent(
            Action<RoomDeletedEventArgs<CustomizableMatchRoom>> action
        )
        {
            _roomDeletedEvent = action;
        }

        // ルーム参加時イベント(他クライアント＆サーバ)の設定
        public void SetRoomJoinNotifiedEvent(
            Action<NotificationEventArgs<RoomJoinNotification<CustomizableMatchRoom>>> action
        )
        {
            _roomJoinNotifiedEvent = action;
        }

        // ルーム退出時イベント(他クライアント＆サーバ)の設定
        public void SetRoomLeaveNotifiedEvent(
            Action<NotificationEventArgs<RoomLeaveNotification<CustomizableMatchRoom>>> action
        )
        {
            _roomLeaveNotifiedEvent  = action;
        }

        // ルーム削除時イベント(他クライアント＆サーバ)の設定
        public void SetRoomDeleteNotifiedEvent(
            Action<NotificationEventArgs<RoomDeleteNotification<CustomizableMatchRoom>>> action
        )
        {
            _roomDeleteNotifiedEvent = action;
        }

        // ルームオーナーによるプロパティ変更時イベント(他クライアント＆サーバ)の設定
        public void SetRoomSetNotifiedEvent(
            Action<NotificationEventArgs<RoomSetNotification<CustomizableMatchRoom>>> action
        )
        {
            _roomSetNotifiedEvent = action;
        }

        // ルームメンバーによるプロパティ変更時イベント(他クライアント＆サーバ)の設定
        public void SetRoomSetMemberNotifiedEvent(
            Action<NotificationEventArgs<RoomSetMemberNotification<CustomizableMatchRoomMember>>> action
        )
        {
            _roomSetMemberNotifiedEvent = action;
        }

        // ダイレクトメッセージ受信時イベント(他クライアント＆サーバ)の設定
        public void SetRoomDirectRelayNotifiedEvent(
            Action<NotificationEventArgs<RoomDirectRelayNotification>> action
        )
        {
            _roomDirectRelayNotifiedEvent = action;
        }

        // 全体メッセージイベント(他クライアント＆サーバ)の設定
        public void SetRoomRelayNotifiedEvent(
            Action<NotificationEventArgs<RoomRelayNotification>> action
        )
        {
            _roomRelayNotifiedEvent = action;
        }

        // ルームメンバーのキック時イベント(他クライアント＆サーバ)の設定
        public void SetMatchRoomKickNotifiedEvent(
            Action<NotificationEventArgs<MatchRoomKickNotification<CustomizableMatchRoom>>> action
        )
        {
            _matchRoomKickNotifiedEvent = action;
        }

        // ルームオーナー移譲時イベント(他クライアント＆サーバ)の設定
        public void SetRoomOwnerChangedEvent(
            Action<MatchRoomClient<CustomizableMatchRoom, CustomizableMatchRoomMember>.RoomOwnerChangedEventArgs<CustomizableMatchRoomMember>> action
        )
        {
            _roomOwnerChangedEvent = action;
        }

        // ---------- Private関数 ----------

        // 接続確立時イベント
        private void ConnectedEvent(StrixNetworkConnectEventArgs args)
        {
            Log("<color=green>ルームセッション接続確立</color>");
            _connectedEvent?.Invoke(args);
        }

        // 接続失敗時イベント
        private void ConnectFailedEvent(StrixNetworkConnectFailedEventArgs args)
        {
            Log("<color=green>ルームセッション接続失敗</color>:" + args.cause);
            _connectFailedEvent?.Invoke(args);
        }

        // 接続終了時イベント
        private void ClosedEvent(StrixNetworkCloseEventArgs args)
        {
            Log("<color=green>ルームセッション接続終了</color>");
            _closedEvent?.Invoke(args);
        }

        // 接続エラー発生時イベント
        private void ErrorThrownEvent(StrixNetworkErrorEventArgs args)
        {
            Log("<color=green>ルームセッション接続エラー</color>:" + args.cause);
            _errorThrownEvent?.Invoke(args);
        }

        // ルーム作成時イベント(クライアント)
        private void RoomCreatedEvent(RoomCreatedEventArgs<CustomizableMatchRoom> args)
        {
            Log("<color=green>ルームを作成しました</color>");
            _roomCreatedEvent?.Invoke(args);
        }

        // ルーム参加時イベント(クライアント)
        private void RoomJoinedEvent(
            RoomJoinedEventArgs<CustomizableMatchRoom, CustomizableMatchRoomMember> args
        )
        {
            Log("<color=green>ルームに参加しました</color>:" + args.Member.GetName());
            _roomJoinedEvent?.Invoke(args);
        }

        // ルーム退出イベント(クライアント)
        private void RoomLeftEvent(
            RoomLeftEventArgs<CustomizableMatchRoom, CustomizableMatchRoomMember> args
        )
        {
            Log("<color=green>ルームを退出しました</color>:" + args.Member.GetName());
            _roomLeftEvent?.Invoke(args);
        }

        // ルームプロパティ変更時イベント(クライアント＆他クライアント)
        private void RoomSetEvent(RoomSetEventArgs<CustomizableMatchRoom> args)
        {
            Log("<color=green>ルームプロパティを変更しました</color>");
            _roomSetEvent?.Invoke(args);
        }

        // ルーム削除時イベント(クライアント＆他クライアント)
        private void RoomDeletedEvent(RoomDeletedEventArgs<CustomizableMatchRoom> args)
        {
            Log("<color=green>ルームを削除しました</color>");
            _roomDeletedEvent?.Invoke(args);
        }

        // ルーム参加時イベント(他クライアント＆サーバ)
        private void RoomJoinNotifiedEvent(
            NotificationEventArgs<RoomJoinNotification<CustomizableMatchRoom>> args
        )
        {
            Log("<color=green>プレイヤーが参加しました</color>:" + 
                GetMember(uid:args.Data.GetNewlyJoinedMember().GetUid()).GetName());
            _roomJoinNotifiedEvent?.Invoke(args);
        }

        // ルーム退出時イベント(他クライアント＆サーバ)
        private void RoomLeaveNotifiedEvent(
            NotificationEventArgs<RoomLeaveNotification<CustomizableMatchRoom>> args
        )
        {
            Log("<color=green>プレイヤーが退出しました</color>:" + 
                args.Data.GetGoneMember().GetUid());
            _roomLeaveNotifiedEvent?.Invoke(args);
        }

        // ルーム削除時イベント(他クライアント＆サーバ)
        private void RoomDeleteNotifiedEvent(
            NotificationEventArgs<RoomDeleteNotification<CustomizableMatchRoom>> args
        )
        {
            Log("<color=green>ルームを削除しました</color>");
            _roomDeleteNotifiedEvent?.Invoke(args);
        }

        // ルームオーナーによるプロパティ変更時イベント(他クライアント＆サーバ)
        private void RoomSetNotifiedEvent(
            NotificationEventArgs<RoomSetNotification<CustomizableMatchRoom>> args
        )
        {
            Log("<color=green>ルームオーナーがルームプロパティを変更しました</color>");
            _roomSetNotifiedEvent?.Invoke(args);
        }

        // ルームメンバーによるプロパティ変更時イベント(他クライアント＆サーバ)
        private void RoomSetMemberNotifiedEvent(
            NotificationEventArgs<RoomSetMemberNotification<CustomizableMatchRoomMember>> args
        )
        {
            Log("<color=green>ルームメンバーがルームを作成しました</color>");
            _roomSetMemberNotifiedEvent?.Invoke(args);
        }

        // ダイレクトメッセージ受信時イベント(他クライアント＆サーバ)
        private void RoomDirectRelayNotifiedEvent(
            NotificationEventArgs<RoomDirectRelayNotification> args
        )
        {
            Log("<color=green>ダイレクトメッセージを受信しました</color>");
            _roomDirectRelayNotifiedEvent?.Invoke(args);
        }

        // 全体メッセージイベント(他クライアント＆サーバ)
        private void RoomRelayNotifiedEvent(
            NotificationEventArgs<RoomRelayNotification> args
        )
        {
            Log("<color=green>メッセージを受信しました</color>");
            _roomRelayNotifiedEvent?.Invoke(args);
        }

        // ルームメンバーのキック時イベント(他クライアント＆サーバ)
        private void MatchRoomKickNotifiedEvent(
            NotificationEventArgs<MatchRoomKickNotification<CustomizableMatchRoom>> args
        )
        {
            Log("<color=green>ルームからプレイヤーをキックしました</color>");
            _matchRoomKickNotifiedEvent?.Invoke(args);
        }

        // ルームオーナー移譲時イベント(他クライアント＆サーバ)
        private void RoomOwnerChangedEvent(
            MatchRoomClient<CustomizableMatchRoom, CustomizableMatchRoomMember>.RoomOwnerChangedEventArgs<CustomizableMatchRoomMember> args
        )
        {
            Log("<color=green>ルームオーナーを移譲しました</color>");
            _roomOwnerChangedEvent?.Invoke(args);
        }

        // ルーム参加成功時コールバック
        private void OnJoinRoomCallback(RoomJoinEventArgs args)
        {
            Log("<color=blue>ルームに参加しました。</color>");
            _onJoinRoomCallback?.Invoke(args);
        }

        // ルーム参加失敗時コールバック
        private void OnJoinRoomFailedCallback(FailureEventArgs args)
        {
            Log("<color=red>ルームに参加できませんでした。</color>");
            _onJoinRoomFailedCallback?.Invoke(args);
        }

        // ルーム検索成功時コールバック
        private void OnSearchRoomCallback(RoomSearchEventArgs args)
        {
            Log("<color=blue>ルーム検索に成功しました。</color>");
            _onSearchRoomCallback?.Invoke(args);
        }

        // ルーム検索失敗時コールバック
        private void OnSearchRoomFailedCallback(FailureEventArgs args)
        {
            Log("<color=red>ルーム検索に失敗しました。</color>:" + args.cause);
            _onSearchRoomFailedCallback?.Invoke(args);
        }

        // ルーム作成成功時コールバック
        private void OnCreateRoomCallback(RoomCreateEventArgs args)
        {
            Log("<color=blue>新規ルーム[" + roomName + "]を作成しました。</color>");
            _onCreateRoomCallback?.Invoke(args);
        }

        // ルーム作成失敗時コールバック
        private void OnCreateRoomFailedCallback(FailureEventArgs args)
        {
            Log("<color=red>新規ルーム[" + roomName + "]の作成に失敗しました。</color>");
            _onCreateRoomFailedCallback?.Invoke(args);
        }

        // ログ出力
        private void Log(string str)
        {
            if (_isLogOutput) Debug.Log(str);
        }

        // ---------- protected関数 ---------
    }
}



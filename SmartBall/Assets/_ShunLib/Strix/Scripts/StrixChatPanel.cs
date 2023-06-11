using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Client.Room.Message;
using SoftGear.Strix.Client.Match.Room.Model;
using SoftGear.Strix.Client.Core;

using ShunLib.UI.Panel;
using ShunLib.Manager.Game;
using ShunLib.Const.Audio;

using ShunLib.Strix.Const;
using ShunLib.Strix.Manager.Master;
using ShunLib.Strix.Manager.Room;
using ShunLib.Strix.Manager.Message;
using ShunLib.Strix.Model.Message;

namespace ShunLib.Strix.Panel.Chat
{
    public class StrixChatPanel : CommonPanel
    {
        // ---------- 定数宣言 ----------

        // チャットボタン
        public const string CHAT_BUTTON = "ChatButton";
        // チャット送信ボタン
        public const string CHAT_SEND_BUTTON = "ChatSendButton";
        // チャットテキスト
        public const string CHAT_TEXT = "ChatText";
        // チャット入力フォーム
        public const string CHAT_INPUTFIELD = "ChatInputField";
        // チャットテキストのキャンバスグループ
        public const string CHAT_TEXT_CANVASGROUP = "ChatTextCanvasGroup";
        // チャット入力欄のキャンバスグループ
        public const string CHAT_INPUT_CANVASGROUP = "ChatInputCanvasGroup";

        // チャット非表示までの時間
        public const float HIDE_TEXT_TIME = 5f;
        // チャットのフェードスピード
        public const float CHAT_FADE_SPEED = 0.5f;

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("Strixメッセージマネージャー")]
        [SerializeField] protected StrixMessageManager _strixMessageManager = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool _isShowChatText = default;
        private bool _isShowChat = default;
        private float _elapsedTime = default;

        // ---------- Unity組込関数 ----------

        void Update()
        {
            if (_isShowChatText)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= HIDE_TEXT_TIME)
                {
                    if (!_isShowChat) SetActiveChatCanvasGroup(false);
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            
            _isShowChatText = false;
            _elapsedTime = 0f;

            // Strixルームマネージャーの初期化
            StrixRoomManager.Instance.SetRoomJoinNotifiedEvent(JoinOtherPlayerChat);
            StrixRoomManager.Instance.SetRoomLeaveNotifiedEvent(LeaveOtherPlayerChat);

            // Strixメッセージマネージャーの初期化
            _strixMessageManager.Initialize();
            _strixMessageManager.SetPostMessageAllUserCallback(PostChat);
            _strixMessageManager.SetPostMessageOneUserCallback(PostChat);

            // チャットテキストの設定
            SetTextActive(CHAT_TEXT, true);
            SetCanvasGroupAlpha(CHAT_TEXT_CANVASGROUP, 0f);
            SetText(CHAT_TEXT, "");

            // チャットボタンの設定
            SetButtonActive(CHAT_BUTTON, true);
            SetButtonEvent(CHAT_BUTTON, OnClickChatButton);

            // チャット送信ボタンの設定
            SetButtonActive(CHAT_SEND_BUTTON, true);
            SetButtonEvent(CHAT_SEND_BUTTON, OnClickSendButton);

            // チャット入力キャンバスグループの設定
            SetCanvasGroupActive(CHAT_INPUT_CANVASGROUP, false);
        }

        // ---------- Private関数 ----------

        // チャットボタン押下時処理
        private void OnClickChatButton()
        {
            _isShowChat = !_isShowChat;
            if (!_isShowChatText)
            {
                SetCanvasGroupAlpha(CHAT_TEXT_CANVASGROUP, _isShowChat ? 1f : 0f);
            }
            SetCanvasGroupAlpha(CHAT_INPUT_CANVASGROUP, _isShowChat ? 1f : 0f);
            SetCanvasGroupActive(CHAT_INPUT_CANVASGROUP, _isShowChat);
        }

        // チャット送信
        private void OnClickSendButton()
        {
            if (_inputFieldList.IsValue(CHAT_INPUTFIELD))
            {
                // メッセージ送信処理
                string message = _inputFieldList.GetValue(CHAT_INPUTFIELD).GetInputText();
                if (message == "")
                {
                    // TODO 空文字時の処理
                    return;
                }
                ShowChatText(
                    StrixConst.MESSAGE_COLOR_MYSELF + "[" + StrixMasterManager.Instance.playerName + "]</color>" + message
                );
                _inputFieldList.GetValue(CHAT_INPUTFIELD).CleanText();
                _strixMessageManager.OnSendMessage(message);
            }
        }

        // チャットにメッセージ出力処理
        private void ShowChatText(string message)
        {
            string sendMessage = "\n" + message;
            AddText(CHAT_TEXT,sendMessage);
            SetActiveChatCanvasGroup(true);
        }

        // メッセージ受信時の処理
        private void PostChat(StrixMessageModel message)
        {
            string postMessage = StrixConst.MESSAGE_COLOR_OPPONENT + "[" + message.Name + "]</color>" + message.MainText;
            ShowChatText(postMessage);
        }

        // 他プレイヤーが入室した時のメッセージ表示
        private void JoinOtherPlayerChat(NotificationEventArgs<RoomJoinNotification<CustomizableMatchRoom>> args)
        {
            if (GameManager.IsInstance())
            {
                GameManager.Instance.audioManager.PlaySE(AudioConst.SE_JOIN_ROOM);
            }
            CustomizableMatchRoomMember member = StrixNetwork.instance.roomMembers[args.Data.GetNewlyJoinedMember().GetUid()];
            string message = StrixConst.MESSAGE_COLOR_JOIN_ROOM + "[" + member.GetName() + "]がルームに入室しました</color>";
            // string message = StrixConst.MESSAGE_COLOR_JOIN_ROOM + "新規プレイヤーがルームに入室しました</color>";
            ShowChatText(message);
        }

        // 他プレイヤーが退出した時のメッセージ表示
        private void LeaveOtherPlayerChat(NotificationEventArgs<RoomLeaveNotification<CustomizableMatchRoom>> args)
        {
            CustomizableMatchRoomMember member = StrixNetwork.instance.roomMembers[args.Data.GetGoneMember().GetUid()];
            // string message = StrixConst.MESSAGE_COLOR_LEAVE_ROOM + "[" + member.GetName() + "]がルームを退出しました</color>";
            string message = StrixConst.MESSAGE_COLOR_LEAVE_ROOM + "プレイヤーがルームを退出しました</color>";
            ShowChatText(message);
        }

        // チャットテキストの表示・非表示
        private void SetActiveChatCanvasGroup(bool isShow)
        {
            _isShowChatText = isShow;
            _elapsedTime = 0f;
            FadeCanvasGroup(CHAT_TEXT_CANVASGROUP, isShow, CHAT_FADE_SPEED);
        }

        // ---------- protected関数 ---------
    }
}


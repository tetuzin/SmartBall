using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;
using SoftGear.Strix.Client.Room.Message;
using SoftGear.Strix.Client.Core;
using SoftGear.Strix.Net.Serialization;

using ShunLib.Strix.Model.Message;
using ShunLib.Utils.Date;
using ShunLib.Strix.Manager.Master;
using ShunLib.Strix.Manager.Room;

namespace ShunLib.Strix.Manager.Message
{
    public enum SendType
    {
        ONE,
        ALL
    }
    public class StrixMessageManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // メッセージ受信（一斉）のコールバック
        private Action<StrixMessageModel> _postMessageAllUserCallback = default;
        // メッセージ受信（指定）のコールバック
        private Action<StrixMessageModel> _postMessageOneUserCallback = default;

        // ---------- Unity組込関数 ----------

        // 初期化
        public void Initialize()
        {
            StrixRoomManager.Instance.SetRoomRelayNotifiedEvent(PostMessageAllUser);
            StrixRoomManager.Instance.SetRoomDirectRelayNotifiedEvent(PostMessageOneUser);
        }
        
        // メッセージ送信(一斉)
        public void SendMessageAllUser(StrixMessageModel message)
        {
            if (IsJoinRoom()) return;

            StrixNetwork.instance.SendRoomRelay(
                messageToRelay: message,
                handler: sendResult => OnSendMessageCallback(sendResult, message),
                failureHandler: sendError => OnSendMessageFailedCallback(sendError)
            );
        }
        
        // メッセージ送信(指定)
        public void SendMessageOneUser(StrixMessageModel message)
        {
            if (IsJoinRoom()) return;

            StrixNetwork.instance.SendRoomDirectRelay(
                to: message.To,
                messageToRelay: message,
                handler: sendResult => OnSendDirectMessageCallback(sendResult),
                failureHandler: sendError => OnSendDirectMessageFailedCallback(sendError)
            );
        }

        // メッセージ受信(一斉)
        public void PostMessageAllUser(
            NotificationEventArgs<RoomRelayNotification> args
        )
        {
            if (IsJoinRoom()) return;

            // HACK RPCも使用するイベントなのでキャストで判断ではなく別の方法でチャットを実現したい
            StrixMessageModel message = args.Data.GetMessageToRelay() as StrixMessageModel;
            if (message == null) return;
            Debug.Log("<color=black>Message</color>:" + message.MainText);
            _postMessageAllUserCallback?.Invoke(message);
        }

        // メッセージ受信(指定)
        public void PostMessageOneUser(
            NotificationEventArgs<RoomDirectRelayNotification> args
        )
        {
            if (IsJoinRoom()) return;

            // HACK RPCも使用するイベントなのでキャストで判断ではなく別の方法でチャットを実現したい
            StrixMessageModel message = args.Data.GetMessageToRelay() as StrixMessageModel;
            if (message == null) return;
            Debug.Log("<color=black>Message</color>:[" + message.Name + "]" + message.MainText);
            _postMessageOneUserCallback?.Invoke(message);
        }

        // 送信ボタン押下時処理
        public void OnSendMessage(string message, SendType curSendType = SendType.ALL)
        {
            if (IsJoinRoom()) return;

            if (message == "" || message == null || message == default)
            {
                // TODO Strix 空文字の場合
            }
            else
            {
                switch(curSendType)
                {
                    // ルーム内の全員に送信
                    case SendType.ALL:
                        SendMessageAllUser(CreateStrixMessageModel(message, null));
                        break;
                    
                    // TODO Strix ルーム内の特定の人にだけ送信
                    case SendType.ONE:
                        
                        break;
                }
            }
        }

        // StrixMessageModelの生成
        public StrixMessageModel CreateStrixMessageModel(
            string mainText,
            UID to
        )
        {
            StrixMessageModel model = new StrixMessageModel();
            model.Name = StrixMasterManager.Instance.playerName;
            model.MainText = mainText;
            model.Time = DateTimeUtils.GetNow();
            model.To = to;
            return model;
        }

        // メッセージ受信（一斉）のコールバックの設定
        public void SetPostMessageAllUserCallback(Action<StrixMessageModel> action)
        {
            _postMessageAllUserCallback = action;
        }

        // メッセージ受信（指定）のコールバックの設定
        public void SetPostMessageOneUserCallback(Action<StrixMessageModel> action)
        {
            _postMessageOneUserCallback = action;
        }
        
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------
        
        // メッセージ送信成功コールバック(一斉送信)
        private void OnSendMessageCallback(RoomRelayEventArgs args, StrixMessageModel message)
        {
            Debug.Log("<color=blue>メッセージ送信に成功しました。</color>:" + message.MainText);
        }
        
        // メッセージ送信失敗コールバック(一斉送信)
        private void OnSendMessageFailedCallback(FailureEventArgs args)
        {
            Debug.Log("<color=red>メッセージの送信に失敗しました。</color>:" + args.cause);
        }

        // メッセージ送信成功コールバック(指定送信)
        private void OnSendDirectMessageCallback(RoomDirectRelayEventArgs args)
        {
            Debug.Log("<color=blue>メッセージ送信に成功しました。</color>");
        }
        
        // メッセージ送信失敗コールバック(指定送信)
        private void OnSendDirectMessageFailedCallback(FailureEventArgs args)
        {
            Debug.Log("<color=red>メッセージの送信に失敗しました。</color>:" + args.cause);
        }

        // ルーム入室中かどうか
        private bool IsJoinRoom()
        {
            return !StrixRoomManager.Instance.IsConnected;
        }
        
        // ---------- protected関数 ---------
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SoftGear.Strix.Unity.Runtime;

namespace ShunLib.Strix.Room.Item.Select
{
    public class RoomSelectItem : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ルーム名")]
        [SerializeField] protected TextMeshProUGUI _roomNameText = default;
        [Header("ルームの現在人数")]
        [SerializeField] protected TextMeshProUGUI _memberCountText = default;
        [Header("ルームの最大人数")]
        [SerializeField] protected TextMeshProUGUI _memberMaxCountText = default;
        [Header("選択用ボタン")]
        [SerializeField] protected Button _button = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private RoomInfo _roomInfo = default;
        private Action<RoomInfo> _buttonAction = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;

            SetRoomNameText(_roomInfo.name);
            SetMemberCountText(_roomInfo.memberCount.ToString());
            SetMemberMaxCountText(_roomInfo.capacity.ToString());

            SetButtonEvent();
        }

        // ボタンイベントの設定
        public void SetButtonAction(Action<RoomInfo> action)
        {
            _buttonAction = action;
        }

        // ---------- Private関数 ----------

        // ルーム名テキストの設定
        private void SetRoomNameText(string text)
        {
            if (_roomNameText == default) return;

            _roomNameText.text = text;
        }

        // ルーム現在人数テキストの設定
        private void SetMemberCountText(string text)
        {
            if (_memberCountText == default) return;

            _memberCountText.text = text;
        }

        // ルーム最大人数テキストの設定
        private void SetMemberMaxCountText(string text)
        {
            if (_memberMaxCountText == default) return;

            _memberMaxCountText.text = text;
        }

        // ボタンイベントの設定
        private void SetButtonEvent()
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                _buttonAction?.Invoke(_roomInfo);
            });
        }

        // ---------- protected関数 ---------
    }
}



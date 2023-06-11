using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using SoftGear.Strix.Unity.Runtime;

using ShunLib.Strix.Const;

namespace ShunLib.Strix.Room.InfoView
{
    public class RoomInfoView : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ルーム名")]
        [SerializeField] protected TextMeshProUGUI _roomNameText = default;
        [Header("ルームの現在人数")]
        [SerializeField] protected TextMeshProUGUI _memberCountText = default;
        [Header("ルームの最大人数")]
        [SerializeField] protected TextMeshProUGUI _memberMaxCountText = default;
        [Header("ルーム画像")]
        [SerializeField] protected Image _roomImage = default;
        [Header("ルーム説明")]
        [SerializeField] protected TextMeshProUGUI _roomInfoText = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private RoomInfo _roomInfo = default;
        
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _roomInfo = null;

            SetRoomNameText("");
            SetMemberCountText("");
            SetMemberMaxCountText("");
            SetRoomInfoText("");
        }

        // データを設定して描画
        public void ShowRoomInfo(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;

            SetRoomNameText(_roomInfo.name);
            SetMemberCountText(_roomInfo.memberCount.ToString());
            SetMemberMaxCountText(_roomInfo.capacity.ToString());
            if (_roomInfo.properties.ContainsKey(StrixConst.ROOM_INFO_DETAILS_TEXT))
            {
                SetRoomInfoText(_roomInfo.properties[StrixConst.ROOM_INFO_DETAILS_TEXT].ToString());
            }
            else
            {
                SetRoomInfoText("");
            }
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

        // ルーム画像の設定
        private void SetRoomImage(Sprite sprite)
        {
            if (_roomImage == default) return;

            _roomImage.sprite = sprite;
        }

        // ルーム説明テキストの設定
        private void SetRoomInfoText(string text)
        {
            if (_roomInfoText == default) return;

            _roomInfoText.text = text;
        }

        // ---------- protected関数 ---------
    }
}



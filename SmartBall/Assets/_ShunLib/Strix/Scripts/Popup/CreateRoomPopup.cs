using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ShunLib.Btn.Common;
using ShunLib.Popup;
using ShunLib.UI.DropDown.Common;
using ShunLib.UI.Input;

using ShunLib.Strix.Const;
using ShunLib.Strix.Manager.Master;
using ShunLib.Strix.Manager.Room;
using ShunLib.Strix.Room.SelectPopup;

namespace ShunLib.Strix.Room.CreateRoom
{
    public class CreateRoomPopup : BasePopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ルーム作成ボタン")]
        [SerializeField] protected CommonButton _createButton = default;

        [Header("閉じるボタン")]
        [SerializeField] protected CommonButton _closeButton = default;

        [Header("ルーム名入力フィールド")]
        [SerializeField] protected CommonInputField _roomNameField = default;

        [Header("ルーム最大入室人数ドロップダウン")]
        [SerializeField] protected CommonDropDown _capacityDropdown = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public Action CreateRoomCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // 初期化
        protected override void Initialize()
        {
            base.Initialize();
            List<string> dropdownItems = StrixConst.CAPACITY_VALUE_LIST.ConvertAll(x => x.ToString());
            _capacityDropdown.AddItems(dropdownItems);
        }

        // ボタンイベントの設定
        protected override void SetButtonEvents()
        {
            _createButton?.SetOnEvent(() => {
                CreateRoom();
            });
            _closeButton?.SetOnEvent(Close);
        }

        // 新しいルームの作成
        protected virtual void CreateRoom()
        {
            // ルーム名
            string roomName = "";
            roomName = _roomNameField.GetInputText();
            // TODO MetaPachi 例外時の処理
            if (roomName == "") return;

            // 入室最大人数
            int capacity = 0;
            capacity = Convert.ToInt32(_capacityDropdown.GetSelectItem());

            // TODO 接続待機画面の表示
            StrixRoomManager.Instance.CreateRoom(
                capacity,
                roomName,
                StrixMasterManager.Instance.playerId,
                CreateRoomCallback
            );
            Close();
        }
    }
}


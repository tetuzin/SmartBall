using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ShunLib.Btn.Common;
using ShunLib.Popup;

using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;

using ShunLib.Strix.Manager.Master;
using ShunLib.Strix.Manager.Room;
using ShunLib.Strix.Room.Item.Select;
using ShunLib.Strix.Room.InfoView;

namespace ShunLib.Strix.Room.SelectPopup
{
    public class RoomSelectPopup : BasePopup
    {
        // ---------- 定数宣言 ----------

        public const string DECISION_BUTTON_EVENT = "decisionButtonEvent";
        public const string CREATE_BUTTON_EVENT = "createButtonEvent";

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ルーム選択フレームアイコンのプレハブ")]
        [SerializeField] private RoomSelectItem _roomSelectItemPrefab = default;

        [Header("ルームアイコン表示親オブジェクト")]
        [SerializeField] private Transform _roomListParent = default;

        [Header("ルームが見つからなかったときに表示するテキスト")]
        [SerializeField] private TextMeshProUGUI _noRoomText = default;

        [Header("ルーム詳細表示パネル")]
        [SerializeField] private RoomInfoView _roomInfoView = default;

        [Header("ルーム入室ボタン")]
        [SerializeField] private CommonButton _decisionButton = default;

        [Header("ポップアップとじるボタン")]
        [SerializeField] private CommonButton _cancelButton = default;

        [Header("ルーム作成ボタン")]
        [SerializeField] private CommonButton _createButton = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private RoomInfo _selectRoomInfo = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------

        // ルーム一覧表示
        private void ShowRoomList()
        {
            GetRoomList();
        }

        // ルーム一覧の取得
        private void GetRoomList()
        {
            StrixRoomManager.Instance.SearchRoom(UpdateRoomListView);
        }

        // ルーム一覧の描画
        private void UpdateRoomListView(RoomSearchEventArgs args)
        {
            if (args.roomInfoCollection.Count <= 0)
            {
                _selectRoomInfo = null;
                _noRoomText.gameObject.SetActive(true);
                _roomInfoView.Initialize();
            }
            else
            {
                _noRoomText.gameObject.SetActive(false);
                foreach (RoomInfo roomInfo in args.roomInfoCollection)
                {
                    // ルーム選択アイテムの生成
                    RoomSelectItem roomSelectItem = Instantiate(_roomSelectItemPrefab, _roomListParent);
                    roomSelectItem.Initialize(roomInfo);
                    roomSelectItem.SetButtonAction(OnClickRoomSelectItem);
                }
            }
        }

        // ルームアイテム選択時の処理
        private void OnClickRoomSelectItem(RoomInfo roomInfo)   
        {
            _selectRoomInfo = roomInfo;
            _roomInfoView.ShowRoomInfo(_selectRoomInfo);
            _roomInfoView.gameObject.SetActive(true);
            _decisionButton.gameObject.SetActive(true);
        }

        // ---------- protected関数 ---------

        // 初期化
        protected override void Initialize()
        {
            base.Initialize();

            _noRoomText.gameObject.SetActive(false);
            _roomInfoView.Initialize();
            _roomInfoView.gameObject.SetActive(false);
            _decisionButton.gameObject.SetActive(false);
            OpenCallback += () => {
                ShowRoomList();
            };

            _createButton.gameObject.SetActive(!StrixRoomManager.Instance.IsConnected);
        }

        // ボタンイベントの設定
        protected override void SetButtonEvents()
        {
            _createButton?.SetOnEvent(() => {
                Close();
                Action action = GetAction(CREATE_BUTTON_EVENT);
                action?.Invoke();
            });
            _decisionButton?.SetOnEvent(() => {
                if (_selectRoomInfo != null)
                {
                    Close();
                    // TODO ルームへ参加中アニメーションを表示
                    StrixRoomManager.Instance.JoinRoom(
                        StrixRoomManager.ConvertRoomInfoToRoomJoinArgs(
                            _selectRoomInfo,
                            StrixMasterManager.Instance.playerName
                        )
                    );
                }
            });
            _cancelButton?.SetOnEvent(Close);
        }
    }
}



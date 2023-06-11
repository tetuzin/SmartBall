using System;
using System.Collections.Generic;
using UnityEngine;

using PlayFab.ClientModels;

using ShunLib.Manager.Game;
using ShunLib.Popup;
using ShunLib.Btn.Common;
using ShunLib.Model.Room;

namespace ShunLib.Room
{
    public class InteriorSelectPopup : BasePopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("インテリアアイテムプレハブ")]
        [SerializeField] protected InteriorItem _interiorItemPrefab = default;

        [Header("とじるボタン")]
        [SerializeField] private CommonButton _cancelButton = default;

        [Header("インテリアアイテム表示親オブジェクト")]
        [SerializeField] private Transform _parent = default;

        [Header("インテリア未所持テキスト")]
        [SerializeField] private GameObject _nonInteriorText = default;

        // TODO インテリア一覧データリスト
        public RoomInteriorScriptableObject roomInteriorScriptableObject = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        public Action<BaseRoomInterior> OnClickInteriorItemCallback { get; set; }
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        private int _selectItemIndex = default;
        private List<ItemInstance> _interiorList = default;
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // 初期化
        protected override void Initialize()
        {
            base.Initialize();

            _selectItemIndex = 0;

            // 所持アイテムの取得
            _interiorList = GameManager.Instance.dataManager.Data.User.UserInventory;

            _nonInteriorText.SetActive(_interiorList.Count <= 0);

            // 所持アイテムを生成して描画
            foreach (ItemInstance itemInstance in _interiorList)
            {
                InteriorItem item = Instantiate(_interiorItemPrefab, _parent);
                int interiorId = Int32.Parse(itemInstance.ItemClass);
                BaseRoomInterior interior = roomInteriorScriptableObject.roomInteriorTable.GetValue(interiorId);
                int placeCount = 0;
                foreach (BaseRoomInteriorModel interiorModel in GameManager.Instance.dataManager.Data.Game.RoomInteriorList.List)
                {
                    if (interiorModel.InteriorId == interiorId)
                    {
                        placeCount++;
                    }
                }
                bool isGrayOut = itemInstance.RemainingUses <= placeCount;
                item.Initialize(
                    interior.GetInteriorName(), (itemInstance.RemainingUses - placeCount).ToString(),
                    itemInstance.RemainingUses.ToString(), interior.GetInteriorSprite(), isGrayOut
                );
                if (!isGrayOut)
                {
                    item.OnClickCallback = () => {
                        OnClickInteriorItemCallback?.Invoke(interior);
                        Close();
                    };
                }
            }
        }

        // ボタンイベントの設定
        protected override void SetButtonEvents()
        {
            _cancelButton?.SetOnEvent(Close);
        }

        // ---------- デバッグ用関数 ---------
    }
}



using UnityEngine;

using ShunLib.UI.Panel;

using Pachinko.Slot.Controller;

namespace Pachinko.DataCount.Panel
{
    public class DataCountPanel : CommonPanel
    {
        // ---------- 定数宣言 ----------

        // 保留の数(通常時)
        private const string NORMAL_HOLD_COUNT = "normal_hold_count";
        // 保留の数(フィーバー時)
        private const string FEVER_HOLD_COUNT = "fever_hold_count";

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("サブのスロット")]
        [SerializeField] private SlotViewController _slotView = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            _slotView?.Initialize();
            _slotView?.InitializeReels(new int[]{0, 0, 0});
            SetNormalHoldText("0");
            SetFeverHoldText("0");
            Show();
        }

        // 出目の即時表示
        public void SlotViewShowValue(int[] values)
        {
            _slotView?.ShowSlotDesigns(values);
        }

        // SlotViewの回転
        public void SlotViewRotate()
        {
            _slotView?.Rotate();
        }

        // SlotViewの値指定
        public void SlotViewSetValue(int[] values)
        {
            _slotView?.SetValue(values);
        }

        // SlotViewの停止
        public void SlotViewStop()
        {
            _slotView?.StopAll(null, true);
        }

        // サブスロットの表示・非表示
        public void SetActiveSlotView(bool b)
        {
            _slotView?.gameObject.SetActive(b);
        }

        // 保留カウンターテキストの値設定(通常時)
        public void SetNormalHoldText(string value)
        {
            SetText(NORMAL_HOLD_COUNT, value);
        }

        // 保留カウンターテキストの値設定(フィーバー時)
        public void SetFeverHoldText(string value)
        {
            SetText(FEVER_HOLD_COUNT, value);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



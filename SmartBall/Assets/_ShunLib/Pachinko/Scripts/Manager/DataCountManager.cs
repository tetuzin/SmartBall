using UnityEngine;

using Pachinko.DataCount.Panel;

namespace Pachinko.DataCount.Manager
{
    public class DataCountManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("データカウントパネル")] 
        [SerializeField] protected DataCountPanel _panel = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public virtual void Initialize()
        {
            _panel.Initialize();
            Show();
        }

        // 各パラメータの設定
        public void SetParameter(DataCountPanel panel = null)
        {
            _panel = panel;
        }   

        // 表示
        public void Show()
        {
            _panel.Show();
        }

        // 非表示
        public void Hide()
        {
            _panel.Hide();
        }

        // 出目の即時表示
        public void SlotViewShowValue(int[] values)
        {
            _panel.SlotViewShowValue(values);
        }

        // SlotViewの回転
        public void SlotViewRotate()
        {
            _panel.SlotViewRotate();
        }

        // SlotViewの値指定
        public void SlotViewSetValue(int[] values)
        {
            _panel.SlotViewSetValue(values);
        }

        // SlotViewの停止
        public void SlotViewStop()
        {
            _panel.SlotViewStop();
        }

        // 通常時保留の数テキストの設定
        public void SetNormalHoldCount(string value)
        {
            _panel.SetNormalHoldText(value);
        }

        // フィーバー時保留の数テキストの設定
        public void SetFeverHoldCount(string value)
        {
            _panel.SetFeverHoldText(value);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



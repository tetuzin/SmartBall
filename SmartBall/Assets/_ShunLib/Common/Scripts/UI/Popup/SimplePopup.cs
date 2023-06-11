using UnityEngine;
using TMPro;

using ShunLib.Btn.Common;

namespace ShunLib.Popup.Simple
{
    public class SimplePopup : BasePopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("キャンセルボタン")] protected CommonButton _cancelButton = default;
        [SerializeField, Tooltip("キャンセルボタンテキスト")] protected TextMeshProUGUI _cancelText;
        [SerializeField, Tooltip("メインテキスト")] protected TextMeshProUGUI _mainText;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // メイン文言の設定
        public void SetMainText(string text)
        {
            if (_mainText == default) return;

            _mainText.text = text;
        }

        // キャンセルボタン文言の設定
        public void SetCancelText(string text)
        {
            if (_cancelText == default) return;
            
            _cancelText.text = text;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // ボタンイベントの設定
        protected override void SetButtonEvents()
        {   
            _cancelButton?.SetOnEvent(Close);
        }

        // ---------- デバッグ用関数 ---------
    }
}


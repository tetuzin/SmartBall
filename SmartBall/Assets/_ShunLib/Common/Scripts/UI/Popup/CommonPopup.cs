using System;
using UnityEngine;
using TMPro;

using ShunLib.Btn.Common;

namespace ShunLib.Popup.Common
{
    public class CommonPopup : BasePopup
    {
        // ---------- 定数宣言 ----------

        public const string DECISION_BUTTON_EVENT = "decisionButtonEvent";
        public const string CANCEL_BUTTON_EVENT = "cancelButtonEvent";

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("決定ボタン")] protected CommonButton _decisionButton = default;
        [SerializeField, Tooltip("キャンセルボタン")] protected CommonButton _cancelButton = default;
        [SerializeField, Tooltip("タイトルテキスト")] protected TextMeshProUGUI _titleText;
        [SerializeField, Tooltip("メインテキスト")] protected TextMeshProUGUI _mainText;
        [SerializeField, Tooltip("決定ボタンテキスト")] protected TextMeshProUGUI _decisionText;
        [SerializeField, Tooltip("キャンセルボタンテキスト")] protected TextMeshProUGUI _cancelText;
        
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // タイトル文言の設定
        public void SetTitleText(string text)
        {
            if (_titleText == default) return;

            _titleText.text = text;
        }

        // メイン文言の設定
        public void SetMainText(string text)
        {
            if (_mainText == default) return;

            _mainText.text = text;
        }

        // 決定ボタン文言の設定
        public void SetDecisionText(string text)
        {
            if (_decisionText == default) return;

            _decisionText.text = text;
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
            if (_decisionButton != default)
            {
                _decisionButton.SetOnEvent(() => {
                    Action action = GetAction(DECISION_BUTTON_EVENT);
                    action?.Invoke();
                    Close();
                });
            }
            
            if (_cancelButton != default)
            {
                _cancelButton.SetOnEvent(() => {
                    Action action = GetAction(CANCEL_BUTTON_EVENT);
                    action?.Invoke();
                    Close();
                });
            }
        }
    }
}



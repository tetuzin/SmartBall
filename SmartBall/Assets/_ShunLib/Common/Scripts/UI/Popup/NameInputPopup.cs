using System;
using UnityEngine;

using ShunLib.UI.Input;
using ShunLib.Btn.Common;

namespace ShunLib.Popup.NameInput
{
    public class NameInputPopup : BasePopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("名前入力フォーム")]
        [SerializeField] protected CommonInputField _nameInputField;

        [Header("OKボタン")]
        [SerializeField] protected CommonButton _okBtn;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        public Action<string> NameInputCompleteCallback { get; set; }
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------

        // OKボタン押下時の処理
        private void OnClickOkBtn()
        {
            string name = _nameInputField.GetInputText();
            if (name == "")
            {
                // TODO 名前が空文字のとき
                return;
            }
            NameInputCompleteCallback?.Invoke(name);
        }

        // ---------- protected関数 ---------

        // 初期化
        protected override void Initialize()
        {
            base.Initialize();

            _nameInputField.Initialize();
        }

        // ボタンイベントの設定
        protected override void SetButtonEvents()
        {
            base.SetButtonEvents();

            _okBtn?.SetOnActive(true);
            _okBtn?.SetOnEvent(OnClickOkBtn);
        }
    }
}


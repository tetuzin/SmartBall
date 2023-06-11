using System;
using UnityEngine;
using TMPro;

namespace ShunLib.Popup.Simple
{
    public class SimpleTextPopup : BasePopup
    {
        // ---------- 定数宣言 ----------
        
        public const string CALLBACK_EVENT = "CallBackEvent";
        
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("メインテキスト")] protected TextMeshProUGUI _mainText = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        
        [SerializeField, Tooltip("表示する時間")] protected float delayTime = 1.5f;
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        protected float _curTime = default;
        protected bool _isShow = default;

        // ---------- Unity組込関数 ----------

        void FixedUpdate()
        {
            if (_isShow)
            {
                if (delayTime > 0f)
                {
                    _curTime += Time.deltaTime;

                    if (_curTime >= delayTime)
                    {
                        Hide();
                    }
                }
            }
        }

        // ---------- Public関数 ----------

        public virtual void Show()
        {
            _isShow = true;
            _curTime = 0.0f;
        }

        public virtual void Hide()
        {
            _isShow = false;
            _curTime = 0.0f;
            Close();
            Action action = GetAction(CALLBACK_EVENT);
            action();
        }

        // 表示する時間の設定
        public void SetShowTime(float time = 0f)
        {
            delayTime = time;
        }

        // メイン文言の設定
        public void SetMainText(string text)
        {
            if (_mainText == default) return;

            _mainText.text = text;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        
        // ポップアップを開くときの処理
        protected override void ShowPopup(bool isModal)
        {
            base.ShowPopup(isModal);
            Show();
        }
    }
}


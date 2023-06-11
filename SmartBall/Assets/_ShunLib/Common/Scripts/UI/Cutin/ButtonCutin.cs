using System;
using UnityEngine;

using ShunLib.UI.Slider;
using ShunLib.Controller.InputKey;

namespace ShunLib.UI.Cutin.Button
{
    public class ButtonCutin : BaseCutin
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("時間経過ゲージ")]
        [SerializeField] private CommonSlider _slider = default;

        [Header("ボタン押下時に非表示にする")]
        [SerializeField] private bool _isHideOnClick = true;

        [Header("タイミングよく押した際に演出を表示する")]
        [SerializeField] private bool _isJustTiming = true;

        [Header("タイミング")]
        [SerializeField] private float _justTimingTime = 0.5f;

        [Header("ボタン押下キー")]
        [SerializeField] private KeyCode _keyCode = KeyCode.Space;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public Action OnClickCallback { get; set; }
        public Action JustTimingCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private float _progressTime = default;
        private float _progressRate = default;
        private InputKeyController _keyController = default;

        // ---------- Unity組込関数 ----------

        void Update()
        {
            if (_isShow)
            {
                // 時間計測
                _progressTime += Time.deltaTime;
                _progressRate = _progressTime / _showTime;
                _slider.SetCurSlider(1.0f - _progressRate);
                if (_progressTime >= _showTime)
                {
                    Hide();
                }
            }

            if (_keyController == default)
            {
                // キー操作受付
                if (UnityEngine.Input.GetKeyDown(_keyCode))
                {
                    OnClick();
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            _slider.Initialize(1.0f, false);
            _slider.SetActiveGroupUI(false);
        }

        // キーコントローラの設定
        public void SetKeyController(InputKeyController keyController)
        {
            _keyController = keyController;
        }

        // 表示
        public void ShowButtonCutin(Action callback = null)
        {
            if (!_isShow)
            {
                _isShow = true;
                ShowAnimation(callback);
                _slider.SetCurSlider(1.0f);
                _slider.SetActive(true);
            }
        }

        // 処理実行
        public void OnClick()
        {
            if (!_isShow) return;

            if (_isJustTiming)
            {
                if (_progressRate <= _justTimingTime + 0.08 && _progressRate >= _justTimingTime - 0.08)
                {
                    JustTimingAction();
                }
            }

            // TODO MetaPachi 押下時のアニメーションORエフェクトの実装

            OnClickCallback?.Invoke();
            if (_isHideOnClick) Hide();
        }

        // ---------- Private関数 ----------

        // タイミングよくボタンを押した際の演出
        private void JustTimingAction()
        {
            if (!_isJustTiming) return;
            JustTimingCallback?.Invoke();
        }

        // ---------- protected関数 ---------
    }
}
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using ShunLib.Toggle.Common;

namespace ShunLib.Toggle.Slide
{
    public class SlideToggle : CommonToggle
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("スライダー")]
        [SerializeField] protected Slider _slider = default;
        [Header("切替方法")]
        [SerializeField] protected bool _isSmoothChange = default;
        [SerializeField] protected float _smoothChangeTime = 0.5f;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool _isChange = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            _isChange = false;
        }

        // オンオフ切替
        public override void SwitchToggle()
        {
            if (_isChange) return;

            _isOn = !_isOn;
            if (_isOn) 
            {
                ChangeAnim(1f);
                _isOnAction?.Invoke();
            }
            else
            {
                ChangeAnim(0f);
                _isOffAction?.Invoke();
            }
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // 切替アニメーション
        protected virtual void ChangeAnim(float value)
        {
            if (_isSmoothChange)
            {
                _isChange = true;
                DOTween.To(
                    () => _slider.value,
                    val => _slider.value = val,
                    value,
                    _smoothChangeTime
                ).OnComplete(() => {
                    _isChange = false;
                });
            }
            else
            {
                _slider.value = value;
            }
        }
    }
}
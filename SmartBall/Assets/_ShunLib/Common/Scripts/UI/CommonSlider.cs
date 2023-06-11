using System;
using UnityEngine;
using TMPro;

namespace ShunLib.UI.Slider
{
    public class CommonSlider : UnityEngine.UI.Slider
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("値テキストグループ")] public CanvasGroup textGroup = default;
        [SerializeField, Tooltip("値テキスト（現在値）")] public TextMeshProUGUI curText = default;
        [SerializeField, Tooltip("値テキスト（最大値）")] public TextMeshProUGUI maxText = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // HP現在値
        private float _curValue = default;
        // HP最大値
        private float _maxValue = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(float value, bool isShow)
        {
            SetMaxSlider(value);
            SetCurSlider(value);
            SetActive(isShow);
        }

        // 表示・非表示
        public void SetActive(bool b)
        {
            gameObject.SetActive(b);
        }

        // 値テキストグループの表示・非表示
        public void SetActiveGroupUI(bool b)
        {
            textGroup.alpha = b ? 1 : 0;
        }

        // 現在の値を設定する
        public void SetCurValue(float value)
        {
            _curValue = value;
            curText.text = value.ToString();
        }

        // 最大値を設定する
        public void SetMaxValue(float value)
        {
            _maxValue = value;
            maxText.text = value.ToString();
        }

        // スライダーの現在値を設定する
        public void SetCurSlider(float value)
        {
            this.value = value;
            SetCurValue(value);
        }

        // スライダーの最大値を設定する
        public void SetMaxSlider(float value)
        {
            this.maxValue = value;
            SetMaxValue(value);
        }

        // スライダーが動いた時のコールバック設定
        public void SetOnEvent(Action<float> callback)
        {
            this.onValueChanged.RemoveAllListeners();
            this.onValueChanged.AddListener((value) => {
                callback?.Invoke(value);
            });
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



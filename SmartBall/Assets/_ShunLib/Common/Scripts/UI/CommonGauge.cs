using UnityEngine;
using TMPro;

namespace ShunLib.UI.Gauge
{
    public class CommonGauge : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("用Slider")] private UnityEngine.UI.Slider _slider = default;
        [SerializeField, Tooltip("値テキストグループ")] private CanvasGroup _textGroup = default;
        [SerializeField, Tooltip("値テキスト（現在値）")] private TextMeshProUGUI _curText = default;
        [SerializeField, Tooltip("値テキスト（最大値）")] private TextMeshProUGUI _maxText = default;

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
            _textGroup.alpha = b ? 1 : 0;
        }

        // 現在の値を設定する
        public void SetCurValue(float value)
        {
            _curValue = value;
            _curText.text = value.ToString();
        }

        // 最大値を設定する
        public void SetMaxValue(float value)
        {
            _maxValue = value;
            _maxText.text = value.ToString();
        }

        // スライダーの現在値を設定する
        public void SetCurSlider(float value)
        {
            _slider.value = value;
            SetCurValue(value);
        }

        // スライダーの最大値を設定する
        public void SetMaxSlider(float value)
        {
            _slider.maxValue = value;
            SetMaxValue(value);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



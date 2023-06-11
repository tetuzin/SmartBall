using System;
using UnityEngine;
using TMPro;

using ShunLib.Btn.Common;

namespace ShunLib.Btn.Arrow
{
    public class ArrowButton : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("Leftボタン")] protected CommonButton _leftButton = default;
        [SerializeField, Tooltip("Rightボタン")] protected CommonButton _rightButton = default;
        [SerializeField, Tooltip("Leftテキスト")] protected TextMeshProUGUI _leftText = default;
        [SerializeField, Tooltip("Rightテキスト")] protected TextMeshProUGUI _rightText = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // Leftボタンに処理を設定
        public void SetLeftButtonEvent(Action action)
        {
            if (_leftButton == default) return;

            _leftButton.RemoveOnEvent();
            _leftButton.SetOnEvent(action);
        }

        // Rightボタンに処理を設定
        public void SetRightButtonEvent(Action action)
        {
            if (_rightButton == default) return;

            _rightButton.RemoveOnEvent();
            _rightButton.SetOnEvent(action);
        }

        // Leftテキストにテキストを設定
        public void SetLeftText(string text)
        {
            if (_leftText == default) return;

            _leftText.text = text;
        }

        // Rightテキストにテキストを設定
        public void SetRightText(string text)
        {
            if (_rightText == default) return;

            _rightText.text = text;
        }

        // Leftボタンの表示・非表示
        public void SetActiveLeftButton(bool b)
        {
            if (_leftButton == default) return;

            _leftButton.gameObject.SetActive(b);
        }

        // Rightボタンの表示・非表示
        public void SetActiveRightButton(bool b)
        {
            if (_rightButton == default) return;

            _rightButton.gameObject.SetActive(b);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



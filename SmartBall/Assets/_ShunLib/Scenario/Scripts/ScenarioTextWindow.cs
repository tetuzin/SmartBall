using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace ShunLib.Scenario.TextWindow
{
    public class ScenarioTextWindow : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("テキストウィンドウ")]
        [SerializeField, Tooltip("キャンバスグループ")] protected CanvasGroup _basewindoGroup = default;
        [SerializeField, Tooltip("ウィンドウ画像")] protected Image _basewindowImage = default;
        [SerializeField, Tooltip("メインテキスト")] protected TextMeshProUGUI _mainText = default;
        [SerializeField, Tooltip("ページ送りアイコン")] protected GameObject _nextIcon = default;
        [Header("名前ウィンドウ")]
        [SerializeField, Tooltip("キャンバスグループ")] protected CanvasGroup _namewindoGroup = default;
        [SerializeField, Tooltip("ウィンドウ画像")] protected Image _namewindowImage = default;
        [SerializeField, Tooltip("名前テキスト")] protected TextMeshProUGUI _nameText = default;
        [Header("演出")]
        [SerializeField, Tooltip("テキストを一文字ずつ表示する")] protected bool _isSlowly = true;
        [SerializeField, Tooltip("テキストを表示する速度")] protected float _speed = 0.05f;
        [SerializeField, Tooltip("ウィンドウをフェードさせる")] protected bool _isFade = true;
        [SerializeField, Tooltip("フェード時間")] protected float _fadeTime = 0.5f;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private string _curText = default;
        private int _curCharNum = default;
        private bool _isShowEnd = default;
        private float _progressTime = default;

        // ---------- Unity組込関数 ----------

        void FixedUpdate()
        {
            if (_isSlowly && !_isShowEnd)
            {
                _progressTime += Time.deltaTime;
                if (_progressTime >= _speed)
                {
                    _mainText.text += _curText[_curCharNum];
                    _curCharNum++;
                    _progressTime = 0f;
                    if (_curCharNum >= _curText.Length)
                    {
                        _isShowEnd = true;
                    }
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            SetMainText("");
            SetNameText("");
            SetActiveWindow(false);
        }

        // メインテキストの設定
        public void SetMainText(string text)
        {
            if (_isSlowly && text != "")
            {
                _mainText.text = "";
                _curText = text;
                _isShowEnd = false;
                _curCharNum = 0;
                _progressTime = 0f;
            }
            else
            {
                _mainText.text = text;
                _isShowEnd = true;
            }
        }

        // メインテキストのカラーを設定
        public void SetMainTextColor(Color color)
        {
            _mainText.color = color;
        }

        // 名前テキストの設定
        public void SetNameText(string text)
        {
            _nameText.text = text;
        }

        // 名前テキストのカラーを設定
        public void SetNameTextColor(Color color)
        {
            _nameText.color = color;
        }

        // アイコンの表示・非表示
        public void SetActiveNextIcon(bool b)
        {
            _nextIcon.SetActive(b);
        }

        // テキストウィンドウを表示・非表示
        public void SetActiveBaseWindow(bool b, Action callback = null)
        {
            if (_isFade)
            {
                _basewindoGroup.DOFade(
                    endValue: b ? 1f : 0f,
                    duration: _fadeTime
                ).OnComplete(() => {
                    callback?.Invoke();
                });
            }
            else
            {
                _basewindowImage.gameObject.SetActive(b);
                callback?.Invoke();
            }
        }

        // 名前ウィンドウを表示・非表示
        public void SetActiveNameWindow(bool b, Action callback = null)
        {
            if (_isFade)
            {
                _namewindoGroup.DOFade(
                    endValue: b ? 1f : 0f,
                    duration: _fadeTime
                ).OnComplete(() => {
                    callback?.Invoke();
                });
            }
            else
            {
                _namewindowImage.gameObject.SetActive(b);
                callback?.Invoke();
            }
        }

        // ウィンドウを表示・非表示
        public void SetActiveWindow(bool b)
        {
            SetActiveBaseWindow(b);
            SetActiveNameWindow(b);
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}
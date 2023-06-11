using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using ShunLib.Btn.Common;

namespace ShunLib.UI.ListItem
{
    public class CommonListItem : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        
        [SerializeField, Tooltip("アイコン画像")] private Image _image = default;
        [SerializeField, Tooltip("グレーアウトオブジェクト")] private GameObject _grayOutObj = default;
        [SerializeField, Tooltip("ベースボタン")] private Button _baseButton = default;
        [SerializeField, Tooltip("ボタン1")] private CommonButton _commonBtn1 = default;
        [SerializeField, Tooltip("ボタン2")] private CommonButton _commonBtn2 = default;
        [SerializeField, Tooltip("ボタン1テキスト")] private TextMeshProUGUI _btn1Text;
        [SerializeField, Tooltip("ボタン2テキスト")] private TextMeshProUGUI _btn2Text;
        [SerializeField, Tooltip("メインテキスト")] private TextMeshProUGUI _mainText;
        
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // リストアイテムが選択されたときのイベント
        private Action _itemSelectEvent = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        
        // 初期化
        public void Initialize(bool isSelectBtn = false)
        {
            _itemSelectEvent = () => { };
            
            if (_grayOutObj != default) _grayOutObj.SetActive(false);
            if (_commonBtn1 != default) _commonBtn1.gameObject.SetActive(false);
            if (_commonBtn2 != default) _commonBtn2.gameObject.SetActive(false);
            if (_btn1Text != default) _btn1Text.gameObject.SetActive(false);
            if (_btn2Text != default) _btn2Text.gameObject.SetActive(false);
            if (_image != default) _image.gameObject.SetActive(false);
            
            if (isSelectBtn)
            {
                _baseButton.onClick.RemoveAllListeners();
                _baseButton.onClick.AddListener(() =>
                {
                    _itemSelectEvent?.Invoke();
                });
            }
            else
            {
                _baseButton.enabled = false;
            }
        }
        
        // メインテキストの設定
        public void SetMainText(string text)
        {
            if (_mainText != default)
            {
                _mainText.text = text;
            }
        }
        
        // ボタン1テキストの設定
        public void SetBtn1Text(string text)
        {
            if (_btn1Text != default)
            {
                _btn1Text.gameObject.SetActive(true);
                _btn1Text.text = text;
            }
        }
        
        // ボタン2テキストの設定
        public void SetBtn2Text(string text)
        {
            if (_btn2Text != default)
            {
                _btn2Text.gameObject.SetActive(true);
                _btn2Text.text = text;
            }
        }
        
        // アイコン画像の設定
        public void SetIcon(Sprite sprite)
        {
            if (_image != default)
            {
                _image.gameObject.SetActive(true);
                _image.sprite = sprite;
            }
        }
        
        // ボタン1のイベント設定
        public void SetButton1Event(Action action)
        {
            if (_commonBtn1 != default)
            {
                _commonBtn1.gameObject.SetActive(true);
                _commonBtn1.SetOnEvent(() =>
                {
                    action();
                });
            }
        }
        
        // ボタン2のイベント設定
        public void SetButton2Event(Action action)
        {
            if (_commonBtn2 != default)
            {
                _commonBtn2.gameObject.SetActive(true);
                _commonBtn2.SetOnEvent(() =>
                {
                    action();
                });
            }
        }
        
        // 選択時イベントの設定
        public void SetItemSelectEvent(Action action)
        {
            _itemSelectEvent = action;
        }
        
        // グレーアウトの設定
        public void SetGrayOut(bool isGrayOut)
        {
            _grayOutObj.SetActive(isGrayOut);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



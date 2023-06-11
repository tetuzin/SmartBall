using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ShunLib.Room
{
    public class InteriorItem : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("インテリア名")]
        [SerializeField] protected TextMeshProUGUI _NameText = default;
        [Header("配置できる数")]
        [SerializeField] protected TextMeshProUGUI _placeCountText = default;
        [Header("所持数")]
        [SerializeField] protected TextMeshProUGUI _stockCountText = default;
        [Header("インテリアイメージ")]
        [SerializeField] protected Image _interiorImage = default;
        [Header("グレーアウト画像")]
        [SerializeField] protected Image _grayout = default;
        [Header("選択用ボタン")]
        [SerializeField] protected Button _button = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        public Action OnClickCallback { get; set; }
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(string name, string place, string stock, Sprite sprite, bool isGrayOut = false)
        {
            SetNameText(name);
            SetPlaceCountText(place);
            SetStockCountText(stock);
            SetInteriorImage(sprite);
            SetButtonEvent();

            _grayout.gameObject.SetActive(isGrayOut);
        }

        // ---------- Private関数 ----------

        // インテリア名テキストの設定
        private void SetNameText(string text)
        {
            if (_NameText == default) return;

            _NameText.text = text;
        }

        // 配置済みテキストの設定
        private void SetPlaceCountText(string text)
        {
            if (_placeCountText == default) return;

            _placeCountText.text = text;
        }

        // インテリア名テキストの設定
        private void SetStockCountText(string text)
        {
            if (_stockCountText == default) return;

            _stockCountText.text = text;
        }

        // インテリアサムネイルの設定
        private void SetInteriorImage(Sprite sprite)
        {
            _interiorImage.sprite = sprite;
        }

        // ボタンイベントの設定
        private void SetButtonEvent()
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => {
                OnClickCallback?.Invoke();
            });
        }

        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


using System;
using UnityEngine;

using ShunLib.Btn.Common;
using ShunLib.UI.PageIcon;

namespace ShunLib.Page.Slide
{
    public class SlidePage : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("閉じるボタン")] protected CommonButton _closeButton = default;
        [SerializeField, Tooltip("ページアイコン")] protected Indicator _indicator = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private int _pageCount = default;
        private int _thisPage = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(int pageCount, int thisPage)
        {
            _pageCount = pageCount;
            _thisPage = thisPage;
            _indicator.Initialize(pageCount, thisPage);
        }

        // 閉じるボタンの処理設定
        public void SetCloseButtonEvent(Action action)
        {
            _closeButton.SetOnEvent(action);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



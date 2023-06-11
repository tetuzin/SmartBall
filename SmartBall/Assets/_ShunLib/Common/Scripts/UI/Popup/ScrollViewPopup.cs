using UnityEngine;

using ShunLib.Popup.Common;
using ShunLib.UI.Scroll;

namespace ShunLib.Popup.ScrollView
{
    public class ScrollViewPopup : CommonPopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        
        [SerializeField, Tooltip("スクロールビュー")] protected CommonScrollRect _scrollView;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ---------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        
        // スクロールビューに子要素の追加
        public virtual void SetContent(GameObject obj)
        {
            _scrollView.AddContent(obj);
        }

        // 縦スクロールの設定
        public void SetVerticalScroll(bool b)
        {
            _scrollView.SetVerticalScroll(b);
        }

        // 横スクロールの設定
        public void SetHorizontalScroll(bool b)
        {
            _scrollView.SetHorizontalScroll(b);
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        
        // 初期化
        protected override void Initialize()
        {
            _scrollView.Initialize();
        }
    }
}


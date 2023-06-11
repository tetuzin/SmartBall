using System.Collections.Generic;
using UnityEngine;

using ShunLib.Btn.Arrow;
using ShunLib.Page.Slide;
using ShunLib.UI.Scroll;

namespace ShunLib.Popup.Slide
{
    public class SlidePagePopup : BasePopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("ScrollRect")] protected CommonScrollRect _scrollRect = default;
        [SerializeField, Tooltip("Arrowボタン")] protected ArrowButton _arrowButton = default;
        [SerializeField, Tooltip("ページPrefab")] protected List<SlidePage> _slidePages = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private int _curPage = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------

        // ArrowButtonの表示設定
        private void SetActiveArrowButton(int curPage)
        {
            if (curPage == 0)
            {
                _arrowButton.SetActiveLeftButton(false);
                _arrowButton.SetActiveRightButton(true);
            }
            else if(curPage == _slidePages.Count - 1)
            {
                _arrowButton.SetActiveLeftButton(true);
                _arrowButton.SetActiveRightButton(false);
            }
            else
            {
                _arrowButton.SetActiveLeftButton(true);
                _arrowButton.SetActiveRightButton(true);
            }
        }

        // ---------- protected関数 ---------

        // 初期化
        protected override void Initialize()
        {
            for (int i = 0; i < _slidePages.Count; i++)
            {
                _slidePages[i].Initialize(_slidePages.Count, i);
                _slidePages[i].SetCloseButtonEvent(() => {
                    Close();
                });
            }

            _scrollRect.Initialize();
            List<Vector2> posList = new List<Vector2>();
            Vector2 pos = _scrollRect.content.anchoredPosition;
            posList.Add(pos);
            for (int i = 1; i < _slidePages.Count; i++)
            {
                pos = new Vector2(pos.x - _scrollRect.viewport.sizeDelta.x, pos.y);
                posList.Add(pos);
            }
            _scrollRect.InitializePageScroll(posList);
            _curPage = 0;
            _arrowButton.SetLeftButtonEvent(ScrollPrevPage);
            _arrowButton.SetRightButtonEvent(ScrollNextPage);
            SetActiveArrowButton(_curPage);
        }

        // 次のページへスクロール
        protected virtual void ScrollNextPage()
        {
            if (_scrollRect.IsMove) return;

            _curPage++;
            SetActiveArrowButton(_curPage);
            _scrollRect.MovePage(_curPage);
        }

        // 前のページへスクロール
        protected virtual void ScrollPrevPage()
        {
            if (_scrollRect.IsMove) return;

            _curPage--;
            SetActiveArrowButton(_curPage);
            _scrollRect.MovePage(_curPage);
        }
    }
}



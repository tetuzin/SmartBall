using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Manager.UI;

namespace ShunLib.Manager.ScenarioUI
{
    public class ScenarioCanvasConst
    {
        public const string MAIN = "main";
    }
    
    public class ScenarioImageConst
    {
        public const string BACK_GROUND = "back_ground";
    }

    public class ScenarioButtonConst
    {
        public const string WINDOW = "window";  // 画面タッチボタン
        public const string MENU = "menu";      // メニューボタン
        public const string LOG = "log";        // ログ表示ボタン
        public const string HIDE = "hide";      // テキスト非表示ボタン
        public const string SKIP = "skip";      // スキップボタン
        public const string FAST = "fast";      // 早送りボタン
        public const string AUTO = "auto";      // オート再生ボタン
    }

    public class ScenarioUIManager : UIManager
    {
        // 初期化
        public override void Initialize()
        {
            base.Initialize();

            SetBackgroundSize();
        }
    
        // 背景画像のサイズ設定
        private void SetBackgroundSize()
        {
            if (!_canvasList.IsValue(ScenarioCanvasConst.MAIN)) return;
            if (!_imageList.IsValue(ScenarioImageConst.BACK_GROUND)) return;

            RectTransform rectTransform = _canvasList.GetValue(ScenarioCanvasConst.MAIN).GetComponent<RectTransform>();
            Vector2 rectTransSize = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
            _imageList.GetValue(ScenarioImageConst.BACK_GROUND).GetComponent<RectTransform>().sizeDelta = rectTransSize;
        }
    }
}


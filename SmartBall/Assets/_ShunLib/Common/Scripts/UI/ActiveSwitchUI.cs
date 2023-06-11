using UnityEngine;

namespace ShunLib.UI
{
    public class ActiveSwitchUI : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("メインパネル")] 
        [SerializeField] private CanvasGroup _mainPanel = default;

        [Header("グレーアウト")] 
        [SerializeField] private CanvasGroup _grayout = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            SetActiveMainPanel(true);
            SetActiveGrayout(true);
        }

        // メインパネル表示・非表示
        public void SetActiveMainPanel(bool b)
        {
            _mainPanel.alpha = b ? 1 : 0;
            _mainPanel.interactable = b;
            _mainPanel.blocksRaycasts = b;
        }

        // グレーアウト表示・非表示
        public void SetActiveGrayout(bool b)
        {
            _grayout.alpha = b ? 1 : 0;
            _grayout.interactable = b;
            _grayout.blocksRaycasts = b;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



using ShunLib.Manager.UI;

namespace ShunLib.UI.Panel
{
    public class CommonPanel : UIManager
    {
        // ---------- 定数宣言 ----------

        // パネルのキャンバスグループ
        private const string PANEL_CANVAS_GROUP = "panel_canvas_group";

        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            Hide();
        }

        // パネルの表示
        public virtual void Show()
        {
            SetCanvasGroupActive(PANEL_CANVAS_GROUP, true);
        }

        // パネルの非表示
        public virtual void Hide()
        {
            SetCanvasGroupActive(PANEL_CANVAS_GROUP, false);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



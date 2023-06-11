using ShunLib.UI.Panel;

namespace Pachinko.Result.Panel
{
    public class ResultPanel : CommonPanel
    {
        // ---------- 定数宣言 ----------

        // リザルトフィーバー数
        private const string TEXT_RESULT_FEVER_COUNT = "text_result_fever_count";
        // リザルト獲得合計ポイント
        private const string TEXT_RESULT_TOTAL_POINT = "text_result_total_point";

        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // リザルトフィーバー数テキストの設定
        public void SetFeverCount(string text)
        {
            SetText(TEXT_RESULT_FEVER_COUNT, text);
        }

        // リザルト獲得合計ポイントテキストの設定
        public void SetTotalPoint(string text)
        {
            SetText(TEXT_RESULT_TOTAL_POINT, text);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



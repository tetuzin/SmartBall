using ShunLib.UI.Panel;

namespace Pachinko.Round.Panel
{
    public class RoundPanel : CommonPanel
    {
        // ---------- 定数宣言 ----------

        // フィーバー数
        private const string TEXT_FEVER_COUNT = "text_fever_count";
        // 現在のラウンド数
        private const string TEXT_CUR_ROUND_COUNT = "text_cur_round_count";
        // ラウンドの最大値
        private const string TEXT_MAX_ROUND_COUNT = "text_max_round_count";
        // 合計ポイント値
        private const string TEXT_TOTAL_POINT = "text_total_point";
        // 現在のポイント値
        private const string TEXT_CUR_POINT = "text_cur_point";
        // ポイント値の最大値
        private const string TEXT_MAX_POINT = "text_max_point";

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
        }

        // フィーバー数のテキスト設定
        public void SetFeverCount(string text)
        {
            SetText(TEXT_FEVER_COUNT, text);
        }

        // 現在のラウンド数のテキスト設定
        public void SetCurRoundCount(string text)
        {
            SetText(TEXT_CUR_ROUND_COUNT, text);
        }

        // ラウンドの最大値のテキスト設定
        public void SetMaxRoundCount(string text)
        {
            SetText(TEXT_MAX_ROUND_COUNT, text);
        }

        // 合計ポイント値のテキスト設定
        public void SetTotalPoint(string text)
        {
            SetText(TEXT_TOTAL_POINT, text);
        }

        // 現在のポイント値のテキスト設定
        public void SetCurPoint(string text)
        {
            SetText(TEXT_CUR_POINT, text);
        }

        // ポイント値の最大値のテキスト設定
        public void SetMaxPoint(string text)
        {
            SetText(TEXT_MAX_POINT, text);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



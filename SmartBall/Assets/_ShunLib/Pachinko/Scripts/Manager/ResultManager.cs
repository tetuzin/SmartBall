using UnityEngine;

using Pachinko.Result.Panel;

namespace Pachinko.Result.Manager
{
    public class ResultManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("リザルトパネル")] 
        [SerializeField] private ResultPanel _panel = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _panel.Initialize();
        }

        // 各パラメータの設定
        public void SetParameter(ResultPanel panel = null)
        {
            _panel = panel;
        }

        // 各種値の設定後、リザルト表示
        public void ShowResult(int feverCount, int totalPoint)
        {
            _panel.SetFeverCount(feverCount.ToString());
            _panel.SetTotalPoint(totalPoint.ToString());
            _panel.Show();
        }

        // リザルト非表示
        public void HideResult()
        {
            _panel.Hide();
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



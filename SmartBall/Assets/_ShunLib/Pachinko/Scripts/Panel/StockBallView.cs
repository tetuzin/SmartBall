using UnityEngine;
using TMPro;

namespace Pachinko.DataCount.Stock
{
    public class StockBallView : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("持ち玉数")]
        [SerializeField] private TextMeshProUGUI _stockBallCount = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            ResetDataView();
        }

        // データ描画
        public void UpdateDataView(int stockBallCount)
        {
            _stockBallCount.text = stockBallCount.ToString();
        }

        // 描画初期化
        public void ResetDataView()
        {
            _stockBallCount.text = "0";
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


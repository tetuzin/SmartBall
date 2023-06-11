using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ShunLib.UI.Slider;

using Pachinko.Model;

namespace Pachinko.DataView
{
    public class DataViewPanel : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ゲーム数・回転数")]
        [SerializeField] private TextMeshProUGUI _rotateCount = default;

        [Header("総大当たり数")]
        [SerializeField] private TextMeshProUGUI _totalHitCount = default;

        [Header("連チャン数")]
        [SerializeField] private TextMeshProUGUI _continueHitCount = default;

        [Header("回転数の履歴")]
        [SerializeField] private List<CommonSlider> _historyContents = default;

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
        public void UpdateDataView(PlayDataModel data)
        {
            if (data == null || data == default) return;

            _rotateCount.text = data.RotateCount.ToString();
            _continueHitCount.text = data.FeverCount.ToString();
            _totalHitCount.text = data.TotalHitCount.ToString();
            for (int i = 0; i < data.HistoryCountArray.Length; i++)
            {
                _historyContents[i].SetCurSlider(data.HistoryCountArray[i]);
            }
        }

        // 描画初期化
        public void ResetDataView()
        {
            _rotateCount.text = "0";
            _totalHitCount.text = "0";
            _continueHitCount.text = "0";
            foreach (CommonSlider slider in _historyContents)
            {
                slider.Initialize(100, true);
                slider.SetCurSlider(0);
            }
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


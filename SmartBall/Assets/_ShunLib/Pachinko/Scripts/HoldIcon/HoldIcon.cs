using UnityEngine;

using Pachinko.Model;
using Pachinko.Const;

namespace Pachinko.HoldView.Icon
{
    public class HoldIcon : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("保留アイコンの種類")] private HoldIconState _holdIconState = default;
        [SerializeField, Tooltip("キャンバスグループ")] private CanvasGroup _canvasGroup = default;
        [SerializeField, Tooltip("最低限の大当たり確率")] private int _minHitProb = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        public HoldIconState HoldIconState
        {
            get { return _holdIconState; }
        }
        public CanvasGroup CanvasGroup
        {
            get { return _canvasGroup; }
        }
        public int MinHitProb
        {
            get { return _minHitProb; }
        }

        public bool IsRemain { get; set; }
        public int Value { get; set; }
        public HoldModel Model { get; set; }
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}
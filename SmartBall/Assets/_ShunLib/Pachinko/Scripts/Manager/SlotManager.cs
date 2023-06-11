using System;
using System.Collections.Generic;
using UnityEngine;

using Pachinko.Const;
using Pachinko.Slot.Controller;
using Pachinko.Slot.Utils;

namespace Pachinko.Slot
{
    public class SlotManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("リール数")]
        [SerializeField] private int _reelCount = 3;

        [Header("図柄数")]
        [SerializeField] private int _designCount = 7;

        [Header("疑似連発生図柄")]
        [SerializeField, Tooltip("図柄番号")] private int _pseudoIndex = -1;

        [Header("リーチしない図柄")]
        [SerializeField, Tooltip("図柄番号")] private List<int> _nonReachIndex = new List<int>();

        [Header("当たらない図柄")]
        [SerializeField, Tooltip("図柄番号")] private List<int> _nonHitIndex = new List<int>();

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // メインスロット
        public SlotBaseController SlotMain { get; set; }

        // 値リスト
        public int[] ValueArray
        {
            get { return _valueArray; }
        }

        // ポーズ
        public bool IsPause
        {
            get { return _isPause; }
            set 
            {
                _isPause = value;
                if (SlotMain != default) SlotMain.IsPause = _isPause;
            }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // ポーズフラグ
        private bool _isPause = default;
        // 値リスト
        private int[] _valueArray = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            IsPause = false;
            
            SlotMain?.Initialize();

            InitializeSlotValue(new int[]{0,0,0});
        }

        // 各パラメータの設定
        public void SetParameter(
            int reelCount = 3, int designCount = 7, int pseudoIndex = -1, 
            List<int> nonReachIndex = null, List<int> nonHitIndex = null
        )
        {
            _reelCount = reelCount;
            _designCount = designCount;
            _pseudoIndex = pseudoIndex;
            _nonReachIndex = nonReachIndex;
            _nonHitIndex = nonHitIndex;
        }

        // 値の生成
        public int[] CreateValue(ValueState valueState)
        {
            int[] intArray = SlotUtils.CreateSlotValue(
                _designCount, _pseudoIndex, valueState, _nonReachIndex, _nonHitIndex
            );
            return intArray;
        }

        // 値の設定
        public void SetValue(int[] slotValue)
        {
            SlotMain?.SetValue(slotValue);
        }

        // スロット値の設定
        public void SetSlotValue(int[] values)
        {
            _valueArray = values;
            SlotMain?.ShowSlotDesigns(values);
        }

        // 回転開始
        public void Rotate(int[] values)
        {
            SlotMain?.Rotate(values);
        }

        // 回転停止
        public void Stop()
        {
            SlotMain.IsRotate = false;
        }

        // 回転停止
        public void ReelStop(Action callback = null)
        {
            SlotMain.Stop(callback);
        }

        // 回転全停止
        public void StopAll(Action callback = null, bool isFast = false)
        {
            SlotMain?.StopAll(callback, isFast);
        }

        // メインスロットの表示・非表示
        public void SetActiveSlotMain(bool b)
        {
            if (SlotMain == default || SlotMain == null) return;
            SlotMain.gameObject.SetActive(b);
        }

        // ---------- Private関数 ----------

        // 出目を初期化
        private void InitializeSlotValue(int[] intArray)
        {
            _valueArray = intArray;
            SlotMain?.InitializeReels(intArray);
        }

        // ---------- protected関数 ---------
    }
}
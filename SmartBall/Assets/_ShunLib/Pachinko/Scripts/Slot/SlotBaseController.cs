using System;
using UnityEngine;

using Pachinko.Slot.Reel;

namespace Pachinko.Slot.Controller
{
    public class SlotBaseController : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("各リール")]
        [SerializeField, Tooltip("リール")] protected SlotReel[] _slotReel = new SlotReel[3];

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // リール数
        public int ReelCount
        {
            get { return _slotReel.Length; }
        }

        // 図柄数
        public int DesignCount
        {
            get { return _slotReel[0].DesignCount; }
        }

        // 回転数
        public int RotateCount { get; set; }
        // スロット回転フラグ
        public bool IsRotate { get; set; }
        // ポーズフラグ
        public bool IsPause { get; set; }

        // スロット回転時コールバック
        public Action SlotRotateCallback { get; set; }
        // スロット停止時コールバック
        public Action SlotStopCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // スロット停止値
        protected int[] _slotValue = default;
        // 次に止めるリール番号
        protected int _nextStopIndex = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public virtual void Initialize()
        {
            // 各種パラメータの初期化
            IsPause = false;
            IsRotate = false;
            RotateCount = 0;
            _nextStopIndex = 0;
            _slotValue = new int[ReelCount];
        }

        // 全リール初期化
        public virtual void InitializeReels(int[] values)
        {
            for (int i = 0; i < _slotReel.Length; i++)
            {
                _slotReel[i].Initialize();
            }

            // 初期表示図柄の設定
            ShowSlotDesigns(values);
        }

        // リールに値を設定し即表示
        public void ShowSlotDesigns(int[] values)
        {
            SetValue(values);
            for (int i = 0; i < _slotReel.Length; i++)
            {
                _slotReel[i].InitializeSlotDesigns(_slotValue[i]);
            }
        }

        // スロット出目設定
        public virtual void SetValue(int[] values)
        {
            int[] intArray = values;
            for (int i = 0; i < _slotReel.Length; i++)
            {
                _slotValue[i] = intArray[i];
                _slotReel[i].SetStopDesign(_slotValue[i]);
            }
        }

        // 回転開始
        public virtual void Rotate()
        {
            IsRotate = true;
            _nextStopIndex = 0;
            for (int i = 0; i < _slotReel.Length; i++)
            {
                _slotReel[i].StartRotate(_slotValue[i]);
            }
            SlotRotateCallback?.Invoke();
        }

        // 回転開始(値指定)
        public virtual void Rotate(int[] values)
        {
            SetValue(values);
            Rotate();
        }

        // 回転停止(全リール)
        public virtual void StopAll(Action stopCallback = null, bool isFast = false)
        {
            if (isFast)
            {
                // 即停止
                for (int i = 0; i < _slotReel.Length; i++)
                {
                    _slotReel[i].RotateStop(_slotValue[i]);
                    _slotReel[i].SetDesignAlpha(1f, 0f);
                    _slotReel[i].SetReelAlpha(1f);
                }
                stopCallback?.Invoke();
            }
            else
            {
                // ゆっくり停止
                UpdateDesignPositions();
                for (int i = 0; i < _slotReel.Length; i++)
                {
                    StopReel(i, i == 0 ? stopCallback : null, true);
                }
            }
            IsRotate = false;
            RotateCount++;
            SlotStopCallback?.Invoke();
        }

        // 回転停止(一つ)
        public virtual void Stop(Action callback = null)
        {
            StopReel(_nextStopIndex, callback);
            _nextStopIndex++;
        }

        // 各リールの透明度設定
        public void SetReelAlpha(int index, float alpha)
        {
            _slotReel[index].SetReelAlpha(alpha);
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

         // リールの停止
        protected void StopReel(int reelIndex, Action stopCallback = null, bool fast = false)
        {
            if (reelIndex >= _slotReel.Length) return;
            
            _slotReel[reelIndex].Stop(_slotValue[reelIndex], stopCallback, fast);
        }

        // 各リールの座標整形
        protected void UpdateDesignPositions()
        {
            for (int i = 0; i < _slotReel.Length; i++)
            {
                _slotReel[i].UpdateDesignPositions(_slotValue[i]);
            }
        }

        // 全リールの透明度設定
        protected void SetReelAlpha(float alpha)
        {
            for (int i = 0; i < _slotReel.Length; i++)
            {
                _slotReel[i].SetReelAlpha(alpha);
            }
        }
    }
}



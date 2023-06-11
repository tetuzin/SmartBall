using UnityEngine;
using DG.Tweening;

namespace Pachinko.Slot.Design
{
    public enum SlotAnimState
    {
        Idle = 0,
        Rotate = 1,
        Stop = 2,
        Reach = 3
    }

    public class SlotDesign : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("RectTrans")] private RectTransform _rectTrans = default;
        [SerializeField, Tooltip("CanvasGroup")] private CanvasGroup _canvasGroup = default;
        [SerializeField, Tooltip("図柄値")] private int _value = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        
        // アニメーション再生
        public void PlayAnimation(SlotAnimState animState)
        {
            switch (animState)
            {
                case SlotAnimState.Idle:
                    // Debug.Log("アイドルアニメーション再生");
                    return;
                case SlotAnimState.Rotate:
                    // Debug.Log("開始アニメーション再生");
                    return;
                case SlotAnimState.Stop:
                    // Debug.Log("停止アニメーション再生");
                    return;
                case SlotAnimState.Reach:
                    // Debug.Log("リーチアニメーション再生");
                    return;
                default:
                    // Debug.Log("アニメーションなし");
                    return;
            }
        }

        // 透明度の設定
        public void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        // フェード
        public void Fade(float alpha, float speed)
        {
            _canvasGroup.DOFade(alpha, speed);
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartBall
{
    public class TouchPanel : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // タップ中に移動しているときのコールバック
        public Action<Vector2, Vector2> MoveTappingCallback { get; set; }
        // タップを解除したときのコールバック
        public Action ReleaseTappingCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool isTouch = default;
        private Vector2 clickPos = default;
        private float touchTime = default;

        // ---------- Unity組込関数 ----------

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                touchTime = 0f;
                clickPos = Vector2.zero;
                isTouch = false;
                ReleaseTappingCallback?.Invoke();
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                touchTime = 0f;
                clickPos = Input.mousePosition;
                isTouch = true;
            }

            if (isTouch)
            {
                touchTime += Time.deltaTime;
                if (clickPos != Vector2.zero)
                {
                    MoveTappingCallback?.Invoke(clickPos, Input.mousePosition);
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            isTouch = false;
            clickPos = Vector2.zero;
            touchTime = 0f;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


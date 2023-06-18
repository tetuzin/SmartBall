using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmartBall
{
    public class SmartBallLever : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("RB")]
        [SerializeField] protected Rigidbody _rb = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private Vector3 defaultPos = default;
        private bool isRelease = default;

        // ---------- Unity組込関数 ----------

        void Update()
        {
            if (isRelease && this.transform.localPosition.z >= defaultPos.z)
            {
                isRelease = false;
                _rb.velocity = Vector3.zero;
                this.transform.localPosition = defaultPos;
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            defaultPos = this.transform.localPosition;
            isRelease = false;
        }

        // レバーを引いているときの処理
        public void Pull(Vector2 beforePos, Vector2 afterPos)
        {
            if (beforePos.y < afterPos.y) return;

            float distance = (afterPos.y - beforePos.y) / 100;
            this.transform.localPosition = new Vector3(
                defaultPos.x, defaultPos.y, defaultPos.z + distance
            );
        }

        // レバーを離したときの処理
        public void Release()
        {
            // this.transform.localPosition = defaultPos;

            isRelease = true;
            float forceValue = Mathf.Abs(Vector3.Distance(this.transform.localPosition, defaultPos));
            forceValue *= 100;
            Debug.Log(forceValue);
            Vector3 force = new Vector3(0f, 0f, forceValue);
            _rb.AddForce(force, ForceMode.Impulse);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


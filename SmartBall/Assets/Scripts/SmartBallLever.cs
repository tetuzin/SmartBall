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
        private Vector3 _force = default;
        private bool isRelease = default;
        private bool isStop = default;

        // ---------- Unity組込関数 ----------

        void Update()
        {
            if (isStop)
            {
                this.transform.localPosition = defaultPos;
            }
            if (isRelease && this.transform.localPosition.z >= defaultPos.z)
            {
                isRelease = false;
                _rb.velocity = Vector3.zero;
                this.transform.localPosition = defaultPos;
                isStop = true;
            }
            if (isRelease)
            {
                _rb.AddForce(_force);
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            defaultPos = this.transform.localPosition;
            _force = Vector3.zero;
            isRelease = false;
            isStop = true;
        }

        // レバーを引いているときの処理
        public void Pull(Vector2 beforePos, Vector2 afterPos)
        {
            if (beforePos.y < afterPos.y) return;

            isStop = false;
            isRelease = false;
            float distance = (afterPos.y - beforePos.y) / 100;
            this.transform.localPosition = new Vector3(
                defaultPos.x, defaultPos.y, defaultPos.z + distance
            );
        }

        // レバーを離したときの処理
        public void Release()
        {
            isRelease = true;
            float forceValue = Mathf.Abs(Vector3.Distance(this.transform.localPosition, defaultPos));
            forceValue *= 150000f;
            _force = new Vector3(0f, 0f, 600000f + forceValue);
            Debug.Log(_force);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


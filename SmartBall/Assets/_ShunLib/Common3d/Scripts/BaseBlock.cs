using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShunLib.Lib3D.Block.Block
{
    public class BaseBlock : MonoBehaviour, IPointerClickHandler, IPointerUpHandler
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("硬度")]
        [SerializeField] public float hardness = 3f;

        [Header("耐久値を記憶する")]
        [SerializeField] public bool isLeaveDurable = false;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // 耐久値
        protected float durableValue = default;
        // 破壊中
        protected bool isBreaking = default;

        // ---------- Unity組込関数 ----------

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            // 破壊中の処理
            if (isBreaking)
            {
                durableValue -= Time.deltaTime;
                if (durableValue <= 0f)
                {
                    BreakBlock();
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            durableValue = hardness;
            isBreaking = false;
        }

        // ブロックをクリックして選択した時の処理
        public void OnPointerClick(PointerEventData eventData)
        {
            switch(eventData.pointerId)
            {
                // 左クリック
                case -1:
                    isBreaking = true;
                    break;

                // 右クリック
                case -2:
                    UseBlock();
                    break;
            }
        }

        // ブロックの選択を解除した時の処理
        public void OnPointerUp(PointerEventData eventData)
        {
            switch(eventData.pointerId)
            {
                // 左クリック
                case -1:
                    isBreaking = false;
                    if (!isLeaveDurable) durableValue = hardness;
                    break;
            }
        }

        // ブロックを使用
        public void UseBlock()
        {

        }

        // ブロックが破壊された時の処理
        public void BreakBlock()
        {
            Destroy(this.gameObject);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


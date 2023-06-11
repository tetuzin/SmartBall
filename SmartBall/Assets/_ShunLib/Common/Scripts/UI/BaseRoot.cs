using UnityEngine;

using ShunLib.Controller.InputKey;
using ShunLib.Controller.InputMouse;

namespace ShunLib.Root
{
    public class BaseRoot : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        
        [Header("BaseRootの変数")]
        [SerializeField, Tooltip("Cameraオブジェクト")] protected Camera mainCamera = default;
        [SerializeField, Tooltip("Canvasオブジェクト")] protected GameObject canvas = default;
        [SerializeField, Tooltip("AudioSourceオブジェクト")] protected AudioSource audioSource = default;
        [SerializeField, Tooltip("キーボードコントローラ")] protected InputKeyController keyCtr = default;
        [SerializeField, Tooltip("マウスボタンコントローラ")] protected InputMouseController mouseCtr = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------

        void Start()
        {
            InitRoot();
        }
        // ---------- Public関数 ----------

        // 初期化
        public virtual void InitRoot()
        {
            SetData();
            SetButtonEvents();
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // ボタンイベントを設定する（継承先でOverrideする）
        protected virtual void SetButtonEvents(){}

        // データを設定する（継承先でOverrideする）
        protected virtual void SetData() {}
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

using ShunLib.Manager.Initialize;

namespace ShunLib
{
    [DefaultExecutionOrder(-1)]
    public class SceneChecker : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------

        private void Awake()
        {
            Initialize();
        }

        // ---------- Public関数 ----------
        // ---------- Private関数 ----------

        private void Initialize()
        {
            if (!InitializeManager.IsInitialize)
            {
                SceneManager.LoadScene(0);
            }
        }

        // ---------- protected関数 ---------
    }
}
using UnityEngine;

using ShunLib.Singleton;
using ShunLib.Manager.Master;
using ShunLib.Manager.Audio;
using ShunLib.Manager.Data;

namespace ShunLib.Manager.Game
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("マネージャー")] 
        [SerializeField] public MasterManager masterManager = default;
        [SerializeField] public DataManager dataManager = default;
        [SerializeField] public AudioManager audioManager = default;

        [Header("デバッグモードフラグ")] 
        [SerializeField] public bool isDebugMode = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}
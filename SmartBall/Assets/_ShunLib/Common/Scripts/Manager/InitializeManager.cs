using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Manager.Scene;
using ShunLib.Manager.Game;
using ShunLib.Manager.Master;
using ShunLib.Manager.Audio;
using ShunLib.Manager.Data;

using ShunLib.PlayFab.Manager;
using ShunLib.Strix.Manager.Master;
using ShunLib.Strix.Manager.Room;

namespace ShunLib.Manager.Initialize
{
    [DefaultExecutionOrder(-1)]
    public class InitializeManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        
        [Header("マネージャー")] 
        [SerializeField] protected MasterManager _masterManager = default;
        [SerializeField] protected DataManager _dataManager = default;
        [SerializeField] protected AudioManager _audioManager = default;
        [SerializeField] protected GameManager _gameManager = default;
        [SerializeField] protected PlayFabManager _playFabManager = default;
        [SerializeField] protected StrixMasterManager _strixMasterManager = default;
        [SerializeField] protected StrixRoomManager _strixRoomManager = default;

        [Header("最初に遷移するシーン名")] 
        [SerializeField] protected string _transSceneName = default;
        
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public static bool IsInitialize
        {
            get { return _isInitialize; }
        }
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private static bool _isInitialize = false;

        // ---------- Unity組込関数 ----------
        
        void Start()
        {
            Initialize();
        }

        // ---------- Public関数 ----------

        // 各マネージャーの設定
        public void SetManager(
            MasterManager masterManager,
            DataManager dataManager,
            AudioManager audioManager,
            GameManager gameManager
        )
        {
            _masterManager = masterManager;
            _dataManager = dataManager;
            _audioManager = audioManager;
            _gameManager = gameManager;
        }

        // PlayFabManagerの設定
        public void SetPlayFabManager(PlayFabManager manager)
        {
            _playFabManager = manager;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // 起動時の初期設定
        protected virtual async void Initialize()
        {
            // 基本機能
            _gameManager.Initialize();
            await InitMasterManager();
            await InitDataManager();
            await InitAudioManager();

            // PlayFab
            await InitPlayFabManager();

            // Strix
            await InitStrixMasterManager();
            await InitStrixRoomManager();

            _isInitialize = true;
            
            Transition();
        }
        
        // MasterManagerの初期化
        protected virtual async Task InitMasterManager()
        {
            if (_masterManager != default)
            {
                await _masterManager.Initialize();
                Debug.Log("初期化完了[MasterManager]");
            }
        }
        
        // UserManagerの初期化
        protected virtual async Task InitDataManager()
        {
            if (_dataManager != default)
            {
                await _dataManager.Initialize();
                Debug.Log("初期化完了[DataManager]");
            }
        }
        
        // AudioManagerの初期化
        protected virtual async Task InitAudioManager()
        {
            if (_audioManager != default)
            {
                await _audioManager.Initialize();
                Debug.Log("初期化完了[AudioManager]");
            }
        }

        // PlayFabManagerの初期化
        protected virtual async Task InitPlayFabManager()
        {
            if (_playFabManager != default)
            {
                await _playFabManager.Initialize();
                Debug.Log("初期化完了[PlayFabManager]");
            }
        }

        // StrixMasterManagerの初期化
        protected virtual async Task InitStrixMasterManager()
        {
            if (_strixMasterManager != default)
            {
                await _strixMasterManager.Initialize();
                Debug.Log("初期化完了[StrixMasterManager]");
            }
        }

        // StrixRoomManagerの初期化
        protected virtual async Task InitStrixRoomManager()
        {
            if (_strixRoomManager != default)
            {
                await _strixRoomManager.Initialize();
                Debug.Log("初期化完了[StrixRoomManager]");
            }
        }

        // 最初に遷移するシーン名を取得
        protected virtual string GetSceneName()
        {
            return _transSceneName;
        }
        
        // 最初のシーンへ遷移
        protected virtual void Transition()
        {
            string sceneName = GetSceneName();
            SceneLoadManager.Instance.TransitionScene(sceneName);
        }
    }
}


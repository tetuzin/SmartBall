using UnityEngine;

using ShunLib.Manager.UI;
using ShunLib.Manager.Initialize;
using ShunLib.Manager.Camera;
using ShunLib.Controller.Character3D;
using ShunLib.Controller.InputKey;
using ShunLib.Utils.Debug;

namespace ShunLib.Manager.CommonScene
{
    public class CommonSceneManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("カメラマネージャー")]
        [SerializeField] protected CameraManager _cameraManager = default;

        [Header("UIマネージャー")]
        [SerializeField] protected UIManager _uiManager = default;

        [Header("キャラクターコントローラ3D")]
        [SerializeField] protected CharacterController3D _characterController3D = default;

        [Header("キーコントローラ")]
        [SerializeField] protected InputKeyController _inputKeyController = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------

        void Start()
        {
            if (!InitializeManager.IsInitialize) return;

            Initialize();
        }

        // ---------- Public関数 ----------
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // 初期化
        protected virtual void Initialize()
        {
            // UIManager初期化
            _uiManager.Initialize();

            // CameraManager初期化
            _cameraManager.Initialize();

            // キャラクターコントローラ3D初期化
            if (_characterController3D != default && _characterController3D != null)
            {
                _characterController3D?.SetCameraManager(_cameraManager);
                _cameraManager.SetTrackObject(_characterController3D.GetAvatorCamera());
                _characterController3D?.Initialize();
            }
            else
            {
                DebugUtils.LogWarning("キャラクターコントローラが設定されていません！");
            }
            
            // キーコントローラ初期化
            if (_inputKeyController != default)
            {
                _inputKeyController.Initialize();
                _characterController3D?.SetKeyController(_inputKeyController);
                _inputKeyController.EnableKeyCtrl = true;
            }
        }

        // キャラクターコントローラのオンオフ
        protected virtual void SwitchEnableCharacterController(bool b)
        {
            if (_characterController3D != default)
            {
                _characterController3D.IsEnable = b;
            }
        }
    }
}


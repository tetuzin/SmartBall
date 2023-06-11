using UnityEngine;
using Cinemachine;

using ShunLib.Dict;

namespace ShunLib.Manager.Camera
{
    public class CameraManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        
        // 一人称視点カメラ
        public const string CAMERA_FIRST_PERSON = "CameraFirstPerson";
        // 三人称視点カメラ
        public const string CAMERA_THIRD_PERSON = "CameraThirdPerson";

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("メインカメラ")] protected CinemachineBrain _mainCamera = default;
        [SerializeField, Tooltip("追跡するオブジェクト")] protected Transform _object = default;

        [SerializeField, Tooltip("デフォルトカメラ")] protected CinemachineVirtualCamera _defaultCamera = default;
        [SerializeField, Tooltip("その他カメラ")] protected CinemachineVirtualCameraTable _cameraTable = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public CinemachineVirtualCamera VirtualCamera
        {
            get { return _virtualCamera; }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private CinemachineVirtualCamera _virtualCamera = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            SetVirtualCamera(_defaultCamera);
        }

        // 追跡対象の設定
        public void SetTrackObject(Transform trackObj)
        {
            _object = trackObj;
        }

        // カメラの切り替え
        public void ChangeCamera(string key)
        {
            if (!_cameraTable.GetKeyList().Contains(key)) return;
            SetVirtualCamera(_cameraTable.GetValue(key));
        }

        // ---------- Private関数 ----------

        // カメラ設定
        private void SetVirtualCamera(CinemachineVirtualCamera virtualCamera)
        {
            if (_virtualCamera != default) 
            {
                _virtualCamera.Priority = 1;

                if (_object != default)
                {
                    virtualCamera.transform.rotation = _virtualCamera.transform.rotation;
                    virtualCamera.transform.position = Vector3.zero;
                }
            }

            _virtualCamera = virtualCamera;

            if (_object != default)
            {
                _virtualCamera.Follow = _object.transform;
                _virtualCamera.LookAt = _object.transform;
            }
            _virtualCamera.Priority = 100;
        }

        // ---------- protected関数 ---------
    }
}
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;

using ShunLib.Controller.InputKey;
using ShunLib.Manager.Camera;

namespace ShunLib.Controller.Character3D
{
    public class CharacterController3D : StrixBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("モデルオブジェクト")] private GameObject _modelObj = default;
        [SerializeField, Tooltip("モデルリジットボディ")] private Rigidbody _modelRB = default;
        [SerializeField, Tooltip("アバターアニメーター")] private Animator _avatar = default;
        [SerializeField, Tooltip("アニメーターコントローラ")] private RuntimeAnimatorController _AnimCtrl = default;
        [SerializeField, Tooltip("視点位置")] private Transform _avatarCamera = default;
        [SerializeField, Tooltip("移動速度")] private float _moveSpeed = 5.0f;
        [SerializeField, Tooltip("カメラ回転速度")] private float _rotateSpeed = 10.0f;
        [SerializeField, Tooltip("ジャンプON")] protected bool _isJump = default;
        [SerializeField, Tooltip("ジャンプ力")] private float _jumpPawer = 5.0f;
        [SerializeField, Tooltip("地面タグ")] protected string _groundTag = "";

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public bool IsEnable { get; set; }
        public bool IsMoveCamera { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool _isForward = default;
        private bool _isBackward = default;
        private bool _isLeft = default;
        private bool _isRight = default;
        private bool _isGround = default;

        private bool _isFirstPerson = default;

        private CameraManager _cameraManager = default;
        private InputKeyController _keyController = default;
        private Vector3 _movingVelocity;

        // ---------- Unity組込関数 ----------

        void Update()
        {
            rotateCamera();
            SetAnimation();
        }

        // ---------- Public関数 ----------

        public void Initialize()
        {
            _isForward = false;
            _isBackward = false;
            _isLeft = false;
            _isRight = false;
            IsEnable = true;
            IsMoveCamera = true;
            _isFirstPerson = true;

            _avatar.runtimeAnimatorController = _AnimCtrl;

            _cameraManager.ChangeCamera(
                _isFirstPerson ? CameraManager.CAMERA_FIRST_PERSON : CameraManager.CAMERA_THIRD_PERSON
            );
        }

        // 目線POSを返す
        public Transform GetAvatorCamera()
        {
            return _avatarCamera;
        }

        // モデルオブジェクトを返す
        public GameObject GetModelObject()
        {
            return _modelObj;
        }

        // キャラクターを指定座標へ移動
        public void MovePosition(Transform pos)
        {
            this.transform.position = pos.position;
        }

        // スケールの設定
        public void SetScale(Vector3 scale)
        {
            this.transform.localScale = scale;
            _moveSpeed *= scale.x;
            _modelRB.mass *= scale.x;
        }

        // キーコントローラに処理を設定
        public void SetKeyController(InputKeyController keyController)
        {
            _keyController = keyController;
            _keyController.AddKeyDownAction(KeyCode.Q, () => { ChangeCamera(); });

            _keyController.AddKeyDownAction(KeyCode.W, () => { if (!IsEnable) return; _isForward = true; _isBackward = false; });
            _keyController.AddKeyStayAction(KeyCode.W, MoveForward);
            _keyController.AddKeyUpAction(KeyCode.W, () => { if (!IsEnable) return; _isForward = false; _modelRB.velocity = Vector3.zero; });
            
            _keyController.AddKeyDownAction(KeyCode.S, () => { if (!IsEnable) return; _isBackward = true; _isForward = false; });
            _keyController.AddKeyStayAction(KeyCode.S, MoveBackward);
            _keyController.AddKeyUpAction(KeyCode.S, () => { if (!IsEnable) return; _isBackward = false; _modelRB.velocity = Vector3.zero; });

            _keyController.AddKeyDownAction(KeyCode.A, () => { if (!IsEnable) return; _isLeft = true; _isRight = false; });
            _keyController.AddKeyStayAction(KeyCode.A, MoveLeft);
            _keyController.AddKeyUpAction(KeyCode.A, () => { if (!IsEnable) return; _isLeft = false; _modelRB.velocity = Vector3.zero; });

            _keyController.AddKeyDownAction(KeyCode.D, () => { if (!IsEnable) return; _isRight = true; _isLeft = false; });
            _keyController.AddKeyStayAction(KeyCode.D, MoveRight);
            _keyController.AddKeyUpAction(KeyCode.D, () => { if (!IsEnable) return; _isRight = false; _modelRB.velocity = Vector3.zero; });

            if (_isJump)
            {
                _keyController.AddKeyDownAction(KeyCode.Space, MoveJump);
            }
        }

        // キーコントローラの処理を削除
        public void RemoveKeyController()
        {
            _keyController.RemoveKeyDownAction(KeyCode.Q);
            _keyController.RemoveKeyDownAction(KeyCode.W);
            _keyController.RemoveKeyStayAction(KeyCode.W);
            _keyController.RemoveKeyUpAction(KeyCode.W);
            _keyController.RemoveKeyDownAction(KeyCode.S);
            _keyController.RemoveKeyStayAction(KeyCode.S);
            _keyController.RemoveKeyUpAction(KeyCode.S);
            _keyController.RemoveKeyDownAction(KeyCode.A);
            _keyController.RemoveKeyStayAction(KeyCode.A);
            _keyController.RemoveKeyUpAction(KeyCode.A);
            _keyController.RemoveKeyDownAction(KeyCode.D);
            _keyController.RemoveKeyStayAction(KeyCode.D);
            _keyController.RemoveKeyUpAction(KeyCode.D);
            _keyController.RemoveKeyDownAction(KeyCode.Space);
        }

        // カメラマネージャーの設定
        public void SetCameraManager(CameraManager cameraManager)
        {
            _cameraManager = cameraManager;
        }

        // カメラチェンジ
        public void ChangeCamera()
        {
            _isFirstPerson = !_isFirstPerson;
            _cameraManager.ChangeCamera(
                _isFirstPerson ? CameraManager.CAMERA_FIRST_PERSON : CameraManager.CAMERA_THIRD_PERSON
            );
        }

        // カメラチェンジ
        public void ChangeCamera(string key)
        {
            switch(key)
            {
                case CameraManager.CAMERA_THIRD_PERSON:
                    _isFirstPerson = false;
                    break;
                default:
                    _isFirstPerson = true;
                    break;
            }
            _cameraManager.ChangeCamera(key);
        }

        public override void OnStrixSerialize(StrixSerializationProperties properties)
        {
            base.OnStrixSerialize(properties);
            properties.Set("_isForward", _isForward);
            properties.Set("_isBackward", _isBackward);
            properties.Set("_isLeft", _isLeft);
            properties.Set("_isRight", _isRight);
            properties.Set("_isGround", _isGround);
        }

        public override void OnStrixDeserialize(StrixSerializationProperties properties)
        {
            base.OnStrixDeserialize(properties);
            properties.Get("_isForward", ref _isForward);
            properties.Get("_isBackward", ref _isBackward);
            properties.Get("_isLeft", ref _isLeft);
            properties.Get("_isRight", ref _isRight);
            properties.Get("_isGround", ref _isGround);
        }

        // ---------- Private関数 ----------

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.CompareTag(_groundTag))
            {
                if (_isGround) return;
                _isGround = true;
            }
        }

        // ---------- protected関数 ---------

        // 前へ進む(押下中)
        protected void MoveForward()
        {
            if (!IsEnable) return;
            if (!_isForward && _isBackward) return;
            _isForward = true;
            // transform.position += transform.rotation * Vector3.forward * _moveSpeed * Time.deltaTime;
            _modelRB.velocity = _modelObj.transform.rotation * Vector3.forward * _moveSpeed;
        }

        // 後ろへ進む(押下中)
        protected void MoveBackward()
        {
            if (!IsEnable) return;
            if (!_isBackward && _isForward) return;
            _isBackward = true;
            // transform.position += transform.rotation * Vector3.back * (_moveSpeed / 2) * Time.deltaTime;
            _modelRB.velocity = _modelObj.transform.rotation * Vector3.back * (_moveSpeed / 2);
        }

        // 左へ進む(押下中)
        protected void MoveLeft()
        {
            if (!IsEnable) return;
            if (!_isLeft && _isRight) return;
            _isLeft = true;
            // transform.position += transform.rotation * Vector3.left * (_moveSpeed / 2) * Time.deltaTime;
            _modelRB.velocity = _modelObj.transform.rotation * Vector3.left * (_moveSpeed / 2);
        }

        // 右へ進む(押下中)
        protected void MoveRight()
        {
            if (!IsEnable) return;
            if (!_isRight && _isLeft) return;
            _isRight = true;
            // transform.position += transform.rotation * Vector3.right * (_moveSpeed / 2) * Time.deltaTime;
            _modelRB.velocity = _modelObj.transform.rotation * Vector3.right * (_moveSpeed / 2);
        }

        // ジャンプ
        protected void MoveJump()
        {
            if (!IsEnable) return;
            if (!_isGround) return;
            _isGround = false;
            _avatar.SetInteger("State", 10);
            _modelRB.velocity = Vector3.up * _jumpPawer;
        }

        // アニメーションの設定
        protected void SetAnimation()
        {
            if (!_isGround) return;
            
            if (_isForward && _isLeft) _avatar.SetInteger("State", 5);
            else if (_isForward && _isRight) _avatar.SetInteger("State", 6);
            else if (_isForward) _avatar.SetInteger("State", 1);
            else if (_isBackward && _isLeft) _avatar.SetInteger("State", 7);
            else if (_isBackward && _isRight) _avatar.SetInteger("State", 8);
            else if (_isBackward) _avatar.SetInteger("State", 2);
            else if (_isLeft) _avatar.SetInteger("State", 4);
            else if (_isRight) _avatar.SetInteger("State", 3);
            else _avatar.SetInteger("State", 0);
        }

        // HACK カメラを回転させる関数
        protected void rotateCamera()
        {
            if (!IsEnable) return;
            if (!IsMoveCamera) return;

            if (_cameraManager == default) return;

            //Vector3でX,Y方向の回転の度合いを定義
            float x = Input.GetAxis("Mouse X") * _rotateSpeed;
            float y = -(Input.GetAxis("Mouse Y") * _rotateSpeed);
            float maxLimit = 60, minLimit = 360 - maxLimit;

            // 回転
            var localAngle = _cameraManager.VirtualCamera.transform.localEulerAngles;
            localAngle.x += y;
            if (localAngle.x > maxLimit && localAngle.x < 180)
                localAngle.x = maxLimit;
            if (localAngle.x < minLimit && localAngle.x > 180)
                localAngle.x = minLimit;
            localAngle.y += x;
            _cameraManager.VirtualCamera.transform.localEulerAngles = localAngle;
    
            //transform.RotateAround()をしようしてメインカメラを回転させる
            // transform.RotateAround(transform.position, Vector3.up, x);
            _modelObj.transform.rotation = Quaternion.Euler(0f, localAngle.y, 0f);
        }
    }
}



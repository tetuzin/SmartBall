using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

using ShunLib.Dict;
using ShunLib.Manager.Video;
using ShunLib.Manager.Particle;
using ShunLib.Manager.Audio;
using ShunLib.Controller.InputKey;
using ShunLib.Strix.Manager.Room;

using SoftGear.Strix.Unity.Runtime;
using SoftGear.Strix.Unity.Runtime.Event;

using Pachinko.Model;
using Pachinko.Resource;
using Pachinko.Const;
using Pachinko.DataView;
using Pachinko.Ball;

// マネージャー
using Pachinko.Manager.Pachinko;
using Pachinko.Manager.Accessory;
using Pachinko.Manager.Light;
using Pachinko.Manager.Emission;
using Pachinko.ModeSelect.Manager;
using Pachinko.Round.Manager;
using Pachinko.Result.Manager;
using Pachinko.DataCount.Manager;
using Pachinko.DataCount.Stock;

// パネル
using Pachinko.GameMode.Base.Panel;
using Pachinko.GameMode.Select.Panel;
using Pachinko.DataCount.Panel;
using Pachinko.Round.Panel;
using Pachinko.Result.Panel;

namespace Pachinko.Machine
{
    public class PachinkoMachine : StrixBehaviour, IPointerClickHandler
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("パチンコ名")]
        [SerializeField] public string pachinkoName = "Test_Pachinko";

        [Header("パチンコマネージャー")]
        [SerializeField] public PachinkoManager manager = default;

        [Header("オーディオマネージャー")]
        [SerializeField] public AudioManager audioManager = default;

        [Header("役物マネージャー")]
        [SerializeField] public PachinkoAccessoryManager accessoryManager = default;

        [Header("ライトマネージャー")]
        [SerializeField] public PachinkoLightManager lightManager = default;

        [Header("エミッションマネージャー")]
        [SerializeField] public PachinkoEmissionManager emissionManager = default;

        [Header("遊戯中のプレイヤーの位置")]
        [SerializeField] public Transform playPos = default;

        [Header("パチンコリソース")]
        [SerializeField] public PachinkoMachineResourceScriptableObject resource = default;

        [Header("パーティクル表示ParentObject")]
        [SerializeField] public Transform particleParent = default;

        [Header("動画再生コンポーネント")]
        [SerializeField] public VideoPlayerTable videoPlayerTable = default;

        [Header("パネル配置オブジェクト")]
        [SerializeField] public Transform panels = default;

        [Header("カットイン配置オブジェクト")]
        [SerializeField] public Transform cutins = default;

        [Header("パチンコ玉")]
        [SerializeField] private PachinkoBall _pachiSphere = default;

        [Header("パチンコ玉_生成場所")]
        [SerializeField] private Transform _createSpherePoint = default;

        [Header("パチンコ玉_発射場所")]
        [SerializeField] private Transform _shotPos = default;

        [Header("パチンコ玉_弾の最大速度")]
        [SerializeField] private float _maxShotSpeed = 7.5f;

        [Header("パチンコ玉_発射間隔時間")]
        [SerializeField] private float _shotIntervalTime = 0.5f;

        [Header("生成するパチンコ玉の数")]
        [SerializeField] private int _createSphereCount = 20;

        [Header("ハンドル")]
        [SerializeField] private Transform _handle = default;

        [Header("入賞口リスト")]
        [SerializeField] private List<PachiSphereChucker> _chuckerList = default;

        [Header("右打ち時・ポイント獲得穴の移動パネル")]
        [SerializeField] private Transform _movePanel_1 = default;

        [Header("右打ち時・保留チャージ穴の移動パネル")]
        [SerializeField] private Transform _movePanel_2 = default;

        [Header("データ表示パネル配置オブジェクト")]
        [SerializeField] private Transform _dataViewPanelParent = default;

        [Header("データ表示パネルプレハブ")]
        [SerializeField] private DataViewPanel _dataViewPanelPrefab = default;

        [Header("持ち玉表示パネル配置オブジェクト")]
        [SerializeField] private Transform _stockBallViewParent = default;

        [Header("持ち玉表示パネルプレハブ")]
        [SerializeField] private StockBallView _stockBallViewPrefab = default;

        [Header("デバッグ用")]
        [SerializeField] public bool _isDebugMode = false;
        [SerializeField] public bool _isSkipDirect = false;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // 遊戯中フラグ
        public bool IsPlay { get; set; }

        // パチンコ初期化終了コールバック
        public Action InitializeEndCallback { get; set; }
        // パチンコ選択時コールバック
        public Action<PachinkoMachine> OnClickPachinkoCallback { get; set; }
        // パチンコ遊戯終了コールバック
        public Action PlayEndPachinkoCallback { get; set; }
        // 持ち玉保存処理コールバック
        public Action<int> SavePointCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        private InputKeyController _keyController = default;
        private PlayDataModel _data = default;
        private DataViewPanel _dataViewPanel = default;
        private StockBallView _stockBallView = default;
        private bool _isRotate = default;

        // 玉発射用変数
        private int _stockBallCount = default;
        private bool _isShot = default;
        private float _shotSpeed = default;
        private float _shotTime = default;
        private Dictionary<int, PachinkoBall> _sphereDict = default;
        private int _curShotPachiNumber = default;
        private bool _isMovePanel = default;

        // ---------- Unity組込関数 ----------

        void Start()
        {
            Initialize();
        }

        void FixedUpdate()
        {
            // パチンコ玉発射
            if (_isShot)
            {
                _shotTime += Time.deltaTime;
                if (_shotTime >= _shotIntervalTime)
                {
                    ShotBall();
                    _shotTime = 0f;
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public async void Initialize()
        {
            if (resource == default || resource == null) return;

            _isRotate = false;
            _stockBallCount = 0;
            _isShot = false;
            _shotSpeed = 0f;
            _shotTime = 0f;
            _curShotPachiNumber = 0;
            _isMovePanel = false;

            CreateDataViewPanel();
            CreateStockBallView();

            if (isLocal)
            {
                InitializePlayData();
                InitializeChuckerCallback();
                CreatePachiSphere(_createSphereCount);
            }

            await CreatePachinkoManager();

            accessoryManager?.Initialize();

            // パチンコマネージャーの初期化
            manager.SetResource(resource);

            await manager.Initialize();

            InitializeEndCallback?.Invoke();
        }

        // ゲームスタート
        public void StartGame(int usePoint)
        {
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToRoomOwner(
                    nameof(PachinkoMachine.StartPlayPachiRPC),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    usePoint
                );
            }
            else
            {
                StartPlayPachiRPC(usePoint);
            }

            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoMachine.SetIsPlayFlag),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    true
                );
            }
            else
            {
                SetIsPlayFlag(true);
            }

            // // TODO MetaPachi ルーム入室前に初期化を行うとレプリカ同期エラーが発生する
            // // ルーム入室前ではないっぽい。タイミングが分からないけど、この箇所なら発生しない
            // // タイミングの問題ではないかも
            // InitializePlayData();

            // キーコントローラ設定
            manager?.SetKeyController(_keyController);
            _keyController.AddKeyStayAction(KeyCode.RightArrow, TurnRightHandle);
            _keyController.AddKeyStayAction(KeyCode.LeftArrow, TurnLeftHandle);
            _keyController.AddKeyDownAction(KeyCode.Space, PushButton);
            if (_isDebugMode)
            {
                _keyController.AddKeyDownAction(KeyCode.C, DebugHitMode);
            }
#if UNITY_EDITOR
            // デバッグ用キー設定
            _keyController.AddKeyDownAction(KeyCode.Return, OnEnterPrizeHole);
            _keyController.AddKeyDownAction(KeyCode.V, PullLever);
            _keyController.AddKeyDownAction(KeyCode.P, ShotBall);
            _keyController.AddKeyDownAction(KeyCode.R, OnEnterGetPointHole);
            _keyController.AddKeyDownAction(KeyCode.F, OnEnterFeverModeHole);
#endif
        }

        // ハンドルを右に回す
        public void TurnRightHandle()
        {
            if (_shotSpeed >= _maxShotSpeed) return; 
            _shotSpeed += (_maxShotSpeed / 50f);
            _isShot = true;
            if (_shotSpeed >= _maxShotSpeed)
            {
                _shotSpeed = _maxShotSpeed;
            }
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoMachine.SetHandleRotate),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    _shotSpeed * 20f
                );
            }
            else
            {
                SetHandleRotate(_shotSpeed * 20f);
            }
        }

        // ハンドルを左に回す
        public void TurnLeftHandle()
        {
            if (_shotSpeed <= 0f) return; 
            _shotSpeed -= (_maxShotSpeed / 50f);
            if (_shotSpeed <= 0)
            {
                _isShot = false;
                _shotSpeed = 0f;
            }
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoMachine.SetHandleRotate),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    _shotSpeed * 20f
                );
            }
            else
            {
                SetHandleRotate(_shotSpeed * 20f);
            }
        }

        // パチンコ玉発射
        public void ShotBall()
        {
            if (_stockBallCount > 0)
            {
                if (StrixRoomManager.Instance.IsConnected)
                {
                    this.RpcToRoomOwner(
                        nameof(PachinkoMachine.ShotPachiSphere),
                        (handler) => { },
                        (handler) => { ErrorRPC(handler); },
                        _shotSpeed
                    );
                }
                else
                {
                    ShotPachiSphere(_shotSpeed);
                }
            }
        }

        // パチンコ玉入賞(左打ち保留穴)
        public void OnEnterPrizeHole()
        {
            // if (_keyController == null || _keyController == default) return;

            if (manager.curModeManager.IsPause) return;

            if (manager.state == PachinkoState.NORMAL_MODE)
            {
                if (StrixRoomManager.Instance.IsConnected)
                {
                    // オーナーの筐体で保留生成
                    this.RpcToRoomOwner(
                        nameof(PachinkoMachine.CreateHold),
                        (handler) => { },
                        (handler) => { ErrorRPC(handler); }
                    );
                }
                else
                {
                    // この筐体で保留生成
                    CreateHold();
                }
            }            
        }

        // パチンコ玉入賞(ポイント獲得穴)
        public void OnEnterGetPointHole()
        {
            // if (_keyController == null || _keyController == default) return;

            if (manager.state == PachinkoState.ROUND_MODE)
            {
                if (StrixRoomManager.Instance.IsConnected)
                {
                    this.RpcToRoomOwner(
                        nameof(PachinkoMachine.Point),
                        (handler) => { },
                        (handler) => { ErrorRPC(handler); }
                    );
                }
                else
                {
                    Point();
                }
            }       
        }

        // パチンコ玉入賞(右打ち保留穴)
        public void OnEnterFeverModeHole()
        {
            // if (_keyController == null || _keyController == default) return;

            if (manager.curModeManager.IsPause) return;

            if (manager.state == PachinkoState.FEVER_MODE)
            {
                if (StrixRoomManager.Instance.IsConnected)
                {
                    // オーナーの筐体で保留生成
                    this.RpcToRoomOwner(
                        nameof(PachinkoMachine.CreateHold),
                        (handler) => { },
                        (handler) => { ErrorRPC(handler); }
                    );
                }
                else
                {
                    // この筐体で保留生成
                    CreateHold();
                }
            }
            else if (manager.state == PachinkoState.NORMAL_MODE)
            {
                Debug.Log("左打ちに戻してください！");
            }   
        }

        // パチンコ筐体のボタンを押下
        public void PushButton()
        {
            manager?.PushButton();
        }

        // TODO パチンコ筐体のレバーを引く
        public void PullLever()
        {

        }

        // ゲームエンド
        public void EndGame()
        {
            // キーコントローラ削除
            manager.RemoveKeyController();
            RemoveKeyController();
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoMachine.SetIsPlayFlag),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    false
                );
            }
            else
            {
                SetIsPlayFlag(false);
            }

            SavePointCallback?.Invoke(_stockBallCount);
            _stockBallCount = 0;
            UpdateStockBallViews();
        }

        // パチンコ遊戯選択処理
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClickPachinkoCallback?.Invoke(this);
        }

        // キーコントローラの設定
        public void SetKeyController(InputKeyController keyController)
        {
            _keyController = keyController;
        }

        // キーコントローラ削除
        public void RemoveKeyController()
        {
            _keyController.RemoveKeyDownAction(KeyCode.Return);
            _keyController.RemoveKeyDownAction(KeyCode.Space);
            _keyController = null;
        }

        // ---------- Private関数 ----------

        // パチンコマネージャーの生成
        private Task CreatePachinkoManager()
        {
            if (manager != default) return Task.CompletedTask;

            manager = Instantiate(resource.manager, this.transform);

            // モード選択
            ModeSelectManager modeSelectManager = Instantiate(resource.modeSelectManager, manager.transform);
            GameModeSelectPanel gameModeSelectPanel = Instantiate(resource.gameModeSelectPanel, panels);
            List<PachinkoModeModel> dummyModelList = new List<PachinkoModeModel>(resource.modeModelList);
            foreach (PachinkoModeModel model in dummyModelList)
            {
                BaseModePanel panel = Instantiate(model.ModePanelPrefab, panels);
                panel.SetPopupParent(cutins.gameObject);
                VideoPlayerPair kvp = panel.GetVideoPlayerPair();
                videoPlayerTable.SetValue(kvp.Key, kvp.Value);
                model.Panel = panel;
            }
            modeSelectManager.SetParameter(dummyModelList, gameModeSelectPanel, resource.modeSelectTime, panels);

            // データカウント
            DataCountManager dataCountManager = Instantiate(resource.dataCountManager, manager.transform);
            DataCountPanel dataCountPanel = Instantiate(resource.dataCountPanel, panels);
            dataCountManager.SetParameter(dataCountPanel);

            // ラウンド
            RoundManager roundManager = Instantiate(resource.roundManager, manager.transform);
            RoundPanel roundPanel = Instantiate(resource.roundPanel, panels);
            roundPanel.SetPopupParent(cutins.gameObject);
            roundManager.SetParameter(roundPanel, resource.roundEndTime, resource.roundPointValue);

            // リザルト
            ResultManager resultManager = Instantiate(resource.resultManager, manager.transform);
            ResultPanel resultPanel = Instantiate(resource.resultPanel, panels);
            resultManager.SetParameter(resultPanel);

            // 動画
            VideoManager videoManager = Instantiate(resource.videoManager, manager.transform);
            videoManager.SetParameter(videoPlayerTable, resource.videoClipTable);

            // パーティクル
            ParticleManager particleManager = Instantiate(resource.particleManager, manager.transform);
            particleManager.SetParameter(particleParent, resource.particleTable);

            // オーディオ
            AudioManager audioManager = Instantiate(resource.audioManager, manager.transform);
            audioManager.SetSeAudioDictionary(resource.seTable);
            audioManager.SetVoiceAudioDictionary(resource.voiceTable);
            audioManager.SetBgmAudioDictionary(resource.bgmTable);
            this.audioManager = audioManager;

            // その他
            GameObject startupPanel = Instantiate(resource.startupPanel, panels);
            manager.startupPanel = startupPanel;

            manager.SetManager(
                modeSelectManager, dataCountManager, roundManager,
                resultManager, videoManager, particleManager, audioManager,
                accessoryManager, lightManager, emissionManager
            );
            return Task.CompletedTask;
        }

        // パチンコ玉の生成
        private void CreatePachiSphere(int count)
        {
            _curShotPachiNumber = 0;
            _sphereDict = new Dictionary<int, PachinkoBall>();
            for (int i = 0; i < count; i++)
            {
                PachinkoBall newBall = Instantiate(_pachiSphere);
                newBall.SetActiveAllClone(false);
                _sphereDict.Add(i, newBall);
            }
        }

        // それぞれ入賞口に処理を設定する
        private void InitializeChuckerCallback()
        {
            foreach (PachiSphereChucker chucker in _chuckerList)
            {
                switch(chucker.state)
                {
                    case StartChuckerState.MAIN_HOLE:
                        chucker.Callback = OnEnterPrizeHole;
                        break;
                    case StartChuckerState.RIGHT_MAIN_HOLE:
                        chucker.Callback = OnEnterFeverModeHole;
                        break;
                    case StartChuckerState.RIGHT_POINT_HOLE:
                        chucker.Callback = OnEnterGetPointHole;
                        break;
                    default:
                        break;
                }
            }
        }

        // TODO MetaPachi ポイント獲得穴のパネルを動かす
        private void MovePanelPointHole(bool isOpen)
        {
            _movePanel_1.gameObject.SetActive(!isOpen);
        }

        // TODO MetaPachi 保留チャージ穴のパネルを動かす
        private void MovePanelHoldHole(bool isOpen)
        {
            _isMovePanel = isOpen;
            if (!_isMovePanel)
            {
                _movePanel_2.gameObject.SetActive(true);
                return;
            }
            async void Move(bool isActive)
            {
                if (!_isMovePanel)
                {
                    _movePanel_2.gameObject.SetActive(true);
                    return;
                }
                _movePanel_2.gameObject.SetActive(isActive);
                await Task.Delay(5000);
                Move(!isActive);
            }
            Move(false);
        }

        // データ表示パネルの生成
        private void CreateDataViewPanel()
        {
            if (_dataViewPanelPrefab == default || _dataViewPanelPrefab == null) return;
            if (_dataViewPanelParent == default || _dataViewPanelParent == null) return;

            DataViewPanel obj = Instantiate(_dataViewPanelPrefab, _dataViewPanelParent);
            obj.Initialize();
            _dataViewPanel = obj;
        }

        // 持ち玉表示パネルの生成
        private void CreateStockBallView()
        {
            if (_stockBallViewPrefab == default || _stockBallViewPrefab == null) return;
            if (_stockBallViewParent == default || _stockBallViewParent == null) return;

            StockBallView obj = Instantiate(_stockBallViewPrefab, _stockBallViewParent);
            obj.Initialize();
            _stockBallView = obj;
        }

        // 持ち玉表示の描画更新
        private void UpdateStockBallViews()
        {
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoMachine.UpdateStockBallView),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    _stockBallCount
                );
            }
            else
            {
                UpdateStockBallView(_stockBallCount);
            }
        }

        // プレイデータの初期化
        private void InitializePlayData()
        {
            if (_data != default) return;
            _data = new PlayDataModel();
            _data.CurMode = PachinkoModeState.NORMAL_MODE;
        }

        // 保留リストから先頭の保留を抽出
        private HoldModel PopHold()
        {
            // 内部データ処理
            HoldModel hold = null;
            if (_data.HoldList.Count > 0)
            {
                hold = _data.HoldList[0];
                _data.HoldList.RemoveAt(0);
            }

            // 描画処理
            if (StrixRoomManager.Instance.IsConnected)
            {
                // 全ての筐体でスロット回転
                this.RpcToAll(
                    nameof(PachinkoMachine.MoveHold),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    _data.HoldList.Count
                );
            }
            else
            {
                // この筐体のみスロット回転
                MoveHold(_data.HoldList.Count);
            }
            return hold;
        }

        // 保留データの更新
        private void UpdateHoldModel(
            bool isChanceUpCurHold, int chanceUpHoldIndex, HoldModel updateHold
        )
        {
            if (isChanceUpCurHold)
            {
                _data.CurHold = updateHold;
                return;
            }
            else
            {
                _data.HoldList[chanceUpHoldIndex] = updateHold;
                return;
            }
        }

        // リザルト表示:最終決戦
        private void ShowResult()
        {

        }

        // RPCエラー処理
        private void ErrorRPC(FailureEventArgs args)
        {
            Debug.Log(args.cause.Message);
            // switch (args.cause.)
            // {

            // }
        }

        // 現モードの当たる確率を100％にする
        private void DebugHitMode()
        {
            manager?.DebugHitMode();
        }

        // ---------- protected関数 ---------

        // 遊戯スタート
        [StrixRpc]
        protected virtual void StartPlayPachiRPC(int usePoint)
        {
            _stockBallCount = usePoint;
            _stockBallView?.UpdateDataView(_stockBallCount);
        }

        // プレイ中フラグ
        [StrixRpc]
        protected virtual void SetIsPlayFlag(bool isPlay)
        {
            IsPlay = isPlay;
        }

        // パチンコ玉の発射
        [StrixRpc]
        protected virtual void ShotPachiSphere(float speed)
        {
            if (!isLocal) return;
            
            // 玉の抽選
            PachinkoBall ball = _sphereDict[_curShotPachiNumber];
            _curShotPachiNumber++;
            if (_curShotPachiNumber >= _createSphereCount) _curShotPachiNumber = 0;

            // 玉の座標などを初期化
            ball.RB.velocity = Vector3.zero;
            ball.transform.position = _createSpherePoint.position;
            ball.transform.rotation = Quaternion.identity;
            ball.transform.LookAt(_shotPos);

            // 発射
            Vector3 direction = ball.transform.forward;
            ball.SetActiveAllClone(true);
            audioManager.PlaySE(PachinkoConst.SE_BALL_SHOT);
            ball.RB.AddForce(direction * speed, ForceMode.Impulse);
            
            _stockBallCount--;
            UpdateStockBallViews();
        }

        // ハンドルの回転
        [StrixRpc]
        protected void SetHandleRotate(float rotate)
        {
            _handle.localRotation = Quaternion.Euler(0f, 0f, rotate);
        }

        // 持ち玉数の表示更新
        [StrixRpc]
        protected virtual void UpdateStockBallView(int stockBallCount)
        {
            _stockBallView?.UpdateDataView(stockBallCount);
        }

        // 入賞(保留)
        [StrixRpc]
        protected virtual void Charge(HoldModel hold, int holdCount)
        {
            manager?.Charge(hold, holdCount);
        }

        // モード選択
        [StrixRpc]
        protected async virtual void SelectMode()
        {
            if (!isLocal) return;

            string modeKey = await manager.GetNextModeKey();
            _data.BeforeMode = _data.CurMode;
            _data.CurMode = manager.modeSelectManager.GetModeStateByModeName(modeKey);
            HandOverRemainHold();
            bool isFeverMode = manager.state == PachinkoState.FEVER_MODE;
            _data.RotateCount = 0;
            if (StrixRoomManager.Instance.IsConnected)
            {
                // 全ての筐体にモードチェンジを指示
                this.RpcToAll(
                    nameof(PachinkoMachine.ChangeMode),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    modeKey, _data, isFeverMode
                );
            }
            else
            {
                // この筐体でモードチェンジ
                ChangeMode(modeKey, _data, isFeverMode);
            }
        }

        // モードチェンジ
        [StrixRpc]
        protected async virtual void ChangeMode(
            string nextModeKey, PlayDataModel data, bool isFeverMode
        )
        {
            _dataViewPanel.UpdateDataView(data);
            await manager.NextMode(nextModeKey, data.CurHold, data.HoldList, isFeverMode);

            if (isFeverMode) MovePanelHoldHole(true);

            if (!isLocal) return;

            manager.curModeManager.UpdateHoldModelCallback = UpdateHoldModelRPC;

            if (manager.curModeManager.State == PachinkoModeState.FINAL_BATTLE)
            {
                ((Pachinko.FinalBattle.Manager.FinalBattleManager)manager.curModeManager)
                    .ChargeEndCallback = StartRotate;
            }

            // 自動で保留を変動させるモードの場合
            _isRotate = false;
            if (manager.CheckAutoRotateMode()) CheckRotate();
        }

        // 回転命令
        [StrixRpc]
        protected virtual void CheckRotate()
        {
            if (_isRotate) return;
            if (_data.HoldList.Count <= 0) return;
            if (!isLocal) return;
            StartRotate();
        }

        // 回転開始
        [StrixRpc]
        protected virtual void StartRotate()
        {
            if (_isRotate) return;

            _isRotate = true;

            HoldModel hold = PopHold();
            if (hold == null) return;
            _data.CurHold = hold;
            _data.RotateCount++;
            if (StrixRoomManager.Instance.IsConnected)
            {
                // 全ての筐体でスロット回転
                this.RpcToAll(
                    nameof(PachinkoMachine.StartSlotRotate),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    _data.CurHold
                );
            }
            else
            {
                // この筐体のみスロット回転
                StartSlotRotate(_data.CurHold);
            }
        }

        // 保留生成
        [StrixRpc]
        protected virtual void CreateHold()
        {
            // 保留の生成
            HoldModel hold = manager.CreateHold(_data.HoldList);
            if (hold == null || hold == default) return;
            if (manager.IsCurModePause()) return;
            if (_data.HoldList.Count < manager.GetMaxHoldCount())
            {   
                _data.HoldList.Add(hold);
            }

            if (StrixRoomManager.Instance.IsConnected)
            {
                // 全ての筐体で保留描画
                this.RpcToAll(
                    nameof(PachinkoMachine.Charge),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    hold, _data.HoldList.Count
                );
            }
            else
            {
                // この筐体のみ保留を描画
                Charge(hold, _data.HoldList.Count);
            }

            if (!manager.CheckAutoRotateMode()) return;
            if (_isRotate) return;
            if (isLocal) CheckRotate();
        }

        // 当たり保留の引継ぎ
        [StrixRpc]
        protected virtual void HandOverRemainHold()
        {
            if (_data.HoldList.Count > 0)
            {
                List<HoldModel> newHoldList = new List<HoldModel>();
                if (_data.BeforeMode == PachinkoModeState.NORMAL_MODE)
                {
                    // 当たり保留のみ抽出
                    foreach (HoldModel hold in _data.HoldList)
                    {
                        if (hold.IsHit) newHoldList.Add(hold);
                    }
                }
                else
                {
                    newHoldList = _data.HoldList;
                }
                
                // 現在のモード用の保留へ変換
                _data.HoldList = manager?.ConvertRemainHold(newHoldList);
            }
        }

        // 保留変動開始
        [StrixRpc]
        protected virtual void MoveHold(int holdCount)
        {
            manager?.MoveHold(holdCount);
        }

        // 保留状態変化
        [StrixRpc]
        protected virtual void UpdateHoldModelRPC(bool isChanceUpCurHold, int chanceUpHoldIndex, HoldModel updateHold)
        {
            if (StrixRoomManager.Instance.IsConnected)
            {
                // オーナーの筐体で保留生成
                this.RpcToRoomOwner(
                    nameof(PachinkoMachine.UpdateHoldModel),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    isChanceUpCurHold, chanceUpHoldIndex, updateHold
                );
            }
            else
            {
                // この筐体で保留生成
                UpdateHoldModel(isChanceUpCurHold, chanceUpHoldIndex, updateHold);
            }
        }

        // スロット回転
        [StrixRpc]
        protected virtual void StartSlotRotate(HoldModel hold)
        {
            if (hold == null) return;
            manager?.StartRotate(hold);

            if (isLocal) CreateRotateDirect();
        }

        // スロット回転中演出の生成
        [StrixRpc]
        protected virtual void CreateRotateDirect()
        {
            // 演出を生成して再生
            DirectionModel direct = manager?.CreateDirect(_data.CurHold, _data.HoldList);
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoMachine.RotateDirect),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    _data, _data.CurHold, direct
                );
            }
            else
            {
                RotateDirect(_data, _data.CurHold, direct);
            }
        }        

        // スロット回転中演出
        [StrixRpc]
        protected virtual void RotateDirect(PlayDataModel data, HoldModel hold, DirectionModel direct)
        {
            if (_isSkipDirect)
            {
                StopRotate(data, hold);
            }
            else
            {
                manager?.ShowDirect(direct, () => {
                    StopRotate(data, hold);
                });
            }
        }

        // スロット停止
        [StrixRpc]
        protected virtual void StopRotate(PlayDataModel data, HoldModel hold)
        {
            manager?.StopAllRotate(true);
            manager?.StopAllRotateAction(data.RotateCount);

            // データ表示パネルの描画
            _dataViewPanel.UpdateDataView(data);

            // 大当たりの場合
            if (hold.IsHit)
            {
                MovePanelHoldHole(false);
                Win(data, hold);
                return;
            }

            // 敗北の場合
            else if 
            (
                manager.curModeManager.GetEndRotateCount() > 0 &&
                manager.curModeManager.GetEndRotateCount() <= _data.RotateCount
            )
            {
                MovePanelHoldHole(false);
                Lose(data);
                return;
            }

            Miss(data);
        }

        // 勝利(大当たり)
        [StrixRpc]
        protected async virtual void Win(PlayDataModel data, HoldModel hold)
        {
            // 演出再生
            await manager?.WinAction(data);

            _dataViewPanel.UpdateDataView(data);

            if (isLocal)
            {
                manager.roundManager.EndRoundCallback = () => {
                    EndRoundCallback(false);
                };
                manager.roundManager.BreakEndRoundCallback = () => {
                    EndRoundCallback(true);
                };

                // 回転数などのデータを整理
                _data.TotalHitCount++;
                _data.FeverCount++;
                for (int i = _data.HistoryCountArray.Length - 1; i > 0; i--)
                {
                    _data.HistoryCountArray[i] = _data.HistoryCountArray[i - 1];
                }
                _data.HistoryCountArray[0] = _data.RotateCount;

                // ラウンドスタート
                int roundCount = manager.GetRoundCount(data);
                if (StrixRoomManager.Instance.IsConnected)
                {
                    this.RpcToAll(
                        nameof(PachinkoMachine.StartRound),
                        (handler) => { },
                        (handler) => { ErrorRPC(handler); },
                        roundCount, _data
                    );
                }
                else
                {
                    StartRound(roundCount, _data);
                }
            }
        }

        // 敗北
        [StrixRpc]
        protected virtual async void Lose(PlayDataModel data)
        {
            // 演出再生
            await manager.LoseAction(data);

            _dataViewPanel.UpdateDataView(data);

            if (isLocal)
            {
                // リザルト表示
                if (StrixRoomManager.Instance.IsConnected)
                {
                    this.RpcToAll(
                        nameof(PachinkoMachine.ShowResult),
                        (handler) => { },
                        (handler) => { ErrorRPC(handler); },
                        data
                    );
                }
                else
                {
                    ShowResult(data);
                }
            }
        }

        // はずれ
        [StrixRpc]
        protected virtual async void Miss(PlayDataModel data)
        {
            // 演出再生
            await manager.MissAction(data);

            _dataViewPanel.UpdateDataView(data);

            if (isLocal)
            {
                _isRotate = false;
                CheckRotate();
            }
        }
        
        // ラウンドスタート
        [StrixRpc]
        protected virtual void StartRound(int roundCount, PlayDataModel data)
        {
            _isRotate = false;
            _dataViewPanel.UpdateDataView(data);
            MovePanelPointHole(true);
            manager.StartRound(roundCount);
        }

        // ラウンド入賞(ポイント獲得)
        [StrixRpc]
        protected virtual void Point()
        {
            if (isLocal)
            {
                manager.roundManager.UpdateGetPoint();
                _stockBallCount += manager.roundManager.GetOnePrizePoint();
                UpdateStockBallViews();
                _data.CurPoint = manager.roundManager.GetPoint();
                _data.TotalPoint = manager.roundManager.GetTotalPoint();
                _data.CurRound = manager.roundManager.GetCurRound();
                if (StrixRoomManager.Instance.IsConnected)
                {
                    this.RpcToAll(
                        nameof(PachinkoMachine.UpdatePoint),
                        (handler) => { },
                        (handler) => { ErrorRPC(handler); },
                        _data
                    );
                }
                else
                {
                    UpdatePoint(_data);
                }
            }
        }

        // ポイント描画
        [StrixRpc]
        protected virtual void UpdatePoint(PlayDataModel data)
        {
            manager.roundManager.UpdateView(data);
        }

        // ラウンド終了時コールバック
        protected virtual void EndRoundCallback(bool isEndFever)
        {
            if (StrixRoomManager.Instance.IsConnected)
            {
                this.RpcToAll(
                    nameof(PachinkoMachine.EndRound),
                    (handler) => { },
                    (handler) => { ErrorRPC(handler); },
                    isEndFever
                );
            }
            else
            {
                EndRound(isEndFever);
            }
        }

        // ラウンドエンド
        [StrixRpc]
        protected virtual void EndRound(bool isEndFever)
        {
            MovePanelPointHole(false);
            if (isEndFever)
            {
                manager.ReturnNormalMode();
                if (isLocal)
                {
                    _data.FeverCount = 0;
                    _data.CurRound = 0;
                    _data.TotalPoint = 0;
                    _data.HoldList = new List<HoldModel>();
                }
            }
            else
            {
                manager.EndRound();
            }

            if (!isLocal) return;
            SelectMode();
        }

        // リザルト表示
        [StrixRpc]
        protected virtual async void ShowResult(PlayDataModel data)
        {
            manager.ShowResult(data);
            await Task.Delay(3000);

            // TODO Pachi 残保留消化

            // TODO Pachi 復活

            // 非表示にし左打ちへ
            manager.state = PachinkoState.NORMAL_MODE;
            manager.ReturnNormalMode();
            if (isLocal) 
            {
                _data.FeverCount = 0;
                _data.CurRound = 0;
                _data.TotalPoint = 0;
                _data.HoldList = new List<HoldModel>();
                SelectMode();
            }
        }
    }
}

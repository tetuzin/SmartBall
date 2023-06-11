using System;
using System.Collections.Generic;
using UnityEngine;

using Pachinko.Const;
using Pachinko.Dict;
using Pachinko.GameMode.Base.Manager;
using Pachinko.GameMode.Base.Panel;

using Pachinko.Slot;
using Pachinko.HoldView;

namespace Pachinko.Model
{
    [System.Serializable]
    public class PachinkoModel
    {

    }

    [System.Serializable]
    public class PlayDataModel
    {
        // 停止図柄
        [SerializeField] private List<int> _stopDesign = new List<int>();
        // 変動中
        [SerializeField] private bool _isRotate;
        // 変動中保留
        [SerializeField] private HoldModel _curHold;
        // 保留リスト
        [SerializeField] private List<HoldModel> _holdList  = new List<HoldModel>();
        // フィーバー数
        [SerializeField] private int _feverCount;
        // 総大当たり数
        [SerializeField] private int _totalHitCount;
        // 回転数の履歴
        [SerializeField] private int[] _historyCountArray = new int[5];
        // 獲得ポイント数
        [SerializeField] private int _curPoint;
        // 合計獲得ポイント数
        [SerializeField] private int _totalPoint;
        // 現在のラウンド数
        [SerializeField] private int _curRound;
        // 回転数
        [SerializeField] private int _rotateCount;
        // プレイ中のモード
        [SerializeField] private PachinkoModeState _curMode = PachinkoModeState.NONE;
        // 前回のモード
        [SerializeField] private PachinkoModeState _beforeMode = PachinkoModeState.NONE;

        public List<int> StopDesign
        {
            get { return _stopDesign; }
            set { _stopDesign = value; }
        }
        public bool IsRotate
        {
            get { return _isRotate; }
            set { _isRotate = value; }
        }
        public HoldModel CurHold
        {
            get { return _curHold; }
            set { _curHold = value; }
        }
        public List<HoldModel> HoldList
        {
            get { return _holdList; }
            set { _holdList = value; }
        }
        public int FeverCount
        {
            get { return _feverCount; }
            set { _feverCount = value; }
        }
        public int TotalHitCount
        {
            get { return _totalHitCount; }
            set { _totalHitCount = value; }
        }
        public int[] HistoryCountArray
        {
            get { return _historyCountArray; }
            set { _historyCountArray = value; }
        }
        public int CurPoint
        {
            get { return _curPoint; }
            set { _curPoint = value; }
        }
        public int TotalPoint
        {
            get { return _totalPoint; }
            set { _totalPoint = value; }
        }
        public int CurRound
        {
            get { return _curRound; }
            set { _curRound = value; }
        }
        public int RotateCount
        {
            get { return _rotateCount; }
            set { _rotateCount = value; }
        }
        public PachinkoModeState CurMode
        {
            get { return _curMode; }
            set { _curMode = value; }
        }
        public PachinkoModeState BeforeMode
        {
            get { return _beforeMode; }
            set { _beforeMode = value; }
        }
    }

    [System.Serializable]
    public class HoldModel
    {
        // あたり
        [SerializeField] private bool _isHit;
        // 種類ID
        [SerializeField] private int _holdId;
        // 値
        [SerializeField] private int _value;
        // スロット値
        [SerializeField] private int[] _slotValue;
        // スロット状態
        [SerializeField] private ValueState _valueState;
        // リーチ種
        [SerializeField] private string _reachKey;
        // リーチ入り種別
        [SerializeField] private ReachDirectionState _reachDirectionState;
        // 大当たり確率（期待度・信頼度）
        [SerializeField] private int _hitRate;

        public bool IsHit
        {
            get { return _isHit; }
            set { _isHit = value; }
        }
        public int HoldId
        {
            get { return _holdId; }
            set { _holdId = value; }
        }
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public int[] SlotValue
        {
            get { return _slotValue; }
            set { _slotValue = value; }
        }
        public ValueState ValueState
        {
            get { return _valueState; }
            set { _valueState = value; }
        }
        public string ReachKey
        {
            get { return _reachKey; }
            set { _reachKey = value; }
        }
        public ReachDirectionState ReachDirectionState
        {
            get { return _reachDirectionState; }
            set { _reachDirectionState = value; }
        }
        public int HitRate
        {
            get { return _hitRate; }
            set { _hitRate = value; }
        }
    }

    // 演出再生モデル
    [System.Serializable]
    public class DirectionModel
    {
        // 当たりフラグ
        [SerializeField] private bool _isHit = false;
        // 高速消化フラグ
        [SerializeField] private bool _isHighSpeedRotate = false;
        // 先バレフラグ
        [SerializeField] private bool _isFirstFindOut = false;
        // ７テンフラグ
        [SerializeField] private bool _isSevenReach = false;
        // 表示パーティクルキー
        [SerializeField] private string _particleKey = null;
        // 表示パーティクルカラー
        [SerializeField] private Color _particleColor = default;
        // 保留変化フラグ
        [SerializeField] private bool _isChanceUpHold = false;
        // 変動保留の保留変化
        [SerializeField] private bool _isChanceUpCurHold = false;
        // 保留変化する保留の番号
        [SerializeField] private int _chanceUpHoldIndex = 0;
        // 保留変化先の保留種
        [SerializeField] private HoldIconState _holdIconState = HoldIconState.NORMAL;
        // 表示カットイン
        [SerializeField] private string _showCutin = null;
        // 表示カットインのレアリティ
        [SerializeField] private RarityConst _cutinRarity = RarityConst.NONE;
        // リーチフラグ
        [SerializeField] private bool _isReach = false;
        // 再生リーチ動画名
        [SerializeField] private string _reachMovieKey = null;
        // リーチ中役物動作フラグ
        [SerializeField] private bool _isReachAccessory = false;
        // 疑似連フラグ
        [SerializeField] private bool _isPseudo = false;
        // 疑似連状態
        [SerializeField] private ReachDirectionState _reachState = ReachDirectionState.NONE;
        // 疑似連用演出データ
        [SerializeField] private DirectionModel _pseudoData = null;
        // 疑似連用スロット値
        [SerializeField] private int[] _pseudoSlotValue = new int[3];
        // その他データ
        [SerializeField] private Dictionary<string, object> _propertyes = new Dictionary<string, object>();

        public bool IsHit
        {
            get { return _isHit; }
            set { _isHit = value; }
        }
        public bool IsHighSpeedRotate
        {
            get { return _isHighSpeedRotate; }
            set { _isHighSpeedRotate = value; }
        }
        public bool IsFirstFindOut
        {
            get { return _isFirstFindOut; }
            set { _isFirstFindOut = value; }
        }
        public bool IsSevenReach
        {
            get { return _isSevenReach; }
            set { _isSevenReach = value; }
        }
        public string ParticleKey
        {
            get { return _particleKey; }
            set { _particleKey = value; }
        }
        public Color ParticleColor
        {
            get { return _particleColor; }
            set { _particleColor = value; }
        }
        public bool IsChanceUpHold
        {
            get { return _isChanceUpHold; }
            set { _isChanceUpHold = value; }
        }
        public bool IsChanceUpCurHold
        {
            get { return _isChanceUpCurHold; }
            set { _isChanceUpCurHold = value; }
        }
        public int ChanceUpHoldIndex
        {
            get { return _chanceUpHoldIndex; }
            set { _chanceUpHoldIndex = value; }
        }
        public HoldIconState HoldIconState
        {
            get { return _holdIconState; }
            set { _holdIconState = value; }
        }
        public string ShowCutin
        {
            get { return _showCutin; }
            set { _showCutin = value; }
        }
        public RarityConst CutinRarity
        {
            get { return _cutinRarity; }
            set { _cutinRarity = value; }
        }
        public bool IsReach
        {
            get { return _isReach; }
            set { _isReach = value; }
        }
        public string ReachMovieKey
        {
            get { return _reachMovieKey; }
            set { _reachMovieKey = value; }
        }
        public bool IsReachAccessory
        {
            get { return _isReachAccessory; }
            set { _isReachAccessory = value; }
        }
        public bool IsPseudo
        {
            get { return _isPseudo; }
            set { _isPseudo = value; }
        }
        public ReachDirectionState ReachState
        {
            get { return _reachState; }
            set { _reachState = value; }
        }
        public DirectionModel PseudoData
        {
            get { return _pseudoData; }
            set { _pseudoData = value; }
        }
        public int[] PseudoSlotValue
        {
            get { return _pseudoSlotValue; }
            set { _pseudoSlotValue = value; }
        }
        public Dictionary<string, object> Propertyes
        {
            get { return _propertyes; }
            set { _propertyes = value; }
        }
    }

    // リーチ演出モデル
    [System.Serializable]
    public class ReachDirectionModel
    {
        // 演出名
        [SerializeField] private string _name;
        // リーチ種
        [SerializeField] private List<ReachDirectionState> _stateList = new List<ReachDirectionState>();
        // 大当たり確率（期待度・信頼度）
        [SerializeField] private int _hitRate;
        // 出現確率
        [SerializeField] private int _appearanceRate;
        // 再生動画キー
        [SerializeField] private string _showMovieKey;
        // 演出実行関数
        [SerializeField] private Action _action;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public List<ReachDirectionState> StateList
        {
            get { return _stateList; }
            set { _stateList = value; }
        }
        public int HitRate
        {
            get { return _hitRate; }
            set { _hitRate = value; }
        }
        public int AppearanceRate
        {
            get { return _appearanceRate; }
            set { _appearanceRate = value; }
        }
        public string ShowMovieKey
        {
            get { return _showMovieKey; }
            set { _showMovieKey = value; }
        }
        public Action Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public ReachDirectionState GetRandomState()
        {
            return StateList[new System.Random().Next(StateList.Count)];
        }
    }

    // 予告演出モデル
    [System.Serializable]
    public class NoticeDirectionModel
    {
        // 演出名
        [SerializeField] private string _name;
        // 予告種
        [SerializeField] private List<NoticeDirectionState> _stateList = new List<NoticeDirectionState>();
        // 大当たり確率（期待度・信頼度）
        [SerializeField] private int _hitRate;
        // 演出実行関数
        [SerializeField] private Action _action;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public List<NoticeDirectionState> StateList
        {
            get { return _stateList; }
            set { _stateList = value; }
        }
        public int HitRate
        {
            get { return _hitRate; }
            set { _hitRate = value; }
        }
        public Action Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public NoticeDirectionState GetRandomState()
        {
            return StateList[new System.Random().Next(StateList.Count)];
        }
    }

    // 最終決戦チャージ用アイコンモデル
    [System.Serializable]
    public class FinalBattleChargeIconModel
    {
        // アイコン名
        [SerializeField] private string _name;
        // アイコン画像
        [SerializeField] private Sprite _sprite;
        // フレームカラー
        [SerializeField] private Color _frameColor;
        // レベル
        [SerializeField] private FinalBattleChargeIconState _level;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public Sprite Sprite
        {
            get { return _sprite; }
            set { _sprite = value; }
        }
        public Color FrameColor
        {
            get { return _frameColor; }
            set { _frameColor = value; }
        }
        public FinalBattleChargeIconState Level
        {
            get { return _level; }
            set { _level = value; }
        }
    }

    // モード指定用モデル
    [System.Serializable]
    public class PachinkoModeModel
    {
        // モード名
        [SerializeField] private string _name;
        // Feverモードかどうか
        [SerializeField] private bool _isFever;
        // モード状態
        [SerializeField] private PachinkoModeState _state;
        // 選択条件
        [SerializeField] private ModeSelectConditionState _selectState;
        // Fever数
        [SerializeField] private int _feverNum;
        // モードのマネージャープレハブ
        [SerializeField] private BaseModeManager _managerPrefab;
        // モードのパネルプレハブ
        [SerializeField] private BaseModePanel _panelPrefab;
        // モードのパネル
        [SerializeField] private BaseModePanel _panel;
        // モードのリソースデータ
        [SerializeField] private PachinkoModeResourceModel _resourceModel;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public bool IsFever
        {
            get { return _isFever; }
            set { _isFever = value; }
        }
        public PachinkoModeState PachinkoModeState
        {
            get { return _state; }
            set { _state = value; }
        }
        public ModeSelectConditionState ModeSelectState
        {
            get { return _selectState; }
            set { _selectState = value; }
        }
        public int FeverNum
        {
            get { return _feverNum; }
            set { _feverNum = value; }
        }
        public BaseModeManager ModeManagerPrefab
        {
            get { return _managerPrefab; }
            set { _managerPrefab = value; }
        }
        public BaseModePanel ModePanelPrefab
        {
            get { return _panelPrefab; }
            set { _panelPrefab = value; }
        }
        public BaseModePanel Panel
        {
            get { return _panel; }
            set { _panel = value; }
        }
        public PachinkoModeResourceModel Resource
        {
            get { return _resourceModel; }
            set { _resourceModel = value; }
        }
    }
    [System.Serializable]
    public class PachinkoModeResourceModel
    {
        [Header("基本")]
        [SerializeField, Tooltip("当たり確率")] private int _hitProb = 30;
        [SerializeField, Tooltip("テンパイ確率(分母)")] private int _reachProb = 10;
        [SerializeField, Tooltip("終了回転回数")] private int _endRotateCount = -1;

        [Header("スロット")]
        [SerializeField, Tooltip("スロットマネージャー")] private SlotManager _slotManager = new SlotManager();
        [SerializeField, Tooltip("リール数")] private int _reelCount = 3;
        [SerializeField, Tooltip("図柄数")] private int _designCount = 7;
        [SerializeField, Tooltip("疑似連発生図柄")] private int _pseudoIndex = -1;
        [SerializeField, Tooltip("リーチしない図柄")] private List<int> _nonReachIndex = new List<int>();
        [SerializeField, Tooltip("当たらない図柄")] private List<int> _nonHitIndex = new List<int>();

        [Header("保留")] 
        [SerializeField, Tooltip("保留表示マネージャー")] private HoldViewManager _holdViewManager = new HoldViewManager();
        [SerializeField, Tooltip("保留アイコンオブジェクト")] private HoldIconTable _holdIconObjects = default;
        [SerializeField, Tooltip("保持できる保留数")] private int _maxHoldCount = 4;
        [SerializeField, Tooltip("残保留を含む")] private bool _isRemainHold = true;
        [SerializeField, Tooltip("保留アニメーション")] private bool _isHoldAnim = true;

        [Header("演出")] 
        [SerializeField, Tooltip("背景動画")] private string _bgMovieKey = default;
        [SerializeField, Tooltip("リーチ演出")] private ReachDirectionTable _reachDirectionTable = new ReachDirectionTable();
        [SerializeField, Tooltip("予告演出")] private NoticeDirectionTable _noticeDirectionTable = new NoticeDirectionTable();

        [Header("最終決戦モード")] 
        [SerializeField, Tooltip("最終決戦のチャージ時間")] private float _maxChargeTime = default;
        [SerializeField, Tooltip("最終決戦アイコンのテーブル")] private FinalBattleChargeIconModelTable _finalBattleIconTable = default;

        public int HitProb
        {
            get { return _hitProb; }
            set { _hitProb = value; }
        }
        public int ReachProb
        {
            get { return _reachProb; }
            set { _reachProb = value; }
        }
        public int EndRotateCount
        {
            get { return _endRotateCount; }
            set { _endRotateCount = value; }
        }
        public SlotManager SlotManager
        {
            get { return _slotManager; }
            set { _slotManager = value; }
        }
        public int ReelCount
        {
            get { return _reelCount; }
            set { _reelCount = value; }
        }
        public int DesignCount
        {
            get { return _designCount; }
            set { _designCount = value; }
        }
        public int PseudoIndex
        {
            get { return _pseudoIndex; }
            set { _pseudoIndex = value; }
        }
        public List<int> NonReachIndex
        {
            get { return _nonReachIndex; }
            set { _nonReachIndex = value; }
        }
        public List<int> NonHitIndex
        {
            get { return _nonHitIndex; }
            set { _nonHitIndex = value; }
        }
        public HoldViewManager HoldViewManager
        {
            get { return _holdViewManager; }
            set { _holdViewManager = value; }
        }
        public HoldIconTable HoldIconObjects
        {
            get { return _holdIconObjects; }
            set { _holdIconObjects = value; }
        }
        public int MaxHoldCount
        {
            get { return _maxHoldCount; }
            set { _maxHoldCount = value; }
        }
        public bool IsRemainHold
        {
            get { return _isRemainHold; }
            set { _isRemainHold = value; }
        }
        public bool IsHoldAnim
        {
            get { return _isHoldAnim; }
            set { _isHoldAnim = value; }
        }
        public string BGMovieKey
        {
            get { return _bgMovieKey; }
            set { _bgMovieKey = value; }
        }
        public ReachDirectionTable ReachDirectionTable
        {
            get { return _reachDirectionTable; }
            set { _reachDirectionTable = value; }
        }
        public NoticeDirectionTable NoticeDirectionTable
        {
            get { return _noticeDirectionTable; }
            set { _noticeDirectionTable = value; }
        }
        public float MaxChargeTime
        {
            get { return _maxChargeTime; }
            set { _maxChargeTime = value; }
        }
        public FinalBattleChargeIconModelTable FinalBattleIconTable
        {
            get { return _finalBattleIconTable; }
            set { _finalBattleIconTable = value; }
        }
    }
}


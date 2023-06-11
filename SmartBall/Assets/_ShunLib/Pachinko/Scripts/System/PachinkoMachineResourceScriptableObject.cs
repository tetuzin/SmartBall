using System.Collections.Generic;
using UnityEngine;

using ShunLib.Dict;
using ShunLib.Manager.Video;
using ShunLib.Manager.Particle;
using ShunLib.Manager.Audio;

using Pachinko.Model;

// マネージャー
using Pachinko.Manager.Pachinko;
using Pachinko.ModeSelect.Manager;
using Pachinko.Round.Manager;
using Pachinko.Result.Manager;
using Pachinko.DataCount.Manager;

// パネル
using Pachinko.GameMode.Select.Panel;
using Pachinko.DataCount.Panel;
using Pachinko.Round.Panel;
using Pachinko.Result.Panel;

namespace Pachinko.Resource
{
    // パチンコ筐体のリソース用クラス
    [CreateAssetMenu(fileName = "PachinkoMachineResource", menuName = "ScriptableObjects/Pachinko/Machine")]
    public class PachinkoMachineResourceScriptableObject : ScriptableObject
    {
        [Header("基本")]
        [SerializeField, Tooltip("保持できる保留数")] public int maxHoldCount = 4;

        [Header("パチンコマネージャー")]
        [SerializeField] public PachinkoManager manager = new PachinkoManager();
        
        [Header("モード")] 
        [SerializeField, Tooltip("ゲームモード選択マネージャー")] public ModeSelectManager modeSelectManager = new ModeSelectManager();
        [SerializeField, Tooltip("左打ち時ステージチェンジを行う合計回転数")] public int maxStageChangeCount = 20;
        [SerializeField, Tooltip("各ゲームモードのモデル")] public List<PachinkoModeModel> modeModelList = new List<PachinkoModeModel>();
        [SerializeField, Tooltip("ゲームモード選択パネル")] public GameModeSelectPanel gameModeSelectPanel = default;
        [SerializeField, Tooltip("モード選択時間")] public float modeSelectTime = 15f;


        [Header("データカウント")] 
        [SerializeField, Tooltip("データカウントマネージャー")] public DataCountManager dataCountManager = new DataCountManager();
        [SerializeField, Tooltip("データカウントパネル")] public DataCountPanel dataCountPanel = default;


        [Header("ラウンド")] 
        [SerializeField, Tooltip("ラウンドマネージャー")] public RoundManager roundManager = new RoundManager();
        [SerializeField, Tooltip("ラウンドパネル")] public RoundPanel roundPanel = default;
        [SerializeField, Tooltip("1ラウンド強制終了時間")] public float roundEndTime = 15f;
        [SerializeField, Tooltip("ラウンド時、一発で得られるポイント量")] public int roundPointValue = 15;


        [Header("リザルト")] 
        [SerializeField, Tooltip("リザルトマネージャー")] public ResultManager resultManager = new ResultManager();
        [SerializeField, Tooltip("リザルトパネル")] public ResultPanel resultPanel = default;


        [Header("動画")] 
        [SerializeField, Tooltip("動画再生マネージャー")] public VideoManager videoManager = new VideoManager();
        [SerializeField, Tooltip("動画リスト")] public VideoClipTable videoClipTable = new VideoClipTable();


        [Header("パーティクル")]
        [SerializeField, Tooltip("パーティクルマネージャー")] public ParticleManager particleManager = new ParticleManager();
        [SerializeField, Tooltip("パーティクルリスト")] public CommonParticleTable particleTable = new CommonParticleTable();

        [Header("音")]
        [SerializeField, Tooltip("オーディオマネージャー")] public AudioManager audioManager = new AudioManager();
        [SerializeField, Tooltip("SE")] public AudioClipTable seTable = new AudioClipTable();
        [SerializeField, Tooltip("Voice")] public AudioClipTable voiceTable = new AudioClipTable();
        [SerializeField, Tooltip("BGM")] public AudioClipTable bgmTable = new AudioClipTable();


        [Header("その他")] 
        [SerializeField, Tooltip("起動中パネル")] public GameObject startupPanel = default;
    }
}


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Utils.Random;
using ShunLib.Manager.Video;
using ShunLib.Manager.Particle;
using ShunLib.Manager.Audio;

using Pachinko.Const;
using Pachinko.Model;
using Pachinko.Slot;
using Pachinko.HoldView;
using Pachinko.GameMode.Base.Manager;
using Pachinko.FinalBattle.Manager;
using Pachinko.DataCount.Manager;
using Pachinko.Manager.Accessory;
using Pachinko.Manager.Light;
using Pachinko.Manager.Emission;
using Pachinko.GameMode.Select.Panel;

namespace Pachinko.ModeSelect.Manager
{
    public class ModeSelectManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("各ゲームモードのモデル")] 
        [SerializeField] private List<PachinkoModeModel> _modeModelList = default;

        [Header("ゲームモード選択パネル")] 
        [SerializeField] private GameModeSelectPanel _gameModeSelectPanel = default;

        [Header("モード選択時間")] 
        [SerializeField] private float _modeSelectTime = default;

        [Header("パネル配置オブジェクト")]
        [SerializeField] private Transform _panels = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // マネージャー //
        public DataCountManager DataCountManager { get; set; }
        public ParticleManager ParticleManager { get; set; }
        public VideoManager VideoManager { get; set; }
        public AudioManager AudioManager { get; set; }
        public PachinkoAccessoryManager AccessoryManager { get; set; }
        public PachinkoLightManager LightManager { get; set; }
        public PachinkoEmissionManager EmissionManager { get; set; }

        // コールバック //
        public Action ShowResultCallback { get; set; }
        public Action HideResultCallback { get; set; }
        public Action<int> RemainHoldCallback { get; set; }
        public Action PushButtonCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private Dictionary<string, BaseModeManager> _normalModeDictionary = default;
        private Dictionary<string, BaseModeManager> _feverModeDictionary = default;

        private BaseModeManager _curModeManager = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _curModeManager = null;
            _gameModeSelectPanel.Initialize();
            _gameModeSelectPanel.Hide();
            InitializeGameMode();
        }

        // 各パラメータの設定
        public void SetParameter(
            List<PachinkoModeModel> modeModelList = null,
            GameModeSelectPanel gameModeSelectPanel = null,
            float modeSelectTime = 0f,
            Transform panels = null
        )
        {
            _modeModelList = modeModelList;
            _gameModeSelectPanel = gameModeSelectPanel;
            _modeSelectTime = modeSelectTime;
            _panels = panels;
        }

        // ゲームモード取得
        public async Task<string> SelectMode(PachinkoState state, int feverCount = 0)
        {
            // 右打ち用モード取得
            bool isFever = state == PachinkoState.FEVER_MODE;
            if (isFever && _feverModeDictionary.Count > 0 && _modeModelList.Count > 0)
            {
                PachinkoModeModel nextModeModel = null;

                // Fever数を元に取得
                foreach (PachinkoModeModel model in _modeModelList)
                {
                    if (!model.IsFever) continue;
                    if (model.ModeSelectState != ModeSelectConditionState.FEVER_COUNT) continue;
                    if (model.FeverNum == feverCount)
                    {
                        nextModeModel = model;
                    }
                }

                // TODO MetaPachi Playerに選択してもらう
                if (nextModeModel == null)
                {
                    List<PachinkoModeModel> selectModeList = new List<PachinkoModeModel>();
                    foreach (PachinkoModeModel model in _modeModelList)
                    {
                        if (model.ModeSelectState == ModeSelectConditionState.PLAYER_SELECT)
                        {
                            selectModeList.Add(model);
                        }
                    }
                    if (selectModeList.Count == 1)
                    {
                        nextModeModel = selectModeList[0];
                    }
                    else if (selectModeList.Count > 1)
                    {
                        int nextModeIndex = 0;
                        _gameModeSelectPanel.SetActiveSwitchUI(0);
                        _gameModeSelectPanel.ShowModeSelect(this, _modeSelectTime);
                        await Task.Delay((int)_modeSelectTime * 1000);
                        await Task.Delay(1000);
                        nextModeIndex = _gameModeSelectPanel.GetCurIndex();
                        nextModeModel = selectModeList[nextModeIndex];
                    }
                }

                // モードが見つからなかったとき
                if (nextModeModel == null)
                {
                    foreach (PachinkoModeModel model in _modeModelList)
                    {
                        if (model.IsFever)
                        {
                            nextModeModel = model;
                            break;
                        }
                    }
                    if (nextModeModel == null) nextModeModel = _modeModelList[0];
                }
                return nextModeModel.Name;
            }

            // 左打ち用モード取得
            else
            {
                return SelectNormalMode();
            }
        }

        // 通常モードの取得
        public string SelectNormalMode()
        {
            // モードが設定されていない場合
            if (_normalModeDictionary.Count <= 0) return null;

            // 通常モードの抽選
            int index = RandomUtils.GetRandomValue(_normalModeDictionary.Count);
            string[] keyArray = new string[_normalModeDictionary.Count];
            _normalModeDictionary.Keys.CopyTo(keyArray, 0);
            return keyArray[index];
        }

        // モードの取得
        public BaseModeManager GetBaseModeManager(string key, bool isFever)
        {
            if (isFever)
            {
                return _feverModeDictionary[key];
            }
            else
            {
                return _normalModeDictionary[key];
            }
        }

        // モード名からモード状態を取得
        public PachinkoModeState GetModeStateByModeName(string modeName)
        {
            foreach (PachinkoModeModel model in _modeModelList)
            {
                if (model.Name == modeName) return model.PachinkoModeState;
            }
            return PachinkoModeState.NONE;
        }

        // ---------- Private関数 ----------

        // 各ゲームモードの初期化
        private void InitializeGameMode()
        {
            _normalModeDictionary = new Dictionary<string, BaseModeManager>();
            _feverModeDictionary = new Dictionary<string, BaseModeManager>();

            foreach (PachinkoModeModel model in _modeModelList)
            {
                // ゲームモードマネージャーの生成
                BaseModeManager manager = Instantiate(
                    model.ModeManagerPrefab,
                    Vector3.zero,
                    Quaternion.identity,
                    this.gameObject.transform
                );

                // 各マネージャーの生成と設定
                SlotManager slotManager = Instantiate(model.Resource.SlotManager, manager.transform);
                slotManager.SetParameter(
                    model.Resource.ReelCount, model.Resource.DesignCount, model.Resource.PseudoIndex,
                    model.Resource.NonReachIndex, model.Resource.NonHitIndex
                );
                HoldViewManager holdViewManager = Instantiate(model.Resource.HoldViewManager, manager.transform);
                holdViewManager.SetParameter(
                    model.Resource.HoldIconObjects, model.Resource.IsHoldAnim
                );
                manager.SetParameter(
                    slotManager, holdViewManager, 
                    model.Resource.HitProb, model.Resource.ReachProb, model.Resource.EndRotateCount,
                    model.Resource.MaxHoldCount, model.Resource.IsRemainHold,
                    model.Resource.BGMovieKey,
                    model.Resource.ReachDirectionTable, model.Resource.NoticeDirectionTable
                );

                // ゲームモードマネージャー初期化
                manager.IsFever = model.IsFever;
                manager.State = model.PachinkoModeState;
                manager.DataCountManager = DataCountManager;
                manager.VideoManager = VideoManager;
                manager.ParticleManager = ParticleManager;
                manager.AccessoryManager = AccessoryManager;
                manager.LightManager = LightManager;
                manager.EmissionManager = EmissionManager;
                manager.AudioManager = AudioManager;
                manager.panel = model.Panel;
                manager.ShowResultCallback = ShowResultCallback;
                manager.HideResultCallback = HideResultCallback;
                switch(model.PachinkoModeState)
                {
                    case PachinkoModeState.FINAL_BATTLE:
                        ((FinalBattleManager)manager).SetParameter(
                            model.Resource.MaxChargeTime, model.Resource.FinalBattleIconTable
                        );
                        ((FinalBattleManager)manager).Initialize();
                        break;
                    default:
                        manager.Initialize();
                        break;
                }

                if (model.IsFever) _feverModeDictionary.Add(model.Name, manager);
                else _normalModeDictionary.Add(model.Name, manager);
            }
        }

        // ---------- protected関数 ---------
    }
}



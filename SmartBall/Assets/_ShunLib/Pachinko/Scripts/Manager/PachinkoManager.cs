using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Manager.Video;
using ShunLib.Manager.Audio;
using ShunLib.Manager.Particle;
using ShunLib.Controller.InputKey;

using Pachinko.Const;
using Pachinko.Model;
using Pachinko.Resource;
using Pachinko.ModeSelect.Manager;
using Pachinko.Round.Manager;
using Pachinko.Result.Manager;
using Pachinko.GameMode.Base.Manager;
using Pachinko.DataCount.Manager;
using Pachinko.Manager.Accessory;
using Pachinko.Manager.Light;
using Pachinko.Manager.Emission;

namespace Pachinko.Manager.Pachinko
{
    [System.Serializable]
    public class PachinkoManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("マネージャー")] 
        [SerializeField, Tooltip("ゲームモード選択マネージャー")] public ModeSelectManager modeSelectManager = default;
        [SerializeField, Tooltip("パーティクルマネージャー")] protected ParticleManager _particleManager = default;
        [SerializeField, Tooltip("ラウンドマネージャー")] public RoundManager roundManager = default;
        [SerializeField, Tooltip("オーディオマネージャー")] public AudioManager audioManager = default;
        [SerializeField, Tooltip("リザルトマネージャー")] private ResultManager _resultManager = default;
        [SerializeField, Tooltip("データカウントマネージャー")] public DataCountManager dataCountManager = default;
        [SerializeField, Tooltip("動画再生マネージャー")] private VideoManager _videoManager = default;
        [SerializeField, Tooltip("役物マネージャー")] public PachinkoAccessoryManager accessoryManager = default;
        [SerializeField, Tooltip("ライトマネージャー")] public PachinkoLightManager lightManager = default;
        [SerializeField, Tooltip("エミッションマネージャー")] public PachinkoEmissionManager emissionManager = default;

        [Header("起動中パネル")] 
        [SerializeField] public GameObject startupPanel = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public PachinkoState state { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        public PachinkoMachineResourceScriptableObject _resource = default;
        public BaseModeManager curModeManager = default;
        private InputKeyController _keyController = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public async Task Initialize()
        {
            state = PachinkoState.NONE;
            startupPanel?.SetActive(true);

            // データカウントマネージャー初期化
            dataCountManager.Initialize();

            // オーディオマネージャーの初期化
            await audioManager.Initialize();

            // 動画再生マネージャー初期化
            _videoManager.AudioSource = audioManager.AudioBGM;
            _videoManager.Initialize();

            // パーティクルマネージャー初期化
            _particleManager.Initialize();

            // ゲームモードマネージャー初期化
            modeSelectManager.DataCountManager = dataCountManager;
            modeSelectManager.VideoManager = _videoManager;
            modeSelectManager.ParticleManager = _particleManager;
            modeSelectManager.AudioManager = audioManager;
            modeSelectManager.AccessoryManager = accessoryManager;
            modeSelectManager.LightManager = lightManager;
            modeSelectManager.EmissionManager = emissionManager;
            modeSelectManager.Initialize();

            // ラウンドマネージャー初期化
            roundManager.VideoManager = _videoManager;
            roundManager.AudioManager = audioManager;
            roundManager.Initialize();

            // リザルトマネージャーの初期化
            _resultManager.Initialize();

            // ゲームスタート
            state = PachinkoState.NORMAL_MODE;
            string modeKey = await modeSelectManager.SelectMode(state, 0);
            curModeManager = modeSelectManager.GetBaseModeManager(modeKey, false);
            curModeManager.StartMode(new int[]{0, 0, 0});

            // HACK 画面の描画を待機する
            await Task.Delay(1500);
            startupPanel?.SetActive(false);
        }

        // パチンコリソースの設定
        public void SetResource(PachinkoMachineResourceScriptableObject resource)
        {
            _resource = resource;
        }

        // 各マネージャーの設定
        public void SetManager(
            ModeSelectManager modeSelectManager, DataCountManager dataCountManager, RoundManager roundManager,
            ResultManager resultManager, VideoManager videoManager, ParticleManager particleManager,
            AudioManager audioManager, PachinkoAccessoryManager accessoryManager, PachinkoLightManager lightManager,
            PachinkoEmissionManager emissionManager
        )
        {
            this.modeSelectManager = modeSelectManager;
            this.dataCountManager = dataCountManager;
            this.roundManager = roundManager;
            _resultManager = resultManager;
            _videoManager = videoManager;
            _particleManager = particleManager;
            this.audioManager = audioManager;
            this.accessoryManager = accessoryManager;
            this.lightManager = lightManager;
            this.emissionManager = emissionManager;

        }

        // 保留生成
        public HoldModel CreateHold(List<HoldModel> holdList)
        {
            HoldModel hold = null;
            if (curModeManager != default && curModeManager != null)
            {
                hold = curModeManager.CreateHold(holdList);
            }
            return hold;
        }

        // 保留チャージ＆ラウンド中ポイント取得
        public void Charge(HoldModel hold, int holdCount)
        {
            curModeManager.ChargeHold(hold, holdCount);
        }

        // 現モードの停止状態を返す
        public bool IsCurModePause()
        {
            return curModeManager.IsPause;
        }

        // ラウンド中ポイント取得
        public void Point()
        {
            roundManager.GetPoint();
        }

        // 最大保留数を返す
        public int GetMaxHoldCount()
        {
            return curModeManager.GetMaxHoldCount();
        }

        // 保留リスト先頭の保留を変動させる
        public void MoveHold(int holdCount)
        {
            curModeManager.MoveHold(holdCount);
        }

        // 変動中保留を削除
        public void RemoveCurHold()
        {
            curModeManager.RemoveCurHold();
        }

        // 保留リストを現在のモードの保留へ変換する
        public List<HoldModel> ConvertRemainHold(List<HoldModel> holdList)
        {
            List<HoldModel> newHoldList = new List<HoldModel>();
            string log = "holdList\n";
            for (int i = 0; i < holdList.Count; i++)
            {
                log += "Count:" + i + ":" + holdList[i] + "\n";
            }
            Debug.Log(log);
            newHoldList = curModeManager.CreateRemainHoldList(holdList);
            string log2 = "newHoldList\n";
            for (int i = 0; i < newHoldList.Count; i++)
            {
                log2 += "Count:" + i + ":" + newHoldList[i] + "\n";
            }
            Debug.Log(log2);
            return newHoldList;
        }

        // スロット回転スタート
        public void StartRotate(HoldModel hold)
        {
            if (hold == null) return;
            curModeManager.StartRotate(hold.SlotValue);
            curModeManager.SlotRotateAction(hold.SlotValue);
        }

        // 演出を再生
        public void ShowDirect(DirectionModel direct, Action callback = null)
        {
            curModeManager.ShowDirect(direct, callback);
        }

        // スロット全停止
        public void StopAllRotate(bool isFast = false)
        {
            curModeManager.StopAllRotate(isFast);
        }

        // スロット全停止時の処理
        public void StopAllRotateAction(int rotateCount)
        {
            curModeManager.SlotStopAction(rotateCount);
            RemoveCurHold();
        }

        // 大当たり時の処理
        public async Task WinAction(PlayDataModel data)
        {
            await curModeManager.WinAction(data);
        }

        // 敗北時の処理
        public async Task LoseAction(PlayDataModel data)
        {
            await curModeManager.LoseAction(data);
        }

        // はずれ時の処理
        public async Task MissAction(PlayDataModel data)
        {
            await curModeManager.MissAction(data);
        }

        // 演出を生成して返す
        public DirectionModel CreateDirect(HoldModel hold, List<HoldModel> holdList)
        {
            return curModeManager.CreateDirect(hold, holdList);
        }

        // 次のモードへ遷移
        public Task NextMode(
            string nextModeKey, HoldModel hold ,List<HoldModel> remainHoldList, bool isFeverMode
        )
        {
            // 次のモードへ遷移
            curModeManager.EndMode();
            BaseModeManager nextModeManager = modeSelectManager.GetBaseModeManager(
                nextModeKey, isFeverMode
            );
            int[] slotValue = hold == null ? new int[3]{0, 0, 0} : hold.SlotValue;
            nextModeManager.StartMode(slotValue);
            nextModeManager.SetRemainHold(remainHoldList);
            curModeManager = nextModeManager;
            _resultManager.HideResult();
            return Task.CompletedTask;
        }

        // 次のモード名を取得する
        public async Task<string> GetNextModeKey()
        {
            string modeKey = await modeSelectManager.SelectMode(state, roundManager.FeverCount);
            Debug.Log("次のモード[" + modeKey + "]");
            return modeKey;
        }

        // 現モードのStateを返す
        public PachinkoModeState GetModeState()
        {
            return curModeManager.State;
        }

        // 貯めた保留が自動で変動開始するモードかどうか返す
        public bool CheckAutoRotateMode()
        {
            if (PachinkoModeState.FINAL_BATTLE == GetModeState())
            {
                return false;
            }
            return true;
        }

        // ラウンド数を返す
        public int GetRoundCount(PlayDataModel data)
        {
            return curModeManager.GetRoundCount(data);
        }

        // ラウンド開始
        public void StartRound(int roundCount)
        {
            state = PachinkoState.ROUND_MODE;
            dataCountManager.Hide();
            _resultManager.HideResult();
            roundManager.StartRound(roundCount);
            curModeManager.Hide();
        }

        // ラウンド終了
        public void EndRound()
        {
            state = PachinkoState.FEVER_MODE;
            roundManager.EndRound();
            dataCountManager.Show();
            curModeManager.Show();
        }

        // 右打ちを終了し通常モードへ戻る
        public void ReturnNormalMode()
        {
            state = PachinkoState.NORMAL_MODE;
            roundManager.EndRound();
            dataCountManager.Show();
            curModeManager.Show();
            curModeManager.ReturnNormalAction();
            roundManager.FeverCount = 0;
            roundManager.TotalPoint = 0;
        }

        // リザルト表示
        public void ShowResult(PlayDataModel data)
        {
            _resultManager.ShowResult(
                feverCount: data.FeverCount,
                totalPoint: data.TotalPoint
            );
        }

        // ボタン押下時処理
        public void PushButton()
        {
            
        }

        // キーコントローラの設定（主にデバッグ用）
        public void SetKeyController(InputKeyController keyController)
        {
            _keyController = keyController;
            _keyController.AddKeyDownAction(KeyCode.S, DebugRoundSkip);
        }

        // キーコントローラの削除
        public void RemoveKeyController()
        {
            _keyController.RemoveKeyDownAction(KeyCode.B);
            _keyController.RemoveKeyDownAction(KeyCode.S);
        }

        // 現モードの当たる確率を100％にする
        public void DebugHitMode()
        {
            curModeManager?.DebugHitMode();
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------

        // ラウンド消化のスキップ
        private void DebugRoundSkip()
        {
            roundManager.NextRound();
        }
    }
}

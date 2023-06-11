using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Utils.Random;
using ShunLib.Manager.Particle;
using ShunLib.Manager.Video;
using ShunLib.Manager.Audio;
using ShunLib.Particle;

using Pachinko.Dict;
using Pachinko.Const;
using Pachinko.Model;
using Pachinko.Utils;
using Pachinko.GameMode.Base.Panel;

using Pachinko.Slot;

using Pachinko.DataCount.Manager;
using Pachinko.Manager.Accessory;
using Pachinko.Manager.Light;
using Pachinko.Manager.Emission;

using Pachinko.HoldView;
using Pachinko.HoldView.Icon;
using Pachinko.Controller.ReachDirection;

namespace Pachinko.GameMode.Base.Manager
{
    public class BaseModeManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("マネージャー")] 
        [SerializeField, Tooltip("スロットマネージャー")] protected SlotManager _slotManager = default;
        [SerializeField, Tooltip("保留表示マネージャー")] protected HoldViewManager _holdViewManager = default;

        [Header("当たり確率")] 
        [SerializeField] protected int _hitProb = default;

        [Header("テンパイ確率(分母)")]
        [SerializeField] protected int _reachProb = default;

        [Header("終了回転回数")]
        [SerializeField] protected int _endRotateCount = default;

        [Header("保持できる保留数")]
        [SerializeField] protected int _maxHoldCount = default;

        [Header("残保留を含む")] 
        [SerializeField] protected bool _isRemainHold = default;

        [Header("演出")] 
        [SerializeField, Tooltip("背景動画")] protected string _bgMovieKey = default;
        [SerializeField, Tooltip("リーチ演出")] protected ReachDirectionTable _reachDirectionTable = default;
        [SerializeField, Tooltip("予告演出")] protected NoticeDirectionTable _noticeDirectionTable = default;

        [Header("デバッグ用")] 
        [SerializeField, Tooltip("当たる確率100%")] protected bool _isConfirmHit = false;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public bool IsPause
        {
            get { return _isPause; }
            set
            {
                _isPause = value;
                Active(!_isPause);
            }
        }

        public bool IsFever { get; set; }

        public PachinkoModeState State { get; set; }

        // モードパネル
        public BaseModePanel panel { get; set; }

        // データカウントマネージャー
        public DataCountManager DataCountManager { get; set; }
        // 動画再生マネージャー
        public VideoManager VideoManager { get; set; }
        // パーティクルマネージャー
        public ParticleManager ParticleManager { get; set; }
        // オーディオマネージャー
        public AudioManager AudioManager { get; set; }
        // 役物マネージャー
        public PachinkoAccessoryManager AccessoryManager { get; set; }
        // ライトマネージャー
        public PachinkoLightManager LightManager { get; set; }
        // エミッションマネージャー
        public PachinkoEmissionManager EmissionManager { get; set; }
        // リザルト表示コールバック
        public Action ShowResultCallback { get; set; }
        // リザルト非表示コールバック
        public Action HideResultCallback { get; set; }
        // 保留更新コールバック
        public Action<bool, int, HoldModel> UpdateHoldModelCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // 活性フラグ
        private bool _isPause = default;

        // 変動フラグ
        private bool _isRotate = default;

        // リーチ演出コントローラ
        protected ReachDirectionController _reachDirectionCtrl = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public virtual void Initialize()
        {
            _reachDirectionCtrl = new ReachDirectionController();
            _reachDirectionCtrl.Initialize(_reachDirectionTable);

            panel.Initialize();
            panel.Hide();

            // パーティクルマネージャー初期化
            InitializeParticleManager();

            // スロットマネージャー初期化
            InitializeSlotManager();

            // 保留表示マネージャー初期化
            InitializeHoldViewManager();
        }

        // 各パラメータの設定
        public void SetParameter(
            SlotManager slotManager = null, HoldViewManager holdViewManager = null,
            int hitProb = 30, int reachProb = 10, int endRotateCount = -1,
            int maxHoldCount = 4, bool isRemainHold = true,
            string bgMovieKey = "",
            ReachDirectionTable reachDirectionTable = null, NoticeDirectionTable noticeDirectionTable = null
        )
        {
            _slotManager = slotManager;
            _holdViewManager = holdViewManager;
            _hitProb = hitProb;
            _reachProb = reachProb;
            _endRotateCount = endRotateCount;
            _maxHoldCount = maxHoldCount;
            _isRemainHold = isRemainHold;
            _bgMovieKey = bgMovieKey;
            _reachDirectionTable = reachDirectionTable;
            _noticeDirectionTable = noticeDirectionTable;
        }

        // 表示
        public void Show()
        {
            panel.Show();
        }

        // 非表示
        public void Hide()
        {
            if (ParticleManager != default)
            {
                ParticleManager.HideAllParticle();
            }
            panel.Hide();
            IsPause = true;
        }

        // モードスタート
        public virtual void StartMode(int[] slotValue)
        {
            IsPause = true;
            _isConfirmHit = false;
            _slotManager.SlotMain.RotateCount = 0;
            if (_bgMovieKey != default && _bgMovieKey != "")
            {
                VideoManager.PlayLoopVideoClip(PachinkoUIConst.BACK_GOURND_PLAYER, _bgMovieKey);
            }
            _slotManager.SetSlotValue(slotValue);
            panel.SetTotalRotateCount("0 G");
            DataCountManager?.SlotViewShowValue(slotValue);
            Show();
            IsPause = false;
        }

        // モードエンド
        public virtual void EndMode()
        {
            IsPause = true;
            DataCountManager?.SetFeverHoldCount("0");
            DataCountManager?.SetNormalHoldCount("0");
            Hide();
        }

        // 停止中の図柄を返す
        public int[] GetSlotValue()
        {
            return _slotManager.ValueArray;
        }

        // 終了回転回数を返す
        public virtual int GetEndRotateCount()
        {
            return _endRotateCount;
        }

        // 最大保留数を返す
        public virtual int GetMaxHoldCount()
        {
            return _maxHoldCount;
        }

        // 当たる確率を返す
        public int GetHitProb()
        {
            if (_isConfirmHit)
            {
                _isConfirmHit = false;
                return 1;
            }
            else
            {
                return _hitProb;
            }
        }

        // 保留生成
        public virtual HoldModel CreateHold(List<HoldModel> holdList)
        {
            if (IsPause) return null;
            HoldModel hold = new HoldModel();
            int randomValue = RandomUtils.GetRandomValue(GetHitProb());
            hold.Value = randomValue;
            hold.IsHit = randomValue == 0;
            hold.ValueState = SetSlotValueState(hold.Value);
            hold.SlotValue = _slotManager.CreateValue(hold.ValueState);
            string reachKey = null;
            int hitRate = 0;
            if (PachinkoUtils.CheckDirectHold(hold))
            {
                ReachDirectionModel reachModel = _reachDirectionCtrl.GetReachDirection(hold.IsHit);
                if (reachModel != null)
                {
                    reachKey = reachModel.Name;
                    hitRate = reachModel.HitRate;
                }
            }
            hold.ReachKey = reachKey;
            hold.HitRate = hitRate;
            hold.HoldId = (int)GetHoldIconState(hold, holdList);
            return hold;
        }

        // 保留変換
        public virtual HoldModel ConvertHold(List<HoldModel> holdList, HoldModel beforeHold = null)
        {
            HoldModel hold = new HoldModel();
            hold.HoldId = beforeHold.HoldId;
            hold.Value = beforeHold.Value;
            hold.IsHit = beforeHold.IsHit;
            hold.HitRate = beforeHold.HitRate;
            hold.SlotValue = beforeHold.SlotValue;
            string reachKey = null;
            if (PachinkoUtils.CheckDirectHold(hold))
            {
                reachKey = _reachDirectionCtrl.GetReachDirection(hold.IsHit).Name;
            }
            hold.ReachKey = reachKey;
            return hold;
        }

        // 前モードの保留リストから現モードの保留リストを生成
        public virtual List<HoldModel> CreateRemainHoldList(List<HoldModel> beforeHoldList)
        {
            List<HoldModel> newHoldList = new List<HoldModel>();
            for (int i = 0; i < beforeHoldList.Count; i++)
            {
                newHoldList.Add(ConvertHold(newHoldList, beforeHoldList[i]));
            }
            return newHoldList;
        }

        // 保留チャージ
        public virtual void ChargeHold(HoldModel hold, int holdCount)
        {
            if (IsPause) return;
            if (holdCount > _maxHoldCount) return;
            _holdViewManager?.AddHold(hold);
            if (IsFever)
            {
                DataCountManager?.SetFeverHoldCount(holdCount.ToString());
            }
            else
            {
                DataCountManager?.SetNormalHoldCount(holdCount.ToString());
            }
        }

        // 変動保留削除
        public virtual void RemoveCurHold()
        {
            _holdViewManager?.RemoveCurHoldIcon();
        }

        // 保留変動
        public virtual void MoveHold(int holdCount)
        {
            if (IsPause) return;
            _holdViewManager?.PopHold();
            if (IsFever)
            {
                DataCountManager?.SetFeverHoldCount(holdCount.ToString());
            }
            else
            {
                DataCountManager?.SetNormalHoldCount(holdCount.ToString());
            }
        }

        // スロット回転スタート
        public virtual void StartRotate(int[] slotValue)
        {
            if (IsPause) return;
            _slotManager.Rotate(slotValue);
            StartSlotRotateAction();
        }

        // スロット回転時の処理
        public virtual void SlotRotateAction(int[] slotvalue)
        {
            if (IsPause) return;
            DataCountManager?.SlotViewSetValue(slotvalue);
            DataCountManager?.SlotViewRotate();
        }

        // スロット全停止
        public virtual void StopAllRotate(bool isFast = false)
        {
            if (IsPause) return;
            _slotManager.StopAll(() => {
                AudioManager.PlaySE(PachinkoConst.SE_REEL_STOP);
            }, isFast);
        }

        // スロット停止時の処理
        public virtual void SlotStopAction(int rotateCount)
        {
            if (IsPause) return;
            DataCountManager?.SlotViewStop();
            EndSlotRotateAction();
            panel.SetTotalRotateCount(rotateCount.ToString() + " G");
            CheckModeEnd();
        }

        // モード終了判定
        public virtual void CheckModeEnd()
        {
            if (IsPause) return;
            bool isEndModeFlag = false;
            if (isEndModeFlag)
            {
                EndMode();
            }
        }

        // 演出モデルを生成
        public virtual DirectionModel CreateDirect(
            HoldModel hold, List<HoldModel> holdList, 
            DirectionModel beforeDirect = null, ReachDirectionState beforeReachState = ReachDirectionState.REACH_PSEUDO_1
        )
        {
            DirectionModel direct = new DirectionModel();

            if (hold.ReachKey != null) Debug.Log(hold.ReachKey);

            direct.IsHit = hold.IsHit;

            // 演出が無い且つ保留が多い場合、高速消化する
            if (
                (!PachinkoUtils.CheckReachHold(hold) && !PachinkoUtils.CheckReachHoldList(holdList)) &&
                holdList.Count > (_maxHoldCount / 2) 
            )
            {
                direct.IsHighSpeedRotate = true;
                return direct;
            }

            // 大当たり時、10％の確率でで先バレ演出を出す
            if (hold.IsHit && RandomUtils.GetRandomBool(10))
            {
                direct.IsFirstFindOut = true;
            }

            // 大当たり時、10％の確率で７テンパイにする
            if (hold.IsHit && RandomUtils.GetRandomBool(10))
            {
                direct.IsSevenReach = true;
                hold.SlotValue = new int[]{0, 0, 0};
                UpdateHoldModelCallback?.Invoke(direct.IsChanceUpCurHold, 0, hold);
            }

            // パーティクル設定
            GetShowParticleDirect(hold, direct, beforeDirect);

            // 保留変化設定
            if (CheckChanceUpHold(hold))
            {
                // 変動中保留
                if (RandomUtils.GetRandomBool(2))
                {
                    HoldIconState holdIconState = _holdViewManager.GetChanceUpHoldState(hold);
                    if (holdIconState != HoldIconState.NORMAL) 
                    {
                        direct.IsChanceUpHold = true;
                        direct.IsChanceUpCurHold = true;
                        direct.HoldIconState = holdIconState;
                        hold.HoldId = (int)holdIconState;
                        UpdateHoldModelCallback?.Invoke(direct.IsChanceUpCurHold, 0, hold);
                    }
                }
            }
            else
            {
                // 保留リスト
                int chanceUpHoldIndex = GetChanceUpHoldIndex(hold, holdList);
                if (chanceUpHoldIndex != -1 && RandomUtils.GetRandomBool(2))
                {
                    HoldIconState holdIconState = _holdViewManager.GetChanceUpHoldState(holdList[chanceUpHoldIndex]);
                    if (holdIconState != HoldIconState.NORMAL)
                    {
                        direct.IsChanceUpHold = true;
                        direct.IsChanceUpCurHold = false;
                        direct.ChanceUpHoldIndex = chanceUpHoldIndex;
                        direct.HoldIconState = holdIconState;
                        holdList[chanceUpHoldIndex].HoldId = (int)holdIconState;
                        UpdateHoldModelCallback?.Invoke(
                            direct.IsChanceUpCurHold, chanceUpHoldIndex, holdList[chanceUpHoldIndex]
                        );
                    }
                }
            }

            // リーチ設定
            if (PachinkoUtils.CheckReachHold(hold))
            {
                direct.IsReach = true;

                if (PachinkoUtils.CheckDirectHold(hold))
                {
                    // リーチ固有演出
                    direct.ReachMovieKey = _reachDirectionCtrl.GetReachDirectionByName(hold.ReachKey)?.ShowMovieKey;

                    // リーチ中役物動作フラグ
                    direct.IsReachAccessory = RandomUtils.GetRandomBool(hold.IsHit ? 2 : 5);
                }
            }

            // カットイン設定
            direct.ShowCutin = GetShowCutin(hold, direct, beforeDirect);

            // 疑似連設定
            ReachDirectionModel reachDirectionModel = _reachDirectionCtrl.GetReachDirectionByName(hold.ReachKey);
            if (reachDirectionModel != null)
            {
                // 疑似連数の取得
                ReachDirectionState reachState;
                if (beforeDirect == null)
                {
                    reachState = _reachDirectionCtrl.GetReachDirectionState(reachDirectionModel);
                }
                else
                {
                    reachState = beforeDirect.ReachState;
                }
                
                //疑似連演出の生成
                if (
                    reachState == ReachDirectionState.REACH_PSEUDO_2 || 
                    reachState == ReachDirectionState.REACH_PSEUDO_3 ||
                    reachState == ReachDirectionState.REACH_PSEUDO_4
                ) 
                {
                    direct.IsPseudo = true;
                    direct.ReachState = reachState;

                    // 疑似連終了
                    if (reachState == beforeReachState)
                    {
                        switch(hold.ValueState)
                        {
                            // 疑似連演出とリーチ演出を再生（はずれ）
                            case ValueState.PSEUDO:
                                direct.PseudoSlotValue = _slotManager.CreateValue(ValueState.REACH);
                                break;

                            // 疑似連演出とリーチ演出を再生（あたり）
                            case ValueState.HIT:
                                direct.PseudoSlotValue = hold.SlotValue;
                                break;
                        }
                    }

                    // 疑似連
                    else
                    {
                        direct.IsSevenReach = false;
                        direct.PseudoSlotValue = _slotManager.CreateValue(ValueState.PSEUDO);
                        ReachDirectionState nextReachState = beforeReachState + 1;
                        DirectionModel nextDirect = CreateDirect(hold, holdList, direct, nextReachState);
                        direct.PseudoData = nextDirect;
                    }
                }
            }

            return direct;
        }

        // 演出を再生
        public virtual async void ShowDirect(DirectionModel direct, Action callback = null)
        {
            if (IsPause) return;

            await Task.Delay(2000);

            // 高速消化
            if (direct.IsHighSpeedRotate)
            {
                _slotManager.StopAll(null, false);
                callback?.Invoke();
            }

            // 通常消化
            else
            {
                // 疑似連が発生する場合、スロット値を設定しておく
                if (direct.IsPseudo) 
                {
                    _slotManager.SetValue(direct.PseudoSlotValue);
                }

                // 先バレ演出
                await ShowFirstFindOutDirect(direct);

                // ７テン演出
                await SevenReachDirect(direct);

                // TODO MetaPachi 会話演出

                // パーティクルシャワー演出
                await ShowParticleDirect(direct);

                // 保留昇格演出
                await ChanceUpHoldDirect(direct);

                await Task.Delay(2000);

                // 図柄停止
                // HACK リールが停止するまでディレイをかけて待機する（もっと正確に処理を記述したい
                _slotManager.ReelStop(() => {
                    AudioManager.PlaySE(PachinkoConst.SE_REEL_STOP);
                });
                await Task.Delay(2000);
                _slotManager.ReelStop(() => {
                    AudioManager.PlaySE(PachinkoConst.SE_REEL_STOP);
                });
                await Task.Delay(2000);

                // リーチ演出
                await ShowReachDirect(direct);

                // カットイン演出
                await ShowCutin(direct);

                // 疑似連演出(上からもう一度)
                if (direct.IsPseudo && direct.PseudoData != null)
                {
                    ShowPseudoDirect(direct.PseudoData, callback);
                }
                else
                {
                    // リーチ固有の演出
                    ShowReachMainDirect(direct, callback);
                }
            }
        }

        // 勝利(大当たり)の処理
        public async Task WinAction(PlayDataModel data)
        {
            if (IsPause) return;
            _slotManager.Stop();
            DataCountManager?.SlotViewStop();
            if (_slotManager != default)
            {
                _slotManager.IsPause = true;
            }
            if (_holdViewManager != default)
            {
                _holdViewManager.IsPause = true;
            }
            await WinAnimation();
            EndMode();
        }

        // 敗北の処理
        public async Task LoseAction(PlayDataModel data)
        {
            if (IsPause) return;
            _slotManager.Stop();
            DataCountManager?.SlotViewStop();
            if (_slotManager != default)
            {
                _slotManager.IsPause = true;
            }
            if (_holdViewManager != default)
            {
                _holdViewManager.IsPause = true;
            }
            await LoseAnimation();
            EndMode();
        }

        // はずれ時の処理
        public async Task MissAction(PlayDataModel data)
        {
            if (IsPause) return;
            await MissAnimation();
        }

        // 通常モードに戻ったときの処理
        public async void ReturnNormalAction()
        {
            await ReturnAnimation();
        }

        // 当たり残保留の設定
        public virtual void SetRemainHold(List<HoldModel> beforeholdList)
        {
            List<HoldModel> holdList = new List<HoldModel>();
            for (int i = 0; i < _maxHoldCount; i++)
            {
                if (beforeholdList.Count > i) holdList.Add(ConvertHold(holdList, beforeholdList[i]));
            }
            _holdViewManager?.CreateHoldIcons(holdList);
        }

        // ラウンド数を返す
        public virtual int GetRoundCount(PlayDataModel data)
        {
            return 3;
        }

        // 機能の有効化・無効化
        public virtual void Active(bool b)
        {
            if (_slotManager != default)
            {
                _slotManager.IsPause = !b;
                _slotManager.SetActiveSlotMain(b);
            }
            if (_holdViewManager != default)
            {
                _holdViewManager.IsPause = !b;
            }
        }

        // 当たる確率を100％にする
        public void DebugHitMode()
        {
            _isConfirmHit = true;
        }

        // ---------- Private関数 ----------

        // スロットマネージャー初期化
        private void InitializeSlotManager()
        {
            if (_slotManager == default) return;

            _slotManager.SlotMain = panel.GetSlotMain();
            _slotManager.Initialize();
        }

        // 保留表示マネージャー初期化
        private void InitializeHoldViewManager()
        {
            if (_holdViewManager == default) return;

            _holdViewManager.AddHoldCallback = ChargeHoldDirect;
            _holdViewManager.SetHoldIconPoint(
                panel.GetShowIconPoint(),
                panel.GetShowIconPoints()
            );
            _holdViewManager.Initialize(_maxHoldCount);
        }

        // パーティクルマネージャー初期化
        private void InitializeParticleManager()
        {
            if (ParticleManager == default) return;

            ParticleManager.Initialize();
        }

        // ---------- protected関数 ---------

        // 動画再生演出表示
        protected virtual async void PlayVideo(string playerKey, string clipKey, Action beforeCallback = null, Action endCallback = null)
        {
            await panel.SetActiveVideoPlayerPanel(true);
            VideoManager.PlayVideoClip(
                playerKey,
                clipKey,
                () => {
                    VideoManager.GetVideoPlayer(PachinkoUIConst.BACK_GOURND_PLAYER).gameObject.SetActive(false);
                    beforeCallback?.Invoke();
                    // panel.SetActiveVideoPlayerPanel(true);
                },
                (vp) => {
                    vp.Pause();
                    VideoManager.GetVideoPlayer(PachinkoUIConst.BACK_GOURND_PLAYER).gameObject.SetActive(true);
                    endCallback?.Invoke();
                }
            );
        }

        // 動画再生演出非表示
        protected virtual async void StopVideo(string playerKey, Action callback = null)
        {
            await panel.SetActiveVideoPlayerPanel(false);
            VideoManager.GetVideoPlayer(playerKey).Stop();
            callback?.Invoke();
        }

        // パーティクル表示
        protected virtual void ShowParticle(string key, Color color = default, Transform parent = null)
        {
            if (IsPause) return;
            CommonParticle particle = ParticleManager.CreateParticle(key, parent);
            if (color != default)
            {
                particle.SetColor(color);
            }
            particle.Show();
        }

        // 昇格可能な保留のインデックス番号を返す。無い場合は-1を返す
        protected virtual int GetChanceUpHoldIndex(HoldModel curHold, List<HoldModel> holdList)
        {
            for (int i = 0; i < holdList.Count; i++)
            {
                if (holdList[i] == null) continue;

                // 昇格できる保留かどうか
                if (!CheckChanceUpHold(holdList[i])) continue;

                // 他に昇格済み保留があるなら処理しない
                if (
                    (HoldIconState)curHold.HoldId != HoldIconState.NORMAL &&
                    (HoldIconState)curHold.HoldId != HoldIconState.LEVEL_1
                ) return -1;
                for (int j = 0; j < i; j++)
                {
                    if (
                        (HoldIconState)holdList[j].HoldId != HoldIconState.NORMAL &&
                        (HoldIconState)holdList[j].HoldId != HoldIconState.LEVEL_1
                    ) return -1;
                }
                return i;
            }
            return -1;
        }

        // 保留アイコンの抽選
        protected virtual HoldIconState GetHoldIconState(HoldModel hold, List<HoldModel> holdList)
        {
            // // すでに特殊保留があるなら通常保留のみ生成
            // if (CheckReachHold(holdList)) return HoldIconState.NORMAL;

            HoldIconState iconState = HoldIconState.NORMAL;
            List<HoldIconState> iconStateList = new List<HoldIconState>();
            foreach (var kvp in _holdViewManager.GetHoldIconObjects().GetTable())
            {
                if (kvp.Key == HoldIconState.NORMAL)
                    continue;
                if (kvp.Value.MinHitProb <= hold.HitRate)
                {
                    iconStateList.Add(kvp.Key);
                }
            }
            iconState = iconStateList[RandomUtils.GetRandomValue(iconStateList.Count)];
            return iconState;
        }

        // 保留が昇格するかどうか
        protected virtual bool CheckChanceUpHold(HoldModel hold)
        {
            HoldIconState state = (HoldIconState)hold.HoldId;

            // 昇格不可能な保留は処理しない
            if (!PachinkoConst.chanceUpHoldArray.Contains(state)) return false;

            // 昇格先の保留が存在しない場合は処理しない
            int stateIndex = PachinkoConst.chanceUpHoldArray.IndexOf(state);
            if (stateIndex > PachinkoConst.chanceUpHoldArray.Count - 1) return false;

            // 昇格先の保留が設定されていない場合は処理しない
            HoldIconState nextState = PachinkoConst.chanceUpHoldArray[stateIndex + 1];
            if (!_holdViewManager.GetHoldIconObjects().GetKeyList().Contains(nextState)) return false;

            // 大当たり確率が昇格先保留水準を満たしていない場合は処理しない
            HoldIcon chanceUpIcon = _holdViewManager.GetHoldIconObjects().GetValue(nextState);
            if (hold.HitRate < chanceUpIcon.MinHitProb) return false;

            return true;
        }

        // 保留チャージ時の演出
        protected virtual void ChargeHoldDirect(HoldModel hold) { }

        // リーチ演出が入る保留がリストに存在するか(ただのテンパイは除く)
        protected virtual bool CheckReachHold(List<HoldModel> holdList)
        {
            foreach (HoldModel holdModel in holdList)
            {
                if(holdModel == null) continue;
                if(holdModel.ReachDirectionState != ReachDirectionState.NONE) return true;
            }
            return false;
        }

        // スロット回転開始時の処理
        protected virtual void StartSlotRotateAction() { }

        // スロット回転停止時の処理
        protected virtual void EndSlotRotateAction() { }

        // TODO スロット回転中の演出
        protected virtual Func<Task> CreateStartRotateCallback(HoldModel hold)
        {
            return async () => {
                await Task.CompletedTask;
            };
        }

        // 勝利時の演出を再生
        protected async virtual Task WinAnimation()
        {
            await Task.CompletedTask;
        }

        // 敗北時の演出を再生
        protected async virtual Task LoseAnimation()
        {
            await Task.CompletedTask;
        }

        // はずれ時の演出を再生
        protected virtual async Task MissAnimation()
        {
            await Task.Delay(2000);
        } 

        // 通常モードに戻った時の演出を再生
        protected virtual async Task ReturnAnimation()
        {
            await Task.CompletedTask;
        }

        // スロットの出目の設定
        protected virtual ValueState SetSlotValueState(int value)
        {
            ValueState valueState = ValueState.NONE;
            // 大当たり
            if (value == 0)
            {
                valueState = ValueState.HIT;
            }
            // 疑似連
            else if (value <= _reachDirectionCtrl.GetReachValue())
            {
                valueState = ValueState.PSEUDO;
            }
            // リーチ
            else if (RandomUtils.GetRandomBool(_reachProb))
            {
                valueState = ValueState.REACH;
            }
            return valueState;
        }

        // パーティクル演出の抽選
        protected virtual void GetShowParticleDirect(HoldModel hold, DirectionModel direct, DirectionModel beforeDirect) { }

        // パーティクル演出
        protected virtual Task ShowParticleDirect(DirectionModel direct)
        {
            if (IsPause) return Task.CompletedTask;
            if (direct.ParticleKey != default || direct.ParticleKey != null)
            {
                ShowParticle(direct.ParticleKey, direct.ParticleColor);
            }
            return Task.CompletedTask;
        }

        // 保留変化演出
        protected virtual Task ChanceUpHoldDirect(DirectionModel direct)
        {
            if (direct.IsChanceUpHold)
            {
                Transform parent = _holdViewManager.ChanceUpHold(
                    direct.IsChanceUpCurHold, direct.ChanceUpHoldIndex, direct.HoldIconState
                );
                ChanceUpDirect(parent, direct);
            }
            return Task.CompletedTask;
        }

        // リーチ演出
        protected virtual Task ShowReachDirect(DirectionModel direct)
        {
            return Task.CompletedTask;
        }

        // カットイン演出
        protected virtual Task ShowCutin(DirectionModel direct)
        {
            return Task.CompletedTask;
        }

        // カットイン番号取得
        protected virtual string GetShowCutin(HoldModel hold, DirectionModel direct, DirectionModel beforeDirect)
        {
            return null;
        }

        // 疑似連演出
        protected virtual async void ShowPseudoDirect(DirectionModel direct, Action callback = null)
        {
            if (IsPause) return;
            _slotManager.ReelStop(() => {
                AudioManager.PlaySE(PachinkoConst.SE_REEL_STOP);
            });
            await Task.Delay(2000);
            StartRotate(direct.PseudoSlotValue);
            ShowDirect(direct, callback);
        }

        // リーチ固有演出
        protected virtual async void ShowReachMainDirect(DirectionModel direct, Action callback = null)
        {
            if (IsPause) return;
            _slotManager.ReelStop(() => {
                AudioManager.PlaySE(PachinkoConst.SE_REEL_STOP);
            });
            await Task.Delay(2000);
            callback?.Invoke();
        }

        // 先バレ演出
        protected virtual Task ShowFirstFindOutDirect(DirectionModel direct)
        {
            if (IsPause) return Task.CompletedTask;
            if (!direct.IsFirstFindOut) return Task.CompletedTask;
            return Task.CompletedTask;
        }

        // ７テン演出
        protected virtual Task SevenReachDirect(DirectionModel direct)
        {
            if (IsPause) return Task.CompletedTask;
            if (!direct.IsSevenReach) return Task.CompletedTask;
            return Task.CompletedTask;
        }

        // 保留変化時の演出
        protected virtual void ChanceUpDirect(Transform holdPos, DirectionModel direct) { }
    }
}
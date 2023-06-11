using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

using ShunLib.Utils.Random;
using ShunLib.Utils.Debug;

using Pachinko.Dict;
using Pachinko.Model;
using Pachinko.Const;
using Pachinko.Utils;
using Pachinko.GameMode.Base.Manager;
using Pachinko.FinalBattle.Panel;
using Pachinko.FinalBattle.ChargeIcon;

namespace Pachinko.FinalBattle.Manager
{
    public class FinalBattleManager : BaseModeManager
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("最終決戦のチャージ時間")] 
        [SerializeField] protected float _maxChargeTime = default;

        [Header("最終決戦アイコンのテーブル")] 
        [SerializeField] protected FinalBattleChargeIconModelTable _iconTable = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public Action ChargeEndCallback { get; set; } 

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        protected bool _isFinalBattleMode = default;
        protected bool _isChargeMode = default;
        protected bool _isChargeComplete = default;
        protected bool _isChanceUpMode = default;
        protected List<FinalBattleChargeIconModel> _iconModelList = default;
        protected float _progressTime = default;
        protected int _remainChargeCount = default;
        protected FinalBattlePanel _finalBattlePanel = default;
        protected int _consumeIconIndex = default;

        // ---------- Unity組込関数 ----------

        void Update()
        {
            if (IsPause) return;
            if (_isChargeMode)
            {
                // 残り時間の描画
                _progressTime += Time.deltaTime;
                _finalBattlePanel.SetRemainText(_maxChargeTime - _progressTime);

                if (_isChargeComplete)
                {
                    _isChargeComplete = false;
                    _finalBattlePanel.ShowChargeCompleteCutin(() => {
                        _isChanceUpMode = true;
                    });
                }

                // 時間終了
                if (_progressTime >= _maxChargeTime)
                {
                    _isChargeMode = false;
                    _progressTime = 0f;
                    _finalBattlePanel.SetRemainText(0f);
                    _consumeIconIndex = 0;
                    ChargeEndCallback?.Invoke();
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            _isFinalBattleMode = false;
            _isChargeMode = false;
            _isChargeComplete = false;
            _isChanceUpMode = false;
            _remainChargeCount = 0;
            _progressTime = 0f;
            _finalBattlePanel = panel as FinalBattlePanel;
        }

        // 各パラメータの設定
        public void SetParameter(float maxChargeTime = 0f, FinalBattleChargeIconModelTable iconTable = null)
        {
            _maxChargeTime = maxChargeTime;
            _iconTable = iconTable;
        }

        // モードスタート
        public override void StartMode(int[] slotValue)
        {
            DebugUtils.Log("最終決戦：開始");

            IsPause = true;
            Show();
            _slotManager.SlotMain.RotateCount = 0;
            if (_bgMovieKey != default && _bgMovieKey != "")
            {
                VideoManager.PlayLoopVideoClip(PachinkoUIConst.BACK_GOURND_PLAYER, _bgMovieKey);
            }
            _slotManager.SetSlotValue(slotValue);
            panel.SetTotalRotateCount("0 G");
            DataCountManager?.SlotViewShowValue(slotValue);
            _isFinalBattleMode = true;
            _isChargeComplete = false;
            _isChanceUpMode = false;
            _iconModelList = new List<FinalBattleChargeIconModel>();
            _finalBattlePanel.Initialize();
            _finalBattlePanel.SetActiveFBContent(true);
            _finalBattlePanel.SetRemainText((float)_maxChargeTime);

            StartAnimation(() => {
                _finalBattlePanel.Show();
                _progressTime = 0f;
                _isChargeMode = true;
                IsPause = false;
            });
        }

        // モードエンド
        public override void EndMode()
        {
            base.EndMode();
            _remainChargeCount = 0;
        }

        // 保留生成
        public override HoldModel CreateHold(List<HoldModel> holdList)
        {
            if (IsPause) return null;
            HoldModel hold = new HoldModel();
            int randomValue = RandomUtils.GetRandomValue(GetHitProb());
            hold.Value = randomValue;
            hold.IsHit = randomValue == 0;
            ValueState valueState = SetSlotValueState(hold.Value);
            hold.ValueState = valueState;
            hold.SlotValue = CreateFinalBattleSlotValue(valueState);
            int hitRate = 0;
            if (PachinkoUtils.CheckDirectHold(hold))
            {
                ReachDirectionModel reachModel = _reachDirectionCtrl.GetReachDirection(hold.IsHit);
                if (reachModel != null)
                {
                    hitRate = reachModel.HitRate;
                }
            }
            hold.ReachKey = null;
            hold.HitRate = hitRate;
            hold.HoldId = (int)GetFinalBattleChargeIconState(hold);
            return hold;
        }

        // 保留変換
        public override HoldModel ConvertHold(List<HoldModel> holdList, HoldModel beforeHold = null)
        {
            HoldModel hold = new HoldModel();
            hold.HoldId = (int)FinalBattleChargeIconState.LEVEL_1;
            hold.ValueState = beforeHold.ValueState;
            hold.Value = beforeHold.Value;
            hold.IsHit = beforeHold.IsHit;
            int hitRate = 0;
            if (PachinkoUtils.CheckDirectHold(hold))
            {
                ReachDirectionModel reachModel = _reachDirectionCtrl.GetReachDirection(hold.IsHit);
                hitRate = reachModel.HitRate;
            }
            hold.ReachKey = null;
            hold.HitRate = beforeHold.HitRate;
            hold.SlotValue = beforeHold.SlotValue;
            return hold;
        }

        // TODO 変動保留削除
        public override void RemoveCurHold()
        {
            
        }

        // TODO 保留変動
        public override void MoveHold(int holdCount)
        {
            
        }

        // 当たり残保留の設定
        public override void SetRemainHold(List<HoldModel> beforeholdList)
        {
            _remainChargeCount = beforeholdList.Count;
            SetHoldText(beforeholdList.Count.ToString());
            List<HoldModel> newHoldList = new List<HoldModel>();
            foreach (HoldModel hold in beforeholdList)
            {
                HoldModel newHold = ConvertHold(newHoldList, hold);
                newHoldList.Add(newHold);
                AddIcon(newHold);
            }
        }

        // スロット回転スタート
        public override void StartRotate(int[] slotValue)
        {
            if (IsPause) return;
            _slotManager.SlotMain.ShowSlotDesigns(slotValue);
            DataCountManager.SlotViewSetValue(slotValue);
            StartSlotRotateAction();
        }

        // TODO スロット全停止
        public override void StopAllRotate(bool isFast = false)
        {
            if (IsPause) return;
            _slotManager.StopAll(null, isFast);
        }

        // TODO スロット停止時の処理
        public override void SlotStopAction(int rotateCount)
        {
            if (IsPause) return;
            DataCountManager?.SlotViewStop();
            panel.SetTotalRotateCount(rotateCount.ToString() + " G");
            CheckModeEnd();
        }

        // 終了回転回数を返す
        public override int GetEndRotateCount()
        {
            return _iconModelList.Count;
        }

        // 演出モデルを生成
        public override DirectionModel CreateDirect(
            HoldModel hold, List<HoldModel> holdList, 
            DirectionModel beforeDirect = null, ReachDirectionState beforeReachState = ReachDirectionState.REACH_PSEUDO_1
        )
        {
            DirectionModel direct = new DirectionModel();
            direct.IsHit = hold.IsHit;
            return direct;
        }

        // 演出を再生
        // HACK Delayをもちいずに実装したい
        public override async void ShowDirect(DirectionModel direct, Action callback = null)
        {
            if (IsPause) return;
            int iconCount = _finalBattlePanel.GetIconCount();
            SetHoldText(iconCount.ToString());

            // 開始アニメーション再生
            ChargeIconAction();
            DataCountManager.SlotViewRotate();
            _finalBattlePanel.ShowChanceCountCutin(iconCount);

            await Task.Delay(3000);

            // 実行パネルの選択アニメーション
            await _finalBattlePanel.GetChargeIcon(_consumeIconIndex).FlashIcon();
            ShowChargeIconAnim(_finalBattlePanel.GetChargeIcon(_consumeIconIndex));

            await Task.Delay(300);

            // リーチ図柄の表示
            await ShowDesignAnim();

            // 最終決戦固有の演出
            ShowReachMainDirect(direct, callback);
        }

        // 保留チャージ
        public override void ChargeHold(HoldModel hold, int holdCount)
        {
            if (IsPause) return;
            if (!_isChargeMode) return;
            if (_isChanceUpMode)
            {
                ChanceUpIcons(hold);
            }
            else
            {
                AddIcon(hold);
            }
        }

        // ラウンド数を返す
        public override int GetRoundCount(PlayDataModel data)
        {
            switch (data.CurHold.SlotValue[0])
            {
                case 0:
                case 2:
                case 4:
                case 6:
                    return 10;
                default:
                    return 4;
            }
        }

        // ---------- Private関数 ----------

        // 図柄の抽選
        private int[] CreateFinalBattleSlotValue(ValueState valueState)
        {
            switch(valueState)
            {
                case ValueState.HIT:
                    return _slotManager.CreateValue(ValueState.HIT);
                default:
                    return _slotManager.CreateValue(ValueState.REACH);
            }
        }

        // アイコン追加
        private void AddIcon(HoldModel hold)
        {
            if (hold == null) return;
            if (!_finalBattlePanel.CheckAddIcon()) return;
            DataCountManager.SlotViewRotate();
            if (hold.IsHit) DebugUtils.Log("最終決戦：当たりアイコンチャージ！");

            // Debug.Log(GetIconModel(hold).Level);

            FinalBattleChargeIconModel model = GetIconModel(hold);
            _iconModelList.Add(model);
            SetHoldText(_iconModelList.Count.ToString());

            bool isLastIcon = _iconModelList.Count >= _finalBattlePanel.GetIconListCount;
            if (isLastIcon)
            {
                _finalBattlePanel.AddIcon(
                    sprite: model.Sprite,
                    frameColor: model.FrameColor,
                    callback: () => { _isChargeComplete = _iconModelList.Count >= _finalBattlePanel.GetIconListCount; }
                );
            }
            else
            {
                _finalBattlePanel.AddIcon(
                    sprite: model.Sprite,
                    frameColor: model.FrameColor
                );
            }
        }

        // アイコン昇格
        private void ChanceUpIcons(HoldModel hold)
        {
            if (IsPause) return;
            // TODO 同期処理を仮で動かすためにHoldModelのパラメータを用いて昇格させる
            ChanceUpIcon(hold.Value % 5, hold);
        }

        // アイコン昇格
        private void ChanceUpIcon(int index, HoldModel hold)
        {
            if (IsPause) return;
            FinalBattleChargeIconModel model = _iconModelList[index];

            // 昇格不可能な時は処理しない
            if (!PachinkoConst.chanceUpFinalBattleIconArray.Contains(model.Level)) return;

            // 昇格先が存在しない場合は処理しない
            int stateIndex = PachinkoConst.chanceUpFinalBattleIconArray.IndexOf(model.Level);
            if (stateIndex > PachinkoConst.chanceUpFinalBattleIconArray.Count - 1) return;

            // // 昇格先が設定されていない場合は処理しない
            // FinalBattleChargeIconState nextState = PachinkoConst.chanceUpFinalBattleIconArray[stateIndex + 1];
            // if (!_iconTable.GetKeyList().Contains(nextState)) return;

            // TODO 新規保留のIsHitがTrueの場合は処理しない
            if (hold.IsHit) return;

            // TODO 新規保留のLevelが現保留以下の場合は処理しない
            if ((int)model.Level >= hold.HoldId) return;
            // Debug.Log(model.Level + " >>> " + (FinalBattleChargeIconState)hold.HoldId);

            // TODO 昇格先が設定されていない場合は処理しない
            FinalBattleChargeIconState nextState = (FinalBattleChargeIconState)hold.HoldId;
            if (!_iconTable.GetKeyList().Contains(nextState)) return;

            FinalBattleChargeIcon curIcon = _finalBattlePanel.GetChargeIcon(index);
            FinalBattleChargeIconModel nextIcon = _iconTable.GetValue(nextState);

            // 昇格アニメーション再生
            curIcon.ShowChanceUpAnim();
            curIcon.SetFrameColor(nextIcon.FrameColor);
            curIcon.SetIconImage(nextIcon.Sprite);
            _iconModelList[index] = nextIcon;
            ChanceUpDirect(nextIcon);
            UpdateHoldModelCallback?.Invoke(false, index, hold);
        }

        // アイコンの拡大表示アニメーション
        private void ShowChargeIconAnim(FinalBattleChargeIcon icon, Action callback = null)
        {
            if (IsPause) return;
            icon.transform.SetAsLastSibling();
            icon.gameObject.transform.DOLocalMoveX(0f, 0.2f);
            icon.gameObject.transform.DOScale(new Vector3(3f, 3f, 3f), 0.2f).OnComplete(() => {
                callback?.Invoke();
            });
        }

        // 保留数の表示
        private void SetHoldText(string value)
        {
            if (IsFever)
            {
                DataCountManager.SetFeverHoldCount(value);
            }
            else
            {
                DataCountManager.SetNormalHoldCount(value);
            }
        }

        // ---------- protected関数 ---------

        // 保留消化開始時の処理
        protected virtual void ChargeIconAction() { }

        // 昇格演出
        protected virtual void ChanceUpDirect(FinalBattleChargeIconModel icon) { }

        // 保留アイコンの抽選
        protected virtual FinalBattleChargeIconState GetFinalBattleChargeIconState(HoldModel hold)
        {
            List<FinalBattleChargeIconState> iconStateList = new List<FinalBattleChargeIconState>();
            iconStateList.Add(FinalBattleChargeIconState.LEVEL_1);
            if (RandomUtils.GetRandomBool(hold.IsHit ? 1 : 3)) iconStateList.Add(FinalBattleChargeIconState.LEVEL_2);
            if (RandomUtils.GetRandomBool(hold.IsHit ? 1 : 5)) iconStateList.Add(FinalBattleChargeIconState.LEVEL_3);
            return iconStateList[RandomUtils.GetRandomValue(iconStateList.Count)];;
        }

        // 最終決戦チャージアイコンモデルの抽選
        protected FinalBattleChargeIconModel GetIconModel(HoldModel hold)
        {
            FinalBattleChargeIconModel model = null;
            int iconIndex = hold.HoldId;
            switch (iconIndex)
            {
                case 1:
                    model = _iconTable.GetValue(FinalBattleChargeIconState.LEVEL_1);
                    break;
                case 2:
                    model = _iconTable.GetValue(FinalBattleChargeIconState.LEVEL_2);
                    break;
                case 3:
                    model = _iconTable.GetValue(FinalBattleChargeIconState.LEVEL_3);
                    break;
                default:
                    model = _iconTable.GetValue(FinalBattleChargeIconState.LEVEL_1);
                    break;
            }
            return model;
        }

        // 最終決戦開始アニメーション
        protected virtual void StartAnimation(Action callback)
        {
            callback?.Invoke();
        }

        // リーチ固有演出
        protected override async void ShowReachMainDirect(DirectionModel direct, Action callback = null)
        {
            if (IsPause) return;
            _slotManager.ReelStop(() => {
                AudioManager.PlaySE(PachinkoConst.SE_REEL_STOP);
            });
            await Task.Delay(2000);
            _finalBattlePanel.SetActiveSlotMainPanel(false);
            HideChargeIconAnim(_finalBattlePanel.GetChargeIcon(_consumeIconIndex));
            callback?.Invoke();
        }

        // 勝利時の演出を再生
        protected async override Task WinAnimation()
        {
            if (IsPause) return;
            DebugUtils.Log("最終決戦：勝利");
            await WinDesignAnim();
            await Task.CompletedTask;
        }

        // 敗北時の演出を再生
        protected async override Task LoseAnimation()
        {
            if (IsPause) return;
            DebugUtils.Log("最終決戦：敗北");
            await LoseDesignAnim();
            await Task.CompletedTask;
        }

        // はずれ時の演出を再生
        protected override async Task MissAnimation()
        {
            if (IsPause) return;
            _finalBattlePanel.GetChargeIcon(_consumeIconIndex).SetActive(false);
            _consumeIconIndex++;
            DataCountManager.SlotViewStop();
            await LoseDesignAnim();
        }

        // 拡大アイコンの非表示アニメーション
        protected void HideChargeIconAnim(FinalBattleChargeIcon icon, Action callback = null)
        {
            if (IsPause) return;
            icon.gameObject.transform.localPosition = new Vector3(
                icon.GetDefaultPosX(), 0f, 0f
            );
            icon.gameObject.transform.localScale = Vector3.one;
            callback?.Invoke();
        }

        // 図柄リーチ表示アニメーション
        protected virtual async Task ShowDesignAnim()
        {
            if (IsPause) return;
            _slotManager.SlotMain.SetReelAlpha(0, 1f);
            _slotManager.SlotMain.SetReelAlpha(1, 1f);
            _slotManager.SlotMain.SetReelAlpha(2, 0f);
            _finalBattlePanel.SetActiveSlotMainPanel(true);
            await Task.Delay(1500);
        }

        // 図柄あたりアニメーション
        protected virtual async Task WinDesignAnim()
        {
            if (IsPause) return;
            _slotManager.SlotMain.SetReelAlpha(0, 1f);
            _slotManager.SlotMain.SetReelAlpha(1, 1f);
            _slotManager.SlotMain.SetReelAlpha(2, 1f);
            _finalBattlePanel.SetActiveSlotMainPanel(true);
            await Task.Delay(1500);
            _finalBattlePanel.SetActiveSlotMainPanel(false);
        }

        // 図柄はずれアニメーション
        protected virtual async Task LoseDesignAnim()
        {
            if (IsPause) return;
            _slotManager.SlotMain.SetReelAlpha(0, 1f);
            _slotManager.SlotMain.SetReelAlpha(1, 1f);
            _slotManager.SlotMain.SetReelAlpha(2, 1f);
            _finalBattlePanel.SetActiveSlotMainPanel(true);
            await Task.Delay(1500);
            _finalBattlePanel.SetActiveSlotMainPanel(false);
        }
    }
}



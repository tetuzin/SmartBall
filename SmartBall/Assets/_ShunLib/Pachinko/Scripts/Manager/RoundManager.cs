using System;
using UnityEngine;

using ShunLib.Manager.Video;
using ShunLib.Manager.Audio;

using Pachinko.Model;
using Pachinko.Round.Panel;

namespace Pachinko.Round.Manager
{
    public class RoundManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------

        public const string CUTIN_RIGHT_SHOT_MODE = "cutin_right_shot_mode";

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ラウンドパネル")] 
        [SerializeField] protected RoundPanel _panel = default;

        [Header("1ラウンド強制終了時間")] 
        [SerializeField] protected float _roundEndTime = 15f;
        [Header("ラウンド時、一発で得られるポイント量")] 
        [SerializeField] protected int _roundPointValue = 15;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // フィーバー数
        public int FeverCount
        {
            get { return _feverCount; }
            set { _feverCount = value; }
        }

        // 合計ポイント値
        public int TotalPoint
        {
            get { return _totalPoint; }
            set { _totalPoint = value; }
        }

        // 動画再生マネージャー
        public VideoManager VideoManager { get; set; }
        // 音声再生マネージャー
        public AudioManager AudioManager { get; set; }

        // ラウンド終了時コールバック
        public Action EndRoundCallback { get; set; }
        // ラウンド強制中断時コールバック
        public Action BreakEndRoundCallback { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool _isRoundMode = default;
        private int _round = default;
        private int _curRound = default;
        private int _feverCount = default;
        private int _totalPoint = default;
        private int _curPoint = default;
        private float _progressRoundTime = default;

        // ---------- Unity組込関数 ----------

        void FixedUpdate()
        {
            ConsumeRound();
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _isRoundMode = false;
            _round = 0;
            _curRound = 0;
            _feverCount = 0;
            _totalPoint = 0;
            _curPoint = 0;
            _progressRoundTime = 0.0f;
            _panel.Initialize();
        }

        // 各パラメータの設定
        public void SetParameter(RoundPanel panel = null, float roundEndTime = 0f, int roundPointValue = 0)
        {
            _panel = panel;
            _roundEndTime = roundEndTime;
            _roundPointValue = roundPointValue;
        }

        // ラウンド開始
        public async void StartRound(int roundCount)
        {
            // 各種パラメータ初期化
            _isRoundMode = true;
            _round = roundCount;
            _curRound = 0;
            _curPoint = 0;
            _progressRoundTime = 0.0f;
            _feverCount++;

            // UI描画
            _panel.SetFeverCount(_feverCount.ToString());
            _panel.SetCurRoundCount(_curRound.ToString());
            _panel.SetMaxRoundCount(_round.ToString());
            _panel.SetTotalPoint(_totalPoint.ToString());
            _panel.SetCurPoint(_curPoint.ToString());
            _panel.SetMaxPoint((_roundPointValue * 10 * _round).ToString());
            _panel.Show();
            StartRoundDirect();
        }

        // 次ラウンドへ遷移
        public void NextRound()
        {
            if (_isRoundMode)
            {
                // 1ラウンド目に玉が1発も入らなかった場合は左打ちへ戻す
                if (_curPoint == 0)
                {
                    EndBreakRound();
                }
                _progressRoundTime = 0.0f;
                _curRound++;
                _panel.SetCurRoundCount(_curRound.ToString());
                if (_curRound >= _round)
                {
                    EndMaxRound();
                }
            }
        }

        // ポイントの獲得
        public void UpdateGetPoint()
        {
            _curPoint += _roundPointValue;
            _totalPoint += _roundPointValue;
        }

        // 現在のデータの描画
        public void UpdateView(PlayDataModel data)
        {
            _panel.SetCurPoint(data.CurPoint.ToString());
            _panel.SetTotalPoint(data.TotalPoint.ToString());
            _panel.SetCurRoundCount(data.CurRound.ToString());
        }

        // フィーバー数を返す
        public int GetFever()
        {
            return _feverCount;
        }

        // 一入賞で得られるポイント量を返す
        public int GetOnePrizePoint()
        {
            return _roundPointValue;
        }

        // ポイントを返す
        public int GetPoint()
        {
            return _curPoint;
        }

        // 合計ポイントを返す
        public int GetTotalPoint()
        {
            return _totalPoint;
        }

        // 現在のラウンド数を返す
        public int GetCurRound()
        {
            return _curRound;
        }

        // ラウンド終了
        public void EndRound()
        {
            _panel.Hide();
            _isRoundMode = false;
        }

        // ---------- Private関数 ----------

        // ラウンド消化
        private void ConsumeRound()
        {
            if (_isRoundMode)
            {
                // 時間経過で次のラウンドへ
                _progressRoundTime += Time.deltaTime;
                if (_progressRoundTime >= _roundEndTime)
                {
                    NextRound();
                }

                // 一定数のポイント獲得で次のラウンドへ
                if (_curPoint >= (_roundPointValue * 10 * (_curRound + 1)))
                {
                    NextRound();
                }
            }
        }

        // 最後のラウンドまで消化して終了
        private void EndMaxRound()
        {
            EndRound();
            EndRoundCallback?.Invoke();
        }

        // ラウンドを消化せずに強制終了
        private void EndBreakRound()
        {
            EndRound();
            BreakEndRoundCallback?.Invoke();
        }

        // ---------- protected関数 ---------

        // ラウンドスタート時の演出
        protected virtual async void StartRoundDirect()
        {
            await _panel.ShowCutin(CUTIN_RIGHT_SHOT_MODE, null, 3f);
        }
    }
}
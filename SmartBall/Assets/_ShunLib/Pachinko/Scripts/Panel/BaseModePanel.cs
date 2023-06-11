using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Dict;
using ShunLib.UI.Panel;

using Pachinko.Slot.Controller;

namespace Pachinko.GameMode.Base.Panel
{
    public class BaseModePanel : CommonPanel
    {
        // ---------- 定数宣言 ----------

        // メインスロット
        private const string SLOT_MAIN_CONTROLLER = "slot_main_controller";
        // 詳細パネル
        private const string PANEL_DETAILS = "panel_details";
        // 保留表示パネル
        private const string PANEL_HOLD_ICON_POINT = "panel_hold_icon_panel";
        // 役物ボタン
        private const string BUTTON_CUTIN_PACHINKO = "button_cutin_pachinko";
        // 総回転数
        private const string TEXT_TOTAL_ROTATE_COUNT = "text_total_rotate_count";
        // ステージ名
        private const string TEXT_STAGE_NAME = "text_stage_name";

        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("メインのスロット")]
        [SerializeField] private SlotBaseController _slotMain = default;

        [Header("変動中保留表示ポイント")] 
        [SerializeField] private GameObject _showIconPoint = default;

        [Header("保留表示ポイント")] 
        [SerializeField] private List<GameObject> _showIconPoints = default;

        [Header("ビデオプレイヤー")]
        [SerializeField] private VideoPlayerPair _videoPlayerPair = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            SetActiveVideoPlayerPanel(false);
        }

        // 表示
        public override void Show()
        {
            SetActivePanel(true);
            base.Show();
        }

        // 非表示
        public override void Hide()
        {
            SetActivePanel(false);
            base.Hide();
        }

        // 総回転数テキストの設定
        public void SetTotalRotateCount(string text)
        {
            SetText(TEXT_TOTAL_ROTATE_COUNT, text);
        }

        // ステージ名テキストの設定
        public void SetStageName(string text)
        {
            SetText(TEXT_STAGE_NAME, text);
        }

        // 全パネルの表示・非表示
        public void SetActivePanel(bool b)
        {
            SetActiveDetailsPanel(b);
            SetActiveHoldIconPointPanel(b);
            SetActiveSlotMainPanel(b);
        }

        // 詳細パネルの表示・非表示
        public void SetActiveDetailsPanel(bool b)
        {
            SetCanvasGroupActive(PANEL_DETAILS, b);
        }

        // 保留表示パネルの表示・非表示
        public void SetActiveHoldIconPointPanel(bool b)
        {
            SetCanvasGroupActive(PANEL_HOLD_ICON_POINT, b);
        }

        // メインスロットパネルの表示・非表示
        public void SetActiveSlotMainPanel(bool b)
        {
            SetCanvasGroupActive(SLOT_MAIN_CONTROLLER, b);
        }

        // 動画再生パネルの表示・非表示
        public async Task SetActiveVideoPlayerPanel(bool b)
        {
            // HACK パネルの描画遅れを見せないために少し間をおいて表示する
            if (b)
            {
                await Task.Delay(100);
            }
            _videoPlayerPair.Value.gameObject.SetActive(b);
            await Task.CompletedTask;
        }

        // メインスロットの取得
        public SlotBaseController GetSlotMain()
        {
            return _slotMain;
        }

        // 変動中保留表示ポイントの取得
        public GameObject GetShowIconPoint()
        {
            return _showIconPoint;
        }

        // 保留表示ポイントの取得
        public List<GameObject> GetShowIconPoints()
        {
            return _showIconPoints;
        }

        // ビデオプレイヤーペアの取得
        public VideoPlayerPair GetVideoPlayerPair()
        {
            return _videoPlayerPair;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}



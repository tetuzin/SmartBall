using System;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.UI.Cutin.Text;
using ShunLib.UI.Cutin.Img;

using Pachinko.FinalBattle.ChargeIcon;
using Pachinko.GameMode.Base.Panel;

namespace Pachinko.FinalBattle.Panel
{
    public class FinalBattlePanel : BaseModePanel
    {
        // ---------- 定数宣言 ----------

        // 残り時間テキスト
        private const string TEXT_REMAIN_TIME = "text_remain_time";
        // チャージ完了カットイン
        private const string CHARGE_COMPLETE_CUTIN = "Charge_Complete_Cutin";
        // 残りチャンス回数カットイン
        public const string CHANCE_COUNT_CUTIN = "Chance_Count_Cutin";
        // 残りチャンス回数カットイン
        public const string IMAGE_CUTIN = "Image_Cutin";
        // 最終決戦固有UIのキャンバス
        public const string CANVAS_GROUP_FB_CONTENTS = "canvas_group_final_battle_contents";
        
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("アイコンリスト")] 
        [SerializeField] private List<FinalBattleChargeIcon> _iconList = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public int GetIconListCount 
        {
            get { return _iconList.Count; }
        }
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        private int _activeIconCount = default;
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            _activeIconCount = 0;
            foreach (FinalBattleChargeIcon icon in _iconList)
            {
                icon.Initialize();
                icon.SetActive(false);
            }
        }

        // 表示
        public override void Show()
        {
            base.Show();
            SetActiveSlotMainPanel(false);
        }

        // アイコンの取得
        public FinalBattleChargeIcon GetChargeIcon(int index)
        {
            return _iconList[index];
        }

        // アイコン追加表示
        public void AddIcon(Sprite sprite, Color frameColor, Action callback = null)
        {
            if (_activeIconCount >= _iconList.Count) return;

            FinalBattleChargeIcon icon = _iconList[_activeIconCount];
            _activeIconCount++;
            icon.SetIconImage(sprite);
            icon.SetFrameColor(frameColor);
            ShowChargeIconCutin(sprite, () => {
                icon.ShowIconAnim();
                callback?.Invoke();
            });
        }

        // アイコン追加可否チェック
        public bool CheckAddIcon()
        {
            return _activeIconCount < _iconList.Count;
        }

        // 活性アイコンの数
        public int GetIconCount()
        {
            int count = 0;
            foreach (FinalBattleChargeIcon icon in _iconList)
            {
                if (icon.GetActive())
                {
                    count++;
                }
            }
            return count;
        }

        // 残り時間テキストの設定
        public void SetRemainText(float num)
        {
            SetText(TEXT_REMAIN_TIME, String.Format("{0:00.00}", num));
        }

        // チャージ完了カットイン表示
        public void ShowChargeCompleteCutin(Action callback = null)
        {
            ShowCutin(CHARGE_COMPLETE_CUTIN, () => {
                callback?.Invoke();
            });
        }

        // 残りチャンス回数カットイン表示
        public void ShowChanceCountCutin(int count, Action callback = null)
        {
            TextCutin cutin = (TextCutin)GetCutin(CHANCE_COUNT_CUTIN, () => {
                callback?.Invoke();
            });
            if (cutin == null) return;
            cutin.SetTextList(new List<string>{{count.ToString()}});
            cutin.Show();
        }

        // アイコン表示時カットイン
        public void ShowChargeIconCutin(Sprite sprite, Action callback = null)
        {
            ImageCutin cutin = (ImageCutin)GetCutin(IMAGE_CUTIN, () => {
                callback?.Invoke();
            });
            if (cutin == null) return;
            cutin.SetCutinImage(sprite);
            cutin.Show();
        }

        // 最終決戦固有UIの表示、非表示
        public void SetActiveFBContent(bool b)
        {
            SetCanvasGroupActive(CANVAS_GROUP_FB_CONTENTS, b);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}


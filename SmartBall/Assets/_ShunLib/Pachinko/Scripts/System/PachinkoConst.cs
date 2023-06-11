using System.Collections.Generic;

namespace Pachinko.Const
{
    public class PachinkoConst
    {
        // SE一覧
        public const string SE_REEL_STOP = "se_reel_stop";
        public const string SE_BALL_SHOT = "se_ball_shot";
        
        // パチンコ玉プレハブのタグ名   
        public const string PACHINKO_SPHERE_TAG = "PachiSphere";

        // 保留昇格の昇格順を保持したリスト
        public static readonly List<HoldIconState> chanceUpHoldArray = new List<HoldIconState>(){
            HoldIconState.LEVEL_1,
            HoldIconState.LEVEL_2,
            HoldIconState.LEVEL_3,
            HoldIconState.LEVEL_4,
            HoldIconState.LEVEL_5
        };

        // 最終決戦チャージアイコン昇格の昇格順を保持したリスト
        public static readonly List<FinalBattleChargeIconState> chanceUpFinalBattleIconArray = new List<FinalBattleChargeIconState>(){
            FinalBattleChargeIconState.LEVEL_1,
            FinalBattleChargeIconState.LEVEL_2,
            FinalBattleChargeIconState.LEVEL_3,
            FinalBattleChargeIconState.LEVEL_4,
            FinalBattleChargeIconState.LEVEL_5
        };
    }
    public class PachinkoUIConst
    {
        // カメラ操作ボタン
        public const string CAMERA_BUTTON = "CameraButton";
        public const string CAMERA_BUTTON_1 = "CameraButton1";
        public const string CAMERA_BUTTON_2 = "CameraButton2";
        public const string CAMERA_BUTTON_3 = "CameraButton3";

        // カメラ
        public const string CAMERA_POINT_1 = "CameraPoint1";
        public const string CAMERA_POINT_2 = "CameraPoint2";
        public const string CAMERA_POINT_3 = "CameraPoint3";

        // 動画再生パネル
        public const string BACK_GOURND_PLAYER = "BackGroundPlayer";
        public const string BASE_MODE_FRONT_PLAYER = "BaseModeFrontPlayer";
        public const string FINAL_BATTLE_FRONT_PLAYER = "FinalBattleFrontPlayer";
    }

    // 入賞穴の種類
    public enum StartChuckerState
    {
        NONE = 0,                // なし
        MAIN_HOLE = 1,           // 真ん中の入賞口
        RIGHT_MAIN_HOLE = 2,     // 右打ちの入賞口(保留)
        RIGHT_POINT_HOLE = 3,    // 右打ちの入賞口(ポイント)
    }

    // パチンコの状態
    public enum PachinkoState
    {
        NONE = 0,
        NORMAL_MODE = 1,
        ROUND_MODE = 10,
        FEVER_MODE = 20
    }

    // スロットの状態
    public enum ValueState
    {
        NONE,       // はずれ
        PSEUDO,     // 疑似連
        REACH,      // リーチ
        HIT         // 当たり
    }

    // リーチ演出
    public enum ReachDirectionState
    {
        NONE = 0,
        REACH_PSEUDO_1 = 1,
        REACH_PSEUDO_2 = 2,
        REACH_PSEUDO_3 = 3,
        REACH_PSEUDO_4 = 4,
        ALL_ROTATE = 10,
        REVIVAL = 20
    }

    // レアリティ
    public enum RarityConst
    {
        NONE = 0,
        RARITY_1 = 1,
        RARITY_2 = 2,
        RARITY_3 = 3,
        RARITY_4 = 4,
        RARITY_5 = 5,
        RARITY_6 = 6
    }

    // 予告演出
    public enum NoticeDirectionState
    {
        NONE,
        DIALOG, // 会話演出
        GROUP,  // 群演出
        VIBRATION,  // バイブ
        FREEZE, // フリーズ
        // カットイン
    }

    // 保留の状態
    public enum HoldIconState
    {
        NORMAL = 0,
        LEVEL_1 = 1,
        LEVEL_2 = 2,
        LEVEL_3 = 3,
        LEVEL_4 = 4,
        LEVEL_5 = 5,
        EXTRA_1 = 11,
        EXTRA_2 = 12,
        EXTRA_3 = 13,
        EXTRA_4 = 14,
        EXTRA_5 = 15,
        RIGHT_MODE = 100
    }

    // 最終決戦モードの保留の状態
    public enum FinalBattleChargeIconState
    {
        LEVEL_1 = 1,
        LEVEL_2 = 2,
        LEVEL_3 = 3,
        LEVEL_4 = 4,
        LEVEL_5 = 5
    }

    // ゲームモード
    public enum PachinkoModeState
    {
        NONE,
        NORMAL_MODE,
        NORMAL_FEVER,
        FINAL_BATTLE,
    }

    // ゲームモード選択の条件
    public enum ModeSelectConditionState
    {
        NONE,
        FEVER_COUNT,
        PLAYER_SELECT,
    }
}


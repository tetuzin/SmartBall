using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShunLib.Scenario.Model
{
    [Serializable]
    public class DialogModel
    {
        [SerializeField] public string CharacterName { get; set; }
        [SerializeField] public string MainText { get; set; }
        [SerializeField] public List<DialogActionModel> Actions { get; set; }
    }

    [Serializable]
    public class DialogActionModel
    {
        [SerializeField] public ActionType Type { get; set; }
        [SerializeField] public string FilePath { get; set; }
        [SerializeField] public string StrParam1 { get; set; }
        [SerializeField] public string StrParam2 { get; set; }
        [SerializeField] public string StrParam3 { get; set; }
        [SerializeField] public int NumParam1 { get; set; }
        [SerializeField] public int NumParam2 { get; set; }
        [SerializeField] public int NumParam3 { get; set; }

        /// 各パラメータの詳細 ///

        // STAND_CHARACTER
        // ・FilePath　ファイルパス
        // ・StrParam1　キャラ識別ID
        // ・StrParam2　なし
        // ・StrParam3　なし
        // ・NumParam1　定数[StandCharacterConst]
        // ・NumParam2　表示ポイントのインデックス
        // ・NumParam3　なし
    }

    [Serializable]
    public enum ActionType
    {
        STAND_CHARACTER,
        SE,
        BGM,
        BACKGROUND_IMAGE,
        FADE_OUT,
        FADE_IN,
        FLASH,
    }

    public class StandCharacterConst
    {
        public const int SHOW = 0;
        public const int HIDE = 1;
        public const int MOVE = 2;
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShunLib.Strix.Const
{
    public class StrixConst
    {
        // 入室最大人数の配列
        public static readonly List<int> CAPACITY_VALUE_LIST = new List<int>(){
            2, 4, 8, 12, 20
        };

        // ルームの詳細説明
        public const string ROOM_INFO_DETAILS_TEXT = "RoomInfoDetailsText";

        // メッセージの色
        // 自分
        public const string MESSAGE_COLOR_MYSELF = "<color=red>";
        // 相手
        public const string MESSAGE_COLOR_OPPONENT = "<color=blue>";
        // ルーム入室
        public const string MESSAGE_COLOR_JOIN_ROOM = "<color=green>";
        // ルーム退室
        public const string MESSAGE_COLOR_LEAVE_ROOM = "<color=yellow>";
    }
}


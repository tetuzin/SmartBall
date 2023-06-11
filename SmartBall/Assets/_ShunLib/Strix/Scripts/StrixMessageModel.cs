using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SoftGear.Strix.Client.Core;

namespace ShunLib.Strix.Model.Message
{
    [Serializable]
    public class StrixMessageModel
    {
        // 送信者
        [SerializeField] private string _name = default;
        // メッセージ本文
        [SerializeField] private string _mainText = default;
        // ルームID
        [SerializeField] private long _roomId = default;
        // 送信日時
        [SerializeField] private DateTime _time = default;
        // 送信先
        [SerializeField] private UID _to = default;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string MainText
        {
            get { return _mainText; }
            set { _mainText = value; }
        }

        public long RoomId
        {
            get { return _roomId; }
            set { _roomId = value; }
        }

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public UID To
        {
            get { return _to; }
            set { _to = value; }
        }
    }
}
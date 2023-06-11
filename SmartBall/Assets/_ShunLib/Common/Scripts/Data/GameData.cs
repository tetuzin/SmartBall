using System;
using UnityEngine;

using ShunLib.Model.Room;

namespace ShunLib.Data.Game
{
    [Serializable]
    public class GameData : BaseData
    {
        // ルームインテリアリスト
        [SerializeField] private BaseRoomInteriorModelList _roomInteriorList = default;

        public BaseRoomInteriorModelList RoomInteriorList
        {
            get { return _roomInteriorList; }
            set { _roomInteriorList = value; }
        }
        public void Initialize()
        {
            _roomInteriorList = new BaseRoomInteriorModelList();
        }
    }
}
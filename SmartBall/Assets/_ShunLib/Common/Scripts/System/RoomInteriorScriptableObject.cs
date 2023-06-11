using UnityEngine;

using ShunLib.Dict;

namespace ShunLib.Room
{
    [CreateAssetMenu(fileName = "RoomInteriorScriptableObject", menuName = "ScriptableObjects/Room")]
    public class RoomInteriorScriptableObject : ScriptableObject
    {
        [Header("インテリア一覧")]
        [SerializeField] public RoomInteriorTable roomInteriorTable = new RoomInteriorTable();
    }
}



using UnityEngine;
using ShunLib.Dict;

namespace ShunLib.Lib3D.Block
{
    [CreateAssetMenu(fileName = "BlockScriptableObject", menuName = "ScriptableObjects/Block")]
    public class BlockScriptableObject : ScriptableObject
    {
        [Header("ブロック一覧")]
        [SerializeField] public BaseBlockTable baseBlockTable = new BaseBlockTable();
    }
}


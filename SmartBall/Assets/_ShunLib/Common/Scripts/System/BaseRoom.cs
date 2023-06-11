using UnityEngine;

namespace ShunLib.Room
{
    public class BaseRoom : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("キャラクター生成場所")]
        [SerializeField] protected Transform _spawnCharacterPos = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // キャラクター生成位置を返す
        public Transform GetSpawnCharacterPos()
        {
            return _spawnCharacterPos;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


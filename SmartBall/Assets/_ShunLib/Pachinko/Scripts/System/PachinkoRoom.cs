using UnityEngine;

using ShunLib.Room;

namespace Pachinko.Room
{
    public class PachinkoRoom : BaseRoom
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("パチンコ筐体生成場所")]
        [SerializeField] private Transform _spawnPachinkoPos = default;

        [Header("パチンコ筐体生成場所")]
        [SerializeField] private Transform _spawnPachinkoPos2 = default;

        [Header("ガチャ生成場所")]
        [SerializeField] private Transform _spawnGachaPos = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // キャラクター生成位置を返す
        public Transform GetSpawnPachinkoPos()
        {
            return _spawnPachinkoPos;
        }

        // キャラクター生成位置を返す
        public Transform GetSpawnPachinkoPos2()
        {
            return _spawnPachinkoPos2;
        }

        // ガチャ生成位置を返す
        public Transform GetSpawnGachaPos()
        {
            return _spawnGachaPos;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}



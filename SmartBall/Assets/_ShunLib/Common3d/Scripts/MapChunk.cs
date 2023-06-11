using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Lib3D.Block.Const;

namespace ShunLib.Lib3D.Block.Map
{
    [System.Serializable]
    public class MapChunk : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        // 縦横の幅
        public int Width { get; set; }
        // 高さ
        public int Height { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // ブロック配列
        [SerializeField] private BlockState[,,] _chunkArray = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;
            _chunkArray = new BlockState[height, width, width];
        }

        // 指定位置のブロック種別の取得
        public BlockState GetBlockState(int h, int wx, int wy)
        {
            return _chunkArray[h, wx, wy];
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------

        // デモ用：デモブロック配置
        public Task PlaceDemoBlock()
        {
            for (int h = 0; h <= 60; h++)
            {
                for (int wx = 0; wx < Width; wx++)
                {
                    for (int wy = 0; wy < Width; wy++)
                    {
                        _chunkArray[h, wx, wy] = BlockState.DEMO_BLOCK;
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}


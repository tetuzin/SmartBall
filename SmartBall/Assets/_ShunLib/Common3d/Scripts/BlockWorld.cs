using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Table;
using ShunLib.Utils.Debug;

using ShunLib.Lib3D.Block.Const;
using ShunLib.Lib3D.Block.Map;
using ShunLib.Lib3D.Block.Block;

namespace ShunLib.Lib3D.Block.Wolrd
{
    [System.Serializable]
    public class BlockWorld : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ブロック一覧")]
        [SerializeField] private BlockScriptableObject _blockTable = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // チャンクテーブル
        [SerializeField] private IndexTable<MapChunk> _chunkTable = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public async Task Initialize()
        {
            DebugUtils.Log("新規ワールド生成");

            _chunkTable = new IndexTable<MapChunk>();

            // マップ生成
            await CreateMapChunk(0, 0);
            await CreateNearMapChunk(0, 0);
        }

        // ブロックテーブルの設定
        public void SetBlockTable(BlockScriptableObject blockTable)
        {
            _blockTable = blockTable;
        }

        // 周囲のチャンクを生成
        public async Task CreateNearMapChunk(int x, int y)
        {
            if (!_chunkTable.Check(x, y + 1)) await CreateMapChunk(x, y + 1);
            if (!_chunkTable.Check(x + 1, y + 1)) await CreateMapChunk(x + 1, y + 1);
            if (!_chunkTable.Check(x + 1, y)) await CreateMapChunk(x + 1, y);
            if (!_chunkTable.Check(x + 1, y - 1)) await CreateMapChunk(x + 1, y - 1);
            if (!_chunkTable.Check(x, y - 1)) await CreateMapChunk(x, y - 1);
            if (!_chunkTable.Check(x - 1, y - 1)) await CreateMapChunk(x - 1, y - 1);
            if (!_chunkTable.Check(x - 1, y)) await CreateMapChunk(x - 1, y);
            if (!_chunkTable.Check(x - 1, y + 1)) await CreateMapChunk(x - 1, y + 1);
        }

        // 指定チャンクの描画
        public void ShowMapChunk(int x, int y)
        {
            DebugUtils.Log("チャンクの描画:[" + x + "," + y + "]");

            MapChunk chunk = _chunkTable.Get(x, y);
            // if (chunk == default) return;
            for (int h = 0; h < chunk.Height; h++)
            {
                for (int wx = 0; wx < chunk.Width; wx++)
                {
                    for (int wy = 0; wy < chunk.Width; wy++)
                    {
                        if (!_blockTable.baseBlockTable.IsValue(chunk.GetBlockState(h, wx, wy))) continue;
                        BaseBlock block = _blockTable.baseBlockTable.GetValue(chunk.GetBlockState(h, wx, wy));
                        if (block == null || block == default) continue;
                        Instantiate(
                            block,
                            new Vector3(
                                (x * BlockConst.MAX_CHUNK_COUNT) + wx,
                                h,
                                (x * BlockConst.MAX_CHUNK_COUNT) + wy
                            ),
                            Quaternion.identity
                        );
                    }
                }
            }
        }

        // 新しいチャンクを生成
        public async Task CreateMapChunk(int x, int y)
        {
            DebugUtils.Log("新規チャンク生成:[" + x + "," + y + "]");

            // チャンク生成
            MapChunk newChunk = new MapChunk();
            newChunk.Initialize(
                BlockConst.MAX_CHUNK_COUNT, BlockConst.MAX_CHUNK_HEIGHT_COUNT
            );

            // TODO Demo仮用
            await newChunk.PlaceDemoBlock();

            // 新チャンクを保存
            _chunkTable.Set(x, y, newChunk);
            
            await Task.CompletedTask;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


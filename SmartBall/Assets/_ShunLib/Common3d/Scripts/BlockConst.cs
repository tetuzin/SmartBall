namespace ShunLib.Lib3D.Block.Const
{
    // ブロック関連定数
    public class BlockConst
    {
        // 1チャンクの縦横幅
        public const int MAX_CHUNK_COUNT = 16;
        // 1チャンクの高さ
        public const int MAX_CHUNK_HEIGHT_COUNT = 256;
    }

    // ブロック種別
    public enum BlockState
    {
        NONE = 0,
        DEMO_BLOCK = 1,
    }
}


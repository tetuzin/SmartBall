namespace ShunLib.Utils.Random
{
    public class RandomUtils
    {
        // 乱数の生成スパンが短すぎると同じシード値から同じ乱数が生成されてしまうため、インスタンスを使いまわすことで異なるシード値を使うようにする
        private static System.Random random = null;

        // 乱数を返す( 0 ~ value-1 の範囲)
        public static int GetRandomValue(int value)
        {
            return GetRandom().Next(0, value);
        }

        // ランダムに真偽値を返す(value分の1の確率)
        public static bool GetRandomBool(int value)
        {
            int random = GetRandom().Next(0, value);
            if (random == 0) return true;
            else return false;
        }

        // ランダムクラスのインスタンスを返す
        public static System.Random GetRandom()
        {
            if (random == null)
            {
                random = new System.Random();
            }
            return random;
        }
    }
}



using System;

namespace ShunLib.Utils.Common
{
    public class CommonUtils
    {
        // 値がN以上か
        public static bool CheckValueOrHigher(int compareValue, int originalValue)
        {
            return compareValue <= originalValue;
        }

        // 値がN以下か
        public static bool CheckValueOrLower(int compareValue, int originalValue)
        {
            return compareValue >= originalValue;
        }
        
        // ゲーム終了
        public static void GameExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
            Application.Quit();//ゲームプレイ終了
#endif
        }

        // 一意な文字列を生成
        public static string CreateGUID()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        // 数値が偶数か判定
        public static bool CheckEvenNumber(int value)
        {
            return value % 2 == 0;
        }
    }
}


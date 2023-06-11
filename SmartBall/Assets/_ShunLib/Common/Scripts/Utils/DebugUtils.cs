using System.Reflection;
using System.Collections.Generic;

namespace ShunLib.Utils.Debug
{
    public class DebugUtils
    {
        public static bool isDebug = true;

        // ログ出力を行いたいときに使う関数があるクラス
        // ・通常ログ　：Log関数
        // ・警告ログ　：LogWarning関数
        // ・エラーログ：LogError関数
        // で、それぞれ出力できる

        public static void Log(object obj)
        {
            if (!isDebug) return;
            FieldInfo[] info = obj.GetType().GetFields(
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | 
                BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Default
            );
            string logText = "ログ出力結果 [クラス] " + obj.GetType().Name + 
                "\n 値 : " + obj + "\n\n[フィールド]\n";
            foreach (FieldInfo i in info)
            {
                string text = "[" + i.FieldType.Name + "]" + i.Name + " : 値 " + i.GetValue(obj) + "\n";
                logText += text;
            }
            UnityEngine.Debug.Log(logText);
        }

        public static void Log<Key, Value>(Dictionary<Key, Value> objDict)
        {
            if (!isDebug) return;
            string logText = "ログ出力結果 [Dictionary]\n";
            foreach (var kvp in objDict)
            {
                string text = "Key" + kvp.Key + " : Value" + kvp.Value + "\n";
                logText += text;
            }
            UnityEngine.Debug.Log(logText);
        }

        public static void Log(List<object> objList)
        {
            if (!isDebug) return;
            UnityEngine.Debug.Log("リスト・ログ出力結果[" + objList + "]");
            for (int i = 0; i < objList.Count; i++)
            {
                Log(objList[i]);
            }
            UnityEngine.Debug.Log("リスト・ログ出力結果終了");
        }

        public static void Log(string str)
        {
            if (!isDebug) return;
            UnityEngine.Debug.Log("ログ出力結果\n" + str);
        }

        public static void LogWarning(object obj)
        {
            if (!isDebug) return;
            UnityEngine.Debug.LogWarning(obj);
        }

        public static void LogError(object obj)
        {
            if (!isDebug) return;
            UnityEngine.Debug.LogError(obj);
        }
    }
}
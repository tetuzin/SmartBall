using System.IO;

namespace ShunLib.Utils.File
{
    public class FileUtils
    {
        // ファイルがあるかチェック
        public static bool CheckFile(string path)
        {
            bool result = System.IO.File.Exists(path);

            if (!result)
            {
                UnityEngine.Debug.LogWarning("<color=red>ファイル[\"" + path + "\"]は見つかりません</color>");
            }

            return result;
        }

        // ファイルの読み込み
        public static string LoadFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            string file = reader.ReadToEnd();
            reader.Close();
            return file;
        }

        // ファイルの保存
        public static void SaveFile(string path, string file)
        {
            StreamWriter sw = new StreamWriter(path,false);
            sw.WriteLine(file);
            sw.Flush();
            sw.Close();
        }

        // ファイル新規作成
        public static void CreateFile(string path, string file)
        {
            FileStream fs = System.IO.File.Create(path);
            fs.Flush();
            fs.Close();
            SaveFile(path, file);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

namespace ShunLib.Utils.Json
{
    public class JsonUtils
    {
        // JSONファイルを読み込んでリストにして返す
        public static List<T> Load<T>(string jsonPath)
        {
            List<T> list = new List<T>();
            list.AddRange(JsonToArray<T>(LoadJson(jsonPath)));
            return list;
        }
        
        // オブジェクト(list)をJSONファイル形式で保存する
        public static void SaveJsonList<T>(List<T> list, string fileName)
        {
            // string json = "[" + JsonUtility.ToJson (list) + "]";
            string json = "[" + JsonConvert.SerializeObject(list) + "]";
            SaveJsonFile(json, fileName);
        }

        // オブジェクト(model)をJSONファイル形式で保存する
        public static void SaveJsonModel<T>(T model, string fileName)
        {
            // string json = "[" + JsonUtility.ToJson (model) + "]";
            string json = "[" + JsonConvert.SerializeObject(model) + "]";
            SaveJsonFile(json, fileName);
        }

        public static void SaveJsonFile(string json, string fileName)
        {
            // string path = Application.persistentDataPath + "/" + fileName + ".json";
            string path = "Assets/Resources/" + fileName + ".json";
            StreamWriter sw = new StreamWriter(path,false);
            sw.WriteLine(json);
            sw.Flush();
            sw.Close();
        }

        // PATHからJSONを読み込んで文字列にして返す
        public static string LoadJson(string path)
        {
            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            reader.Close();
            return json;
        }

        // JSON形式を指定した型の配列に変換して返す
        public static T[] JsonToArray<T>(string json)
        {
            string jsonObject = "{\"item\":" + json + "}";
            // Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonObject);
            Wrapper<T> wrapper = JsonConvert.DeserializeObject<Wrapper<T>>(jsonObject);
            return wrapper.item;
        }

        // JSONファイル名からModelのListを生成する
        public static List<T> LoadResourceFile<T>(string file)
        {
            List<T> list = new List<T>();
            if (System.IO.File.Exists(@"Assets/Resources/" + file + ".json"))
            {
                list = ConvertJsonToList<T>(Resources.Load<TextAsset>(file));
            }
            else
            {
                UnityEngine.Debug.LogWarning(file + "は存在しません。");
            }
            return list;
        }

        // JSON(TextAsset)をListにする
        public static List<T> ConvertJsonToList<T>(TextAsset json)
        {
            List<T> list = new List<T>();
            list.AddRange(JsonToArray<T>(json.ToString()));
            return list;
        }

        // JSON(string)を指定クラスにする
        public static T ConvertJsonToClass<T>(string json)
        {
            // return JsonUtility.FromJson<T>(json);
            return JsonConvert.DeserializeObject<T>(json);
        }

        // クラスをJSON(string)にする
        public static string ConvertObjectToJson(object obj)
        {
            // return JsonUtility.ToJson(obj);
            return JsonConvert.SerializeObject(obj);
        }
    }

    [Serializable]
    public class Wrapper<T>
    {
        public T[] item;
    }
}


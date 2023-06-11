using UnityEngine;
using System.Collections.Generic;
using ShunLib.Utils.Json;

namespace ShunLib.Dao
{
    public interface BaseDao
    {
        void Initialize();
        void SetJsonFile(string fileName, TextAsset json);
        void LoadJsonMasterList();
        void SaveJsonMasterList();
    }

    public class BaseDao<T> : BaseDao where T : new()
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        protected List<T> _list = default;
        protected string _jsonFileName = default;
        protected TextAsset _json = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // リストの初期化
        public void Initialize()
        {
            _list = new List<T>();
        }

        // Modelのリストを取得
        public List<T> Get()
        {
            return _list;
        }

        // Modelのリストを設定
        public void Set(List<T> list)
        {
            _list = list;
        }

        // JSONファイルの設定
        public void SetJsonFile(string fileName, TextAsset json)
        {
            _jsonFileName = fileName;
            _json = json;
        }

        // JSONからデータを読み込みリストを返す
        public virtual void LoadJsonMasterList()
        {
            Set(JsonUtils.ConvertJsonToList<T>(_json));
        }

        // リストを読み込みJSONに保存する
        public virtual void SaveJsonMasterList()
        {
            JsonUtils.SaveJsonList<T>(Get(), _jsonFileName);
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}

using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Utils.File;
using ShunLib.Utils.Json;
using ShunLib.Data.Save;

using ShunLib.PlayFab.Manager;
using ShunLib.PlayFab.Utils.User;

namespace ShunLib.Manager.Data
{
    public class DataManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        
        private const string _fileName = "SaveData";
        
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public SaveData Data
        {
            get { return _data; }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        
        private SaveData _data = default;
        private string _filePath = default;
        
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        
        // 初期化
        public async Task Initialize()
        {
            await InitializeUserData();
        }

        // データ保存
        public async Task Save()
        {
            string file = JsonUtils.ConvertObjectToJson(_data);
            FileUtils.SaveFile(_filePath, file);
            if (PlayFabManager.IsInstance())
            {
                await PlayFabUserUtils.UpdateUserDataAsync();
            }
        }

        // データ再読み込み
        public void Load()
        {
            string file = FileUtils.LoadFile(_filePath);
            _data = JsonUtils.ConvertJsonToClass<SaveData>(file);
        }

        // データのJson文字列を返す
        public string GetDataStr()
        {
            return JsonUtils.ConvertObjectToJson(_data);
        }
        
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        
        // データの初期化
        protected async Task InitializeUserData()
        {
            _filePath = GetSaveDataPath() + "/" + _fileName;
            if (FileUtils.CheckFile(_filePath))
            {
                // セーブデータを読み込み
                string file = FileUtils.LoadFile(_filePath);
                SaveData saveData = JsonUtils.ConvertJsonToClass<SaveData>(file);
                _data = saveData;
            }
            else
            {
                // セーブデータを新規作成
                await CreateData();
                string file = JsonUtils.ConvertObjectToJson(_data);
                FileUtils.CreateFile(_filePath, file);
            }
        }

        // データの作成
        protected async Task CreateData()
        {
            SaveData data = new SaveData();
            data.Initialize();
            _data = data;
        }

        // データ保存先
        protected virtual string GetSaveDataPath()
        {
            return Application.persistentDataPath;
        }
    }
}
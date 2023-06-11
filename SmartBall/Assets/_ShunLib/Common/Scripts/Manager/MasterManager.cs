using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Dao;
using ShunLib.Dict;

namespace ShunLib.Manager.Master
{
    public class MasterManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("読み込むJSON一覧")]
        [SerializeField, Tooltip("Daoクラス名の文字列とJSONファイルを設定する")]
        protected TextAssetTable _jsonDict = default;

        [Header("Daoクラスの名前空間")]
        [SerializeField] protected string _namespace = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private Dictionary<string, BaseDao> _daoDict = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public async Task Initialize()
        {
            await InitializeMaster();
        }

        // Dao取得
        public BaseDao GetDao(string daoName)
        {
            return _daoDict[daoName];
        }
        
        // ---------- Private関数 ----------
        
        // マスタ配列の初期化
        private Task InitializeMaster()
        {
            if (_jsonDict == default || _namespace == default) return Task.CompletedTask;

            _daoDict = new Dictionary<string, BaseDao>();
            foreach (string daoName in GetDaoClassNameList())
            {
                Type daoType = Type.GetType(_namespace + daoName, true);
                BaseDao dao = (BaseDao)Activator.CreateInstance(daoType);
                TextAsset json = _jsonDict.GetValue(daoName);
                dao.SetJsonFile(json.name, json);
                dao.LoadJsonMasterList();
                _daoDict.Add(daoName, dao);
            }

            return Task.CompletedTask;
        }

        // DAOクラス名の配列を返す
        private List<string> GetDaoClassNameList()
        {
            List<string> daoClassNameList = new List<string>();
            foreach (var kvp in _jsonDict.GetTable())
            {
                daoClassNameList.Add(kvp.Key);
            }
            return daoClassNameList;
        }
        
        // ---------- protected関数 ---------
    }
}
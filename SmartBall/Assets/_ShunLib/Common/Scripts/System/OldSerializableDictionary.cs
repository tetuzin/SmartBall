using UnityEngine;
using System.Collections.Generic;

namespace ShunLib.Dict {

    /// <summary>
    /// テーブルの管理クラス
    /// </summary>
    [System.Serializable]
    public class OldTableBase<TKey, TValue, Type> where Type : OldKeyAndValue<TKey, TValue>{
        [SerializeField]
        private List<Type> list;
        private Dictionary<TKey, TValue> table;


        public Dictionary<TKey, TValue> GetTable () {
            if (table == null) {
                table = ConvertListToDictionary(list);
            }
            return table;
        }

        /// <summary>
        /// Editor Only
        /// </summary>
        public List<Type> GetList () {
            return list;
        }

        static Dictionary<TKey, TValue> ConvertListToDictionary (List<Type> list) {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue> ();
            foreach(OldKeyAndValue<TKey, TValue> pair in list){
                dic.Add(pair.Key, pair.Value);
            }
            return dic;
        }
    }

    /// <summary>
    /// シリアル化できる、KeyValuePair
    /// </summary>
    [System.Serializable]
    public class OldKeyAndValue<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public OldKeyAndValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public OldKeyAndValue(KeyValuePair<TKey, TValue> pair)
        {
            Key = pair.Key;
            Value = pair.Value;
        }


    }
}
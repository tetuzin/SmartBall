using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace ShunLib.Dict
{
    [System.Serializable][SerializeField]
    public class TableBase<TKey, TValue, TPair> where TPair : KeyAndValue<TKey, TValue> , new ()
    {
        [SerializeField] protected List<TPair> list;                     
        protected Dictionary<TKey, TValue> table;

        public TableBase ()
        {
            list = new List<TPair>();
        }

        public Dictionary<TKey, TValue> GetTable ()
        {
            if (table == null) {
                table = ConvertListToDictionary(list);
            }
            return table;
        }

        public bool IsValue(TKey key)
        {
            return GetTable().Keys.Contains(key);
        }

        public TValue GetValue (TKey key)
        {
            if(GetTable ().Keys.Contains(key)){
                return GetTable ()[key];
            }
            Debug.LogError(key +" は存在しないKeyです");
            return default(TValue);
        }

        public TValue[] GetValueArray()
        {
            TValue[] valueArray = new TValue[GetTable().Values.Count];
            table.Values.CopyTo(valueArray, 0);
            return valueArray;
        }

        public List<TValue> GetValueList()
        {
            TValue[] valueArray = GetValueArray();
            return new List<TValue>(valueArray);
        }

        public TKey[] GetKeyArray()
        {
            TKey[] keyArray = new TKey[GetTable().Keys.Count];
            table.Keys.CopyTo(keyArray, 0);
            return keyArray;
        }

        public List<TKey> GetKeyList()
        {
            TKey[] keyArray = GetKeyArray();
            return new List<TKey>(keyArray);
        }

        public void SetValue (TKey key, TValue value)
        {
            if(GetTable ().Keys.Contains(key))
            {
                Debug.Log ("SetValue() Change Value.");
                table[key] = value;
            }
            else
            {
                Debug.Log ("SetValue() Add new table.");
                table.Add(key, value);
            }
        }

        public void RemovePair(TKey key)
        {
            if(GetTable ().Keys.Contains(key))
            {
                GetTable().Remove(key);
            }
        }

        public void Reset ()
        {
            table = new Dictionary<TKey, TValue>();
            list = new List<TPair>();
        }
        public void Apply ()
        {
            list = ConvertDictionaryToList(table);
        }

        static Dictionary<TKey, TValue> ConvertListToDictionary (List<TPair> list)
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue> ();
            foreach(KeyAndValue<TKey, TValue> pair in list)
            {
                dic.Add(pair.Key, pair.Value);
            }
            return dic;
        }

        static List<TPair> ConvertDictionaryToList (Dictionary<TKey, TValue> table)
        {
            List<TPair> list = new List<TPair>();

            if(table != null)
            {
                foreach(KeyValuePair<TKey, TValue> pair in table)
                {
                    TPair type = new TPair();
                    type.Key = pair.Key;
                    type.Value = pair.Value;
                    list.Add(type);
                }
            }
            return list;
        }
    }

    /// <summary>
    /// シリアル化できる、KeyValuePairに代わる構造体
    /// </summary>
    [System.Serializable]
    public class KeyAndValue<TKey, TValue>
    {
        [SerializeField] public TKey Key;
        [SerializeField] public TValue Value;
    }
}
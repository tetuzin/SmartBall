using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Utils.Debug;

namespace ShunLib.Table
{
    [System.Serializable]
    public class IndexTable<T>
    {
        // T型の二次元テーブル
        [SerializeField] private Dictionary<int, Dictionary<int, T>> _table = new Dictionary<int, Dictionary<int, T>>();

        // 値の取得
        public T Get(int x, int y)
        {
            if (Check(x, y))
            {
                return _table[x][y];
            }
            else
            {
                return default;
            }
        }

        // 値の設定
        public void Set(int x, int y, T value)
        {
            if (Check(x, y))
            {
                _table[x][y] = value;
            }
            else
            {
                Add(x, y, value);
            }
        }

        // 値の追加
        public void Add(int x, int y, T value)
        {
            if (_table.ContainsKey(x))
            {
                if (_table[x].ContainsKey(y))
                {
                    _table[x][y] = value;
                }
                else
                {
                    _table[x].Add(y, value);
                }
            }
            else
            {
                Dictionary<int, T> newTable = new Dictionary<int, T>();
                newTable.Add(y, value);
                _table.Add(x, newTable);
            }
        }

        // 値のチェック
        public bool Check(int x, int y)
        {
            if (!_table.ContainsKey(x) || !_table[x].ContainsKey(y))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // テーブルの中身をログ出力
        public void Log()
        {
            DebugUtils.Log<int, Dictionary<int, T>>(_table);
        }
    }
}


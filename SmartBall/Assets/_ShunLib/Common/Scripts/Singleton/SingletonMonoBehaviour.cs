using UnityEngine;
using System;

namespace ShunLib.Singleton
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (m_sync_obj)
                    {
                        if (_instance == null)
                        {
                            Type t = typeof(T);
                            _instance = (T)FindObjectOfType (t);
                        }
                    }
                }
                return _instance;
            }
        }
        
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // シングルトンパターン用変数
        private static T _instance = default;
        // デッドロック回避用インスタンス
        private static object m_sync_obj = new object (); 

        // ---------- Unity組込関数 ----------

        protected virtual void Awake()
        {
            // DontDestroyOnLoad(this.gameObject);
            CheckInstance();
        }

        // ---------- Public関数 ----------

        public static bool IsInstance()
        {
            return _instance != default && _instance != null;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // コンストラクタ
        protected SingletonMonoBehaviour() {}

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                _instance = null;
            }
        }

        protected virtual bool CheckInstance()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
                return true;
            }
            else if(Instance == this)
            {
                return true;
            }
            return false;
        }
    }
}


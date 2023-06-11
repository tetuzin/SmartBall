using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShunLib.Controller.InputKey
{
    public class InputKeyController : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public bool EnableKeyCtrl { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        
        // キーボードが押され続けているときに実行する関数の連想配列
        private Dictionary<KeyCode, UnityAction> _keyStayActions = default;
        // キーボードが押されたときに実行する関数の連想配列
        private Dictionary<KeyCode, UnityAction> _keyDownActions = default;
        // キーボードが離されたときに実行する関数の連想配列
        private Dictionary<KeyCode, UnityAction> _keyUpActions = default;

        // ---------- Unity組込関数 ----------
        
        void Update()
        {
            if (!EnableKeyCtrl) return;
            
            // キーボードが押され続けている時
            if (_keyStayActions.Count > 0)
            {
                Dictionary<KeyCode, UnityAction> dummyDict = new Dictionary<KeyCode, UnityAction>(_keyStayActions);
                foreach (KeyValuePair<KeyCode, UnityAction> actions in dummyDict)
                {
                    if (Input.GetKey(actions.Key))
                    {
                        actions.Value();
                    }
                }
            }

            // キーボードが押された時
            if (_keyDownActions.Count > 0)
            {
                Dictionary<KeyCode, UnityAction> dummyDict = new Dictionary<KeyCode, UnityAction>(_keyDownActions);
                foreach (KeyValuePair<KeyCode, UnityAction> actions in dummyDict)
                {
                    if (Input.GetKeyDown(actions.Key))
                    {
                        actions.Value();
                    }
                }
            }
            
            // キーボードが離された時
            if (_keyUpActions.Count > 0)
            {
                Dictionary<KeyCode, UnityAction> dummyDict = new Dictionary<KeyCode, UnityAction>(_keyUpActions);
                foreach (KeyValuePair<KeyCode, UnityAction> actions in dummyDict)
                {
                    if (Input.GetKeyUp(actions.Key))
                    {
                        actions.Value();
                    }
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            EnableKeyCtrl = false;
            _keyStayActions = new Dictionary<KeyCode, UnityAction>();
            _keyDownActions = new Dictionary<KeyCode, UnityAction>();
            _keyUpActions = new Dictionary<KeyCode, UnityAction>();
        }

        // キーボードが押され続けているときの処理を追加する
        public void AddKeyStayAction(KeyCode key, UnityAction action)
        {
            if (_keyStayActions.ContainsKey(key))
            {
                _keyStayActions[key] += action;
            }
            else
            {
                _keyStayActions.Add(key, action);
            }
        }

        // キーボードが押されたときの処理を追加する
        public void AddKeyDownAction(KeyCode key, UnityAction action)
        {
            if (_keyDownActions.ContainsKey(key))
            {
                _keyDownActions[key] += action;
            }
            else
            {
                _keyDownActions.Add(key, action);
            }
        }

        // キーボードが離されたときの処理を追加する
        public void AddKeyUpAction(KeyCode key, UnityAction action)
        {
            if (_keyUpActions.ContainsKey(key))
            {
                _keyUpActions[key] += action;
            }
            else
            {
                _keyUpActions.Add(key, action);
            }
        }

        // キーボードが押され続けているときの処理を設定する
        public void SetKeyStayAction(KeyCode key, UnityAction action)
        {
            if (_keyStayActions.ContainsKey(key))
            {
                RemoveKeyStayAction(key);
                _keyStayActions[key] = action;
            }
            else
            {
                _keyStayActions.Add(key, action);
            }
        }

        // キーボードが押されたときの処理を設定する
        public void SetKeyDownAction(KeyCode key, UnityAction action)
        {
            if (_keyDownActions.ContainsKey(key))
            {
                RemoveKeyDownAction(key);
                _keyDownActions[key] = action;
            }
            else
            {
                _keyDownActions.Add(key, action);
            }
        }

        // キーボードが離されたときの処理を設定する
        public void SetKeyUpAction(KeyCode key, UnityAction action)
        {
            if (_keyUpActions.ContainsKey(key))
            {
                RemoveKeyUpAction(key);
                _keyUpActions[key] = action;
            }
            else
            {
                _keyUpActions.Add(key, action);
            }
        }

        // キーボードが押され続けているときの処理を削除する
        public void RemoveKeyStayAction(KeyCode key)
        {
            if (_keyStayActions.ContainsKey(key))
            {
                _keyStayActions.Remove(key);
            }
        }

        // キーボードが押されたときの処理を削除する
        public void RemoveKeyDownAction(KeyCode key)
        {
            if (_keyDownActions.ContainsKey(key))
            {
                _keyDownActions.Remove(key);
            }
        }

        // キーボードが離されたときの処理を削除する
        public void RemoveKeyUpAction(KeyCode key)
        {
            if (_keyUpActions.ContainsKey(key))
            {
                _keyUpActions.Remove(key);
            }
        }

        // キーボードが押され続けているときの処理を取得する
        public UnityAction GetKeyStayAction(KeyCode key)
        {
            if (_keyStayActions.ContainsKey(key))
            {
                return _keyStayActions[key];
            }
            return null;
        }

        // キーボードが押されたときの処理を取得する
        public UnityAction GetKeyDownAction(KeyCode key)
        {
            if (_keyDownActions.ContainsKey(key))
            {
                return _keyDownActions[key];
            }
            return null;
        }

        // キーボードが離されたときの処理を取得する
        public UnityAction GetKeyUpAction(KeyCode key)
        {
            if (_keyUpActions.ContainsKey(key))
            {
                return _keyUpActions[key];
            }
            return null;
        }

        // 一回きりのキーボードが押され続けているときの処理を設定する
        public void SetOnlyOnceKeyStayAction(KeyCode key, UnityAction action)
        {
            UnityAction beforeAction = null;
            if (_keyStayActions.ContainsKey(key))
            {
                beforeAction = _keyStayActions[key];
            }
            SetKeyStayAction(key, () => {
                action?.Invoke();
                SetKeyStayAction(key, beforeAction);
            });
        }

        // 一回きりのキーボードが押されたときの処理を設定する
        public void SetOnlyOnceKeyDownAction(KeyCode key, UnityAction action)
        {
            UnityAction beforeAction = null;
            if (_keyDownActions.ContainsKey(key))
            {
                beforeAction = _keyDownActions[key];
            }
            SetKeyDownAction(key, () => {
                action?.Invoke();
                SetKeyDownAction(key, beforeAction);
            });
        }

        // 一回きりのキーボードが離されたときの処理を設定する
        public void SetOnlyOnceKeyUpAction(KeyCode key, UnityAction action)
        {
            UnityAction beforeAction = null;
            if (_keyUpActions.ContainsKey(key))
            {
                beforeAction = _keyUpActions[key];
            }
            SetKeyUpAction(key, () => {
                action?.Invoke();
                SetKeyUpAction(key, beforeAction);
            });
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}


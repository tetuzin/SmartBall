using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShunLib.Controller.InputMouse
{
    public class InputMouseController : MonoBehaviour
    {
        // ---------- 定数宣言 ----------

        // マウス左ボタン
        private const int MOUSE_BOTTON_LEFT = 0;
        // マウス右ボタン
        private const int MOUSE_BOTTON_RIGHT = 1;
        // マウス中ボタン
        private const int MOUSE_BOTTON_MIDDLE = 2;
        // マウスボタンの個数
        private const int MOUSE_BOTTON_COUNT = 3;

        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // マウスボタンが押され続けているときの連想配列
        private Dictionary<int, UnityAction> _bottonStayActions = default;
        // マウスボタンが押されたときの連想配列
        private Dictionary<int, UnityAction> _bottonDownActions = default;
        // マウスボタンが離されたときの連想配列
        private Dictionary<int, UnityAction> _bottonUpActions = default;

        // ---------- Unity組込関数 ----------

        void Update()
        {
            // マウスボタンが押され続けているとき
            if (_bottonStayActions.Count != 0)
            {
                for(int i = 0; i < MOUSE_BOTTON_COUNT; i++)
                {
                    if (Input.GetMouseButton(i))
                    {
                        _bottonStayActions[i]();
                    }
                }
            }

            // マウスボタンが押されたとき
            if (_bottonDownActions.Count != 0)
            {
                for(int i = 0; i < MOUSE_BOTTON_COUNT; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        _bottonDownActions[i]();
                    }
                }
            }

            // マウスボタンが離されたとき
            if (_bottonUpActions.Count != 0)
            {
                for(int i = 0; i < MOUSE_BOTTON_COUNT; i++)
                {
                    if (Input.GetMouseButtonUp(i))
                    {
                        _bottonUpActions[i]();
                    }
                }
            }
        }

        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _bottonStayActions = new Dictionary<int, UnityAction>();
            _bottonDownActions = new Dictionary<int, UnityAction>();
            _bottonUpActions = new Dictionary<int, UnityAction>();
        }

        // マウスボタンが押され続けているときの関数を設定
        public void SetButtonStayAction(int mouseBtn, UnityAction action)
        {
            if (mouseBtn < 0 || mouseBtn > 2) { return; }
            if (_bottonStayActions.ContainsKey(mouseBtn))
            {
                _bottonStayActions[mouseBtn] = action;
            }
            else
            {
                _bottonStayActions.Add(mouseBtn, action);
            }
        }

        // マウスボタンが押されたときの関数を設定
        public void SetButtonDownAction(int mouseBtn, UnityAction action)
        {
            if (mouseBtn < 0 || mouseBtn > 2) { return; }
            if (_bottonDownActions.ContainsKey(mouseBtn))
            {
                _bottonDownActions[mouseBtn] = action;
            }
            else
            {
                _bottonDownActions.Add(mouseBtn, action);
            }
        }

        // マウスボタンが離されたときの関数を設定
        public void SetButtonUpAction(int mouseBtn, UnityAction action)
        {
            if (mouseBtn < 0 || mouseBtn > 2) { return; }
            if (_bottonUpActions.ContainsKey(mouseBtn))
            {
                _bottonUpActions[mouseBtn] = action;
            }
            else
            {
                _bottonUpActions.Add(mouseBtn, action);
            }
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}


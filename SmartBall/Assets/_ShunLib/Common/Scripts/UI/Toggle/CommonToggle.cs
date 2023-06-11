using System;
using UnityEngine;
using UnityEngine.UI;

namespace ShunLib.Toggle.Common
{
    public class CommonToggle : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("トグル用ボタン")]
        [SerializeField] protected Button _toggleBtn = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public bool IsOn
        {
            get { return _isOn; }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        protected bool _isOn = default;
        protected Action _isOnAction = default;
        protected Action _isOffAction = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public virtual void Initialize()
        {
            _toggleBtn.onClick.RemoveAllListeners();
            _toggleBtn.onClick.AddListener(() => {
                SwitchToggle();
            });
        }

        // 値設定
        public virtual void SetValue(bool value)
        {
            _isOn = value;
            DoAction(_isOn);
        }

        // オンオフ切替
        public virtual void SwitchToggle()
        {
            _isOn = !_isOn;
            DoAction(_isOn);
        }

        // オン時イベントの設定
        public virtual void SetIsOnAction(Action action)
        {
            _isOnAction = action;
        }

        // オフ時イベントの設定
        public virtual void SetIsOffAction(Action action)
        {
            _isOffAction = action;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // オンオフ処理実行
        protected virtual void DoAction(bool value)
        {
            if (value) 
            {
                _isOnAction?.Invoke();
            }
            else
            {
                _isOffAction?.Invoke();
            }
        }
    }
}
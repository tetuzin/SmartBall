using UnityEngine;

using ShunLib.Toggle.Common;

namespace ShunLib.Toggle.Btn
{
    public class ButtonToggle : CommonToggle
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ON時オブジェクト")]
        [SerializeField] protected GameObject _onObj = default;
        [Header("OFF時オブジェクト")]
        [SerializeField] protected GameObject _offObj = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public override void Initialize()
        {
            base.Initialize();
            SwitchShowObject(false);
        }

        // ---------- Private関数 ----------

        private void SwitchShowObject(bool b)
        {
            _onObj.SetActive(b);
            _offObj.SetActive(!b);
        }

        // ---------- protected関数 ---------

        // オンオフ処理実行
        protected override void DoAction(bool value)
        {
            if (value) 
            {
                SwitchShowObject(value);
                _isOnAction?.Invoke();
            }
            else
            {
                SwitchShowObject(value);
                _isOffAction?.Invoke();
            }
        }
    }
}
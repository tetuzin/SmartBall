using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ShunLib.UI.DropDown.Common
{
    public class CommonDropDown : TMP_Dropdown
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private List<string> _stringItems = default;
        private List<Sprite> _spriteItems = default;

        // ---------- Unity組込関数 ----------

        protected override void Awake() { Initialize(); }

        // ---------- Public関数 ----------

        // 値設定(テキスト)
        public void AddItems(List<string> items)
        {
            AddOptions(items);
            _stringItems = items;
        }

        // 値設定(画像)
        public void AddItems(List<Sprite> items)
        {
            AddOptions(items);
            _spriteItems = items;
        }

        // 現在の値を取得
        public string GetSelectItem()
        {
            return options[value].text;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        
        // 初期化
        private void Initialize()
        {
            InitItem();
        }

        // 値初期化
        private void InitItem()
        {
            ClearOptions();
        }
    }
}
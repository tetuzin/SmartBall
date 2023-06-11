using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ShunLib.Btn.Common;

namespace ShunLib.PokeApi
{
    public class PokemonIcon : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ボタン")]
        [SerializeField] private CommonButton _button = default;

        [Header("背景画像")]
        [SerializeField] private Image _backImage = default;

        [Header("アイコン画像")]
        [SerializeField] private Image _iconImage = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private Pokemon _pokemonData = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(Pokemon pokemon, Sprite sprite)
        {
            _pokemonData = pokemon;
            _iconImage.sprite = sprite;
        }

        // ボタン押下時イベントの設定
        public void SetOnClickButtonAction(Action<Pokemon> action)
        {
            _button.SetOnEvent(() => {
                action?.Invoke(_pokemonData);
            });
        }

        // ボタン長押し時イベントの設定
        public void SetOnDownButtonAction(Action<Pokemon> action)
        {
            _button.SetOnDownEvent(() => {
                action?.Invoke(_pokemonData);
            });
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


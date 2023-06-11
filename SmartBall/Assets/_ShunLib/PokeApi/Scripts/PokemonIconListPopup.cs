using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Popup.ScrollView;
using ShunLib.Utils.Request;

namespace ShunLib.PokeApi
{
    public class PokemonIconListPopup : ScrollViewPopup
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [Header("ポケモンアイコンプレハブ")]
        [SerializeField] private PokemonIcon _iconPrefab = default;

        [Header("該当なしテキスト")]
        [SerializeField] private GameObject _nonTextObj = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------
        // ---------- Private関数 ----------
        // ---------- protected関数 ---------

        // 初期化
        protected async override void Initialize()
        {
            base.Initialize();

            // TODO 仮
            for (int i = 1; i < 21; i++)
            {
                Pokemon poke = await PokeApiRequest.GetPokemonAsync(i);
                PokemonIcon icon = Instantiate(_iconPrefab);
                Sprite sprite = await RequestUtils.GetSpriteAsync(poke.Sprite.FrontMale);
                icon.Initialize(poke, sprite);
                SetContent(icon.gameObject);
            }
        }

        // ---------- デバッグ用関数 ---------
    }
}


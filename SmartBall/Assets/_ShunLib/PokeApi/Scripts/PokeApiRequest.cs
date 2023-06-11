using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShunLib.Utils.Debug;
using ShunLib.Utils.Request;

namespace ShunLib.PokeApi
{
    public class PokeApiRequest
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 指定ApiUrlを実行
        public static async Task<T> GetApiAsync<T>(string url)
        {
            return await RequestUtils.GetDataAsync<T>(url);
        }

        // 図鑑番号からポケモンデータを取得
        public static async Task<Pokemon> GetPokemonAsync(int id)
        {
            string apiUrl = PokeApiConst.GET_POKEMON_URL + id;
            Pokemon result = await RequestUtils.GetDataAsync<Pokemon>(apiUrl);
            DebugUtils.Log(result);
            return result;
        }

        // ポケモン名からポケモンデータを取得
        public static async Task<Pokemon> GetPokemonAsync(string name)
        {
            string apiUrl = PokeApiConst.GET_POKEMON_URL + name;
            Pokemon result = await RequestUtils.GetDataAsync<Pokemon>(apiUrl);
            return result;
        }

        // 全ポケモンの一覧を取得
        public static async Task<PokemonList> GetPokemonListAsync()
        {
            string apiUrl = PokeApiConst.GET_ALL_POKEMON_LIST_URL;
            PokemonList result = await RequestUtils.GetDataAsync<PokemonList>(apiUrl);
            return result;
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
        // ---------- デバッグ用関数 ---------
    }
}


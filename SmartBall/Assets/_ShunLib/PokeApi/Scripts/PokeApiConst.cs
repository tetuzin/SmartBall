using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShunLib.PokeApi
{
    public class PokeApiConst
    {
        // ポケモンを取得する
        public const string GET_POKEMON_URL = "https://pokeapi.co/api/v2/pokemon/";
        // 全ポケモンリストを取得する
        public const string GET_ALL_POKEMON_LIST_URL = "https://pokeapi.co/api/v2/pokemon?limit=100000&offset=0";
    }
}
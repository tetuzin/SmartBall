using Newtonsoft.Json;
using System.Collections.Generic;

namespace ShunLib.PokeApi
{
    [System.Serializable]
    public class PokeApiModel { }

    // ポケモン
    [System.Serializable]
    public class Pokemon
    {
        // 図鑑番号
        [JsonProperty("id")]
        public long Id { get; set; }

        // 名前
        [JsonProperty("name")]
        public string Name { get; set; }

        // 高さ
        [JsonProperty("height")]
        public int Height { get; set; }

        // 重さ
        [JsonProperty("weight")]
        public int Weight { get; set; }

        // 基本経験値
        [JsonProperty("base_experience")]
        public int BaseExperience { get; set; }

        // 種族データ
        [JsonProperty("species")]
        public SimpleData Species { get; set; }

        // 画像
        [JsonProperty("sprites")]
        public PokemonSprite Sprite { get; set; }

        // フォルム
        [JsonProperty("forms")]
        public List<SimpleData> Forms { get; set; }

        // タイプ
        [JsonProperty("types")]
        public List<PokemonType> Types { get; set; }

        // 覚える技
        [JsonProperty("moves")]
        public List<PokemonSkill> Skills { get; set; }

        // 特性
        [JsonProperty("abilities")]
        public List<PokemonAbility> Abilities { get; set; }

        // ステータス
        [JsonProperty("stats")]
        public List<PokemonStats> Stats { get; set; }
    }

    // ポケモンリスト
    [System.Serializable]
    public class PokemonList
    {
        // ポケモンの数
        [JsonProperty("count")]
        public long Count { get; set; }

        // ポケモンのデータ
        [JsonProperty("results")]
        public List<SimpleData> Results { get; set; }
    }

    // ポケモンのタイプ
    [System.Serializable]
    public class PokemonType
    {
        // タイプ数
        [JsonProperty("slot")]
        public long Slot { get; set; }

        // タイプのデータ
        [JsonProperty("type")]
        public SimpleData Type { get; set; }
    }

    // ポケモンの画像
    [System.Serializable]
    public class PokemonSprite
    {
        // 正面画像URL(オス)
        [JsonProperty("front_default")]
        public string FrontMale { get; set; }
        // 正面画像URL(メス)
        [JsonProperty("front_female")]
        public string FrontFemale { get; set; }
        // 背面画像URL(オス)
        [JsonProperty("back_default")]
        public string BackMale { get; set; }
        // 背面画像URL(メス)
        [JsonProperty("back_female")]
        public string BackFemale { get; set; }

        // 正面画像URL(オス:色違い)
        [JsonProperty("front_shiny")]
        public string FrontMaleShiny { get; set; }
        // 正面画像URL(メス:色違い)
        [JsonProperty("front_shiny_female")]
        public string FrontFemaleShiny { get; set; }
        // 背面画像URL(オス:色違い)
        [JsonProperty("back_shiny")]
        public string BackMaleShiny { get; set; }
        // 背面画像URL(メス:色違い)
        [JsonProperty("back_shiny_female")]
        public string BackFemaleShiny { get; set; }
    }

    // ポケモンの技
    [System.Serializable]
    public class PokemonSkill
    {
        // 技データ
        [JsonProperty("move")]
        public SimpleData Data { get; set; }
    }

    // ポケモンの技（詳細）
    [System.Serializable]
    public class PokemonSkillDetails
    {
        
    }

    // ポケモンの特性
    [System.Serializable]
    public class PokemonAbility
    {
        // 特性データ
        [JsonProperty("ability")]
        public SimpleData Data { get; set; }

        // 隠れ特性フラグ
        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }

        // 番号
        [JsonProperty("slot")]
        public bool Slot { get; set; }
    }

    // ポケモンのステータス
    [System.Serializable]
    public class PokemonStats
    {
        // 種族値
        [JsonProperty("base_stat")]
        public string BaseStats { get; set; }

        // 努力値
        [JsonProperty("effort")]
        public string BaseEffort { get; set; }
    }

    // 簡易データモデル
    [System.Serializable]
    public class SimpleData
    {
        // 名前
        [JsonProperty("name")]
        public string Name { get; set; }

        // 詳細データのURL
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}



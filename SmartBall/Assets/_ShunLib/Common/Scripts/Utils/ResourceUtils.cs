using UnityEngine;

namespace ShunLib.Utils.Resource
{
    public class ResourceUtils
    {
        // 画像ファイルパス名からTextureを取得
        public static Texture2D GetTexture2D(string path)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return ConvertTexture(bytes);
        }

        // バイト配列をTextureに変換
        public static Texture2D ConvertTexture(byte[] array)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(array);
            return texture;
        }

        // 画像ファイル名からSpriteを取得
        public static Sprite GetSprite(string path)
        {
            Texture2D texture = GetTexture2D(path);
            return ConvertSprite(texture);
        }

        // TextureをSpriteに変換
        public static Sprite ConvertSprite(Texture2D texture)
        {
            Rect rect = new Rect(0f, 0f, texture.width, texture.height);
            Sprite sprite = Sprite.Create(texture, rect, Vector2.zero);
            return sprite;
        }

        /// ----- Resources ----- ///
        // Resourcesフォルダ以下のPathを指定する。拡張子は含めない

        // ResourcesフォルダからGameObjectを取得
        public static GameObject GetResourcesObject(string path)
        {
            return (GameObject)Resources.Load(path);
        }

        // ResourcesフォルダからTextAssetを取得
        public static TextAsset GetResourcesTextAsset(string path)
        {
            return Resources.Load<TextAsset>(path);
        }

        // ResourcesフォルダからJSON文字列を取得
        public static string GetResourcesJson(string path)
        {
            return Resources.Load<TextAsset>(path).text;
        }

        // ResourcesフォルダからSpriteを取得
        public static Sprite GetResourcesSprite(string path)
        {
            return Resources.Load<Sprite>(path);
        }

        // ResourcesフォルダからSpriteを一括取得
        public static Sprite[] GetResourcesAllSprite(string path)
        {
            return Resources.LoadAll<Sprite>(path);
        }

        // 画像ファイル格納パスを返す
        public static string GetTexturePath(string path)
        {
            return "Assets/Texture/" + path + ".jpg";
        }
    }
}


using System;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

using ShunLib.Utils.Debug;
using ShunLib.Utils.Json;
using ShunLib.Utils.Resource;

namespace ShunLib.Utils.Request
{
    public class RequestUtils
    {
        // 取得したJSONをクラスに変換して返す
        public static async Task<T> GetDataAsync<T>(string url)
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(url);

            if (!result.IsSuccessStatusCode)
            {
                DebugUtils.LogWarning(result.StatusCode.ToString());
                return default;
            }

            try
            {
                using (var content = result.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        T value = JsonUtils.ConvertJsonToClass<T>(data);
                        return value;
                    }
                }
            }
            catch (Exception ex)
            {
                DebugUtils.LogWarning(ex.Message);
            }
            return default;
        }

        // バイト配列を取得して返す
        public static async Task<byte[]> GetByteArrayAsync(string url)
        {
            HttpClient client = new HttpClient();
            byte[] result = await client.GetByteArrayAsync(url);
            if (result.Length > 0)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        // 取得した画像をSpriteに変換して返す
        public static async Task<Sprite> GetSpriteAsync(string url)
        {
            byte[] byteArray = await GetByteArrayAsync(url);
            return ResourceUtils.ConvertSprite(ResourceUtils.ConvertTexture(byteArray));
        }
    }
}


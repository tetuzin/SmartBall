using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using ShunLib.Manager.Game;
using ShunLib.PlayFab.Utils;

namespace ShunLib.PlayFab.Utils.Item
{
    public class PlayFabItemUtils : PlayFabBaseUtils
    {
        // カタログリスト
        public static List<CatalogItem> catalogItems = new List<CatalogItem>();
        // ストアリスト
        public static List<StoreItem> storeItems = new List<StoreItem>();

        // カタログとストアの事前読み込み
        public static async Task LoadCatalogAndStoreAsync(string storeId, string catalogVersion = null)
        {
            await GetCatalogItemsAsync(catalogVersion, result => catalogItems = result.Catalog);
            await GetStoreItemsAsync(storeId, catalogVersion, result => storeItems = result.Store);
        }

        // カタログの取得
        public static async Task GetCatalogItemsAsync(
            string catalogVersion = null, Action<GetCatalogItemsResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new GetCatalogItemsRequest
            {
                CatalogVersion = catalogVersion
            };
            var response = await PlayFabClientAPI.GetCatalogItemsAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("カタログの取得成功");
                successCallback?.Invoke(response.Result);
            }
        }

        // ストアの取得
        public static async Task GetStoreItemsAsync(
            string storeId, string catalogVersion = null, Action<GetStoreItemsResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new GetStoreItemsRequest
            {
                CatalogVersion = catalogVersion,
                StoreId = storeId
            };
            var response = await PlayFabClientAPI.GetStoreItemsAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("ストア[" + storeId + "]の取得成功");
                successCallback?.Invoke(response.Result);
            }
        }

        // アイテムの消費
        public static async Task ConsumeItemAsync(
            string characterId, string itemId, int count, Action<ConsumeItemResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            var request = new ConsumeItemRequest
            {
                ConsumeCount = count,       // 消費数
                ItemInstanceId = itemId,    // アイテムインスタンスID
                CharacterId = characterId   // キャラクターID
            };
            var response = await PlayFabClientAPI.ConsumeItemAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("アイテム[" + itemId + "]を" + count + "個消費しました");
                successCallback?.Invoke(response.Result);
            }
        }

        // アイテムの購入
        public static async Task PurchaseItemAsync(
            string characterId, string catalogVersion, string storeId, string itemId, int price, string vm,
            Action<PurchaseItemResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            PurchaseItemRequest request;
            if (characterId == null)
            {
                request = new PurchaseItemRequest
                {
                    Price = price,  // 価格
                    CatalogVersion = catalogVersion,    // カタログバージョン
                    StoreId = storeId,  // ストアID
                    ItemId = itemId,    // アイテムID
                    VirtualCurrency = vm    // 仮想通貨
                };
            }
            else
            {
                request = new PurchaseItemRequest
                {
                    Price = price,  // 価格
                    CatalogVersion = catalogVersion,    // カタログバージョン
                    StoreId = storeId,  // ストアID
                    ItemId = itemId,    // アイテムID
                    CharacterId = characterId,  // キャラクターID
                    VirtualCurrency = vm    // 仮想通貨
                };
            }
            
            var response = await PlayFabClientAPI.PurchaseItemAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                LogSuccess("アイテム[" + itemId + "]を購入しました");
                await GetUserInventoryAsync();
                successCallback?.Invoke(response.Result);
            }
        }

        // インベントリの更新
        public static async Task GetUserInventoryAsync(
            Action<GetUserInventoryResult> successCallback = null, Action<PlayFabError> errorCallback = null
        )
        {
            await Task.Delay(5000);
            var request = new GetUserInventoryRequest{ };
            var response = await PlayFabClientAPI.GetUserInventoryAsync(request);
            if (CheckResponse(response.Error, errorCallback))
            {
                GameManager.Instance.dataManager.Data.User.UserInventory = response.Result.Inventory;
                await GameManager.Instance.dataManager.Save();
                LogSuccess("インベントリを更新しました");
                successCallback?.Invoke(response.Result);
            }
        }
    }
}


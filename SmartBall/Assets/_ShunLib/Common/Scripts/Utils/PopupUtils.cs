using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShunLib.Popup;

namespace ShunLib.Utils.Popup
{
    public class PopupUtils : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        
        // モーダルウィンドウサイズ
        private static Vector2 WINDOW_SCALE = new Vector2(1920, 1080);
        // モーダル名
        private static string MODAL_NAME = "ModalButton";
        // モーダルカラー
        private static Color MODAL_COLOR = new Color32 (0, 0, 0, 100);
        // 表示済みpopup名のリスト
        private static List<string> _openPopupNameList = new List<string>();

        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // ポップアップを開く
        public static BasePopup OpenPopup(
            GameObject parentObj,
            GameObject popupPrefab,
            Dictionary<string, Action> actions = null,
            bool destroyFlag = true
            )
        {
            // 既に開いているポップアップは開かない
            if (CheckPopupName(popupPrefab)) { return null; }

            // Popupインスタンス生成
            GameObject obj = Instantiate(popupPrefab, parentObj.transform.position, Quaternion.identity);
            BasePopup popup = obj.GetComponent<BasePopup>();
            obj.name = obj.name.Replace( "(Clone)", "" );

            // ポップアップが既に開いているなら開かない
            if (popup.IsOpen) { return null; }

            // Popupをキャンバスの下に配置
            obj.transform.SetParent(parentObj.transform);

            // Popup表示
            popup.InitPopup(actions);
            popup.SetDestroyFlag(destroyFlag);
            // popup.Open();
            return popup;
        }

        // Popup名をリストに追加
        public static void AddPopupName(GameObject obj)
        {
            _openPopupNameList.Add(obj.name);
        }

        // Popup名をリストから削除
        public static void RemovePopupName(GameObject obj)
        {
            _openPopupNameList.Remove(obj.name);
        }

        // 表示済みPopupに存在するか
        public static bool CheckPopupName(GameObject obj)
        {
            return _openPopupNameList.Contains(obj.name);
        }

        // モーダル生成
        public static GameObject CreateModal(GameObject parentObj)
        {
            // インスタンス生成
            GameObject modal = new GameObject(MODAL_NAME);
            modal.transform.localPosition = Vector3.zero;
            modal.transform.localScale = Vector3.one;
            modal.transform.SetParent(parentObj.transform);

            // コンポーネント追加
            // Button
            modal.AddComponent<Button>();
            
            // RectTransform
            RectTransform rectTrans;
            if (modal.GetComponent<RectTransform>())
            {
                rectTrans = modal.GetComponent<RectTransform>();
            }
            else
            {
                modal.AddComponent<RectTransform>();
                rectTrans = modal.GetComponent<RectTransform>();
            }
            rectTrans.localPosition = Vector3.zero;
            rectTrans.localScale = Vector3.one;
            rectTrans.sizeDelta = WINDOW_SCALE;

            // Image
            modal.AddComponent<Image>();
            Image image = modal.GetComponent<Image>();
            image.color = MODAL_COLOR;
            return modal;
        }

        // 表示済みPopupリストの初期化
        public static void InitOpenPopupList()
        {
            _openPopupNameList = new List<string>();
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}
using UnityEngine;

namespace ShunLib.UI.PageIcon
{
    public class Indicator : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("活性時アイコンPrefab")] protected GameObject _activeIconObj = default;
        [SerializeField, Tooltip("非活性時アイコンPrefab")] protected GameObject _nonActiveIconObj = default;
        [SerializeField, Tooltip("親オブジェクト")] protected GameObject _parentObj = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private int _pageCount = default;
        private int _curPageNum = default;
        private GameObject _activeIcon = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize(int pageCount, int curPageNum = 0)
        {
            _pageCount = pageCount;
            _curPageNum = curPageNum;

            for (int i = 0; i < pageCount; i++)
            {
                if (i == curPageNum)
                {
                    _activeIcon = CreateIcon(_activeIconObj);
                }
                else
                {
                    CreateIcon(_nonActiveIconObj);
                }
            }
        }

        // ページ変更
        public void ChangePage(int pageNum)
        {
            if (_activeIcon == default) return;
            _activeIcon.transform.SetSiblingIndex(pageNum); 
        }

        // ---------- Private関数 ----------

        // アイコンを生成
        private GameObject CreateIcon(GameObject obj)
        {
            GameObject icon = Instantiate(obj, Vector3.zero, Quaternion.identity);
            icon.transform.SetParent(_parentObj.transform);
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            return icon;
        }

        // アイコンをすべて削除
        private void RemoveAllIcon()
        {
            foreach (Transform n in _parentObj.transform)
            {
                Destroy(n.gameObject);
            }
        }

        // ---------- protected関数 ---------
    }
}



using UnityEngine;

namespace ShunLib.Utils.Find
{
    public class FindUtils
    {
        // 子要素を全て取得
        public static GameObject[] GetChildObjects(GameObject parent)
        {
            GameObject[] children = new GameObject[parent.transform.childCount];
            int childIndex = 0;
            foreach (Transform child in parent.transform)
            {
                children[childIndex++] = child.gameObject;
            }
            return children;
        }

        // オブジェクト群から指定コンポーネントを持つオブジェクトを返す
        public static T GetFirstComponent<T>(GameObject[] gameObjects)
            where T : Component
        {
            T target = null;
            foreach (GameObject go in gameObjects)
            {
                target = go.GetComponent<T>();
                if (target != null) break;
            }
            return target;
        }
    }
}


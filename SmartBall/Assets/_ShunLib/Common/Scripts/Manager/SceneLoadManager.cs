using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using ShunLib.Singleton;
using ShunLib.Utils.Find;

namespace ShunLib.Manager.Scene
{
    public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------
        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 指定シーンへ遷移する（同期）
        public void TransitionScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(sceneName, mode);
        }

        // 指定シーンへ遷移する（非同期）
        public void TransitionSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(TransitionSceneCoroutine(sceneName, mode));
        }

        // 指定シーンへ遷移する（非同期）
        public IEnumerator TransitionSceneCoroutine(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, mode);
        }

        // 指定シーンに遷移しコンポーネントを取得
        public IEnumerator LoadSceneCoroutine<T>(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
            where T : Component
        {
            yield return SceneManager.LoadSceneAsync(sceneName, mode);

            UnityEngine.SceneManagement.Scene scene = SceneManager.GetSceneByName(sceneName);

            yield return FindUtils.GetFirstComponent<T>(scene.GetRootGameObjects());
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}


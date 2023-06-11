#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

using ShunLib.Manager.Initialize;
using ShunLib.Manager.Game;
using ShunLib.Utils.Editor;

// シーン生成用スクリプタブルオブジェクト
public class CreateSceneTemplate : ScriptableObject
{
    [Header("初期化マネージャー")] public InitializeManager initializeManager = default;
    [Header("ゲームマネージャー")] public GameManager gameManager = default;
    [Header("その他オブジェクト")] public List<GameObject> instanceObjectList = default;

    [MenuItem ("Assets/Create/ScriptableObject/CreateSceneTemplate")]
    static void CreateSceneTemplateScriptable()
    {
        CreateSceneTemplate createSceneTemplate = CreateInstance<CreateSceneTemplate> ();
        string path = AssetDatabase.GenerateUniqueAssetPath(EditorUtils.GetPath() + "/" + "New CreateSceneTemplate.asset");
        AssetDatabase.CreateAsset(createSceneTemplate, path);
        AssetDatabase.Refresh();
    }
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

using ShunLib.Manager.Initialize;
using ShunLib.Manager.Game;
using ShunLib.Utils.Editor;

public class CreateScene : ScriptableObject
{
    public CreateSceneTemplate createSceneTemplate = default;
}

public class CreateSceneEditor : EditorWindow
{
    private const string SCENE_NAME = "InitializeScene";

    private const string SAVE_PATH = "Assets/Scenes";

    private static SerializedProperty _spInitializeManager = default;
    private static SerializedProperty _spGameManager = default;
    private static SerializedProperty _spInstanceObjectList = default;
    private static SerializedProperty _createSceneTemplate = default;

    [MenuItem("Tools/CreateScene/CreateInitializeScene")]
    private static void Initialize()
    {
        ShowWindow();
    }

    // ウィンドウ表示
    public static void ShowWindow()
    {
        EditorWindow a = EditorWindow.GetWindow(typeof(CreateSceneEditor), true, "初期化シーン生成");
        var createScene = ScriptableObject.CreateInstance<CreateScene>();
        var serializedObject = new UnityEditor.SerializedObject(createScene);

        _createSceneTemplate = serializedObject.FindProperty("createSceneTemplate");
    }

    // UI初期化
    private void OnGUI()
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("生成したいシーンのテンプレートを設定");
        EditorGUILayout.PropertyField(_createSceneTemplate);
        GUILayout.Space(10);

        if (GUILayout.Button("生成開始"))
        {
            if (_createSceneTemplate == default)
            {
                return;
            }
            else
            {
                // 初期化シーンの生成
                EditorUtils.CreateScene(SCENE_NAME, SAVE_PATH, EditorApplication.NewEmptyScene);

                CreateSceneTemplate createSceneTemplate = _createSceneTemplate.objectReferenceValue as CreateSceneTemplate;

                // 初期化マネージャーの生成＆初期化
                InitializeManager initializeManager = Instantiate(
                    createSceneTemplate.initializeManager,
                    Vector3.zero,
                    Quaternion.identity
                );
                initializeManager.gameObject.name = initializeManager.gameObject.name.Replace( "(Clone)", "" );

                // ゲームマネージャーの生成＆初期化
                GameManager gameManager = Instantiate(
                    createSceneTemplate.gameManager,
                    Vector3.zero,
                    Quaternion.identity
                );
                gameManager.gameObject.name = gameManager.gameObject.name.Replace( "(Clone)", "" );
                initializeManager.SetManager(
                    gameManager.masterManager,
                    gameManager.dataManager,
                    gameManager.audioManager,
                    gameManager
                );

                // その他のオブジェクトの生成
                for (int i = 0; i < createSceneTemplate.instanceObjectList.Count; i++)
                {
                    GameObject obj = Instantiate(
                        createSceneTemplate.instanceObjectList[i],
                        Vector3.zero,
                        Quaternion.identity
                    );
                    obj.name = obj.name.Replace( "(Clone)", "" );
                }

                EditorSceneManager.MarkAllScenesDirty();
            }
        }
    }
}
#endif
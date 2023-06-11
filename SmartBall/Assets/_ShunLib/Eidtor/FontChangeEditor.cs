#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class MyFont : ScriptableObject
{
    public List<Font> fontList = new List<Font>();
    public List<TMP_FontAsset> fontProList = new List<TMP_FontAsset>();
    public Font changeFont;
    public TMP_FontAsset changeFontPro;
}

public class FontChangeEditor : EditorWindow 
{
    static SerializedProperty spFontList;
    static SerializedProperty spFontProList;
    static SerializedProperty spChangeFont;
    static SerializedProperty spChangeFontPro;
    public static List<UnityEngine.Font> fontList = default;
    public static List<TMPro.TMP_FontAsset> fontProList = default;
    public static List<Text> differTextList = default;
    public static List<TextMeshProUGUI> differTextProList = default;

    [MenuItem("Tools/FontChangeEditor")]
    static void Init()
    {
        ShowWindow();
    }

    public static void ShowWindow()
    {
        EditorWindow a = EditorWindow.GetWindow(typeof(FontChangeEditor), true, "FontChangeEditor");
        var myFont = ScriptableObject.CreateInstance<MyFont>();
        var serializedObject = new UnityEditor.SerializedObject(myFont);

        spFontList = serializedObject.FindProperty("fontList");
        spFontProList = serializedObject.FindProperty("fontProList");
        spChangeFont = serializedObject.FindProperty("changeFont");
        spChangeFontPro = serializedObject.FindProperty("changeFontPro");

        // object obj = sp.objectReferenceValue as object;
        // fontList = obj as List<UnityEngine.Font>;
    }

    void OnGUI()
    {
        EditorGUILayout.PropertyField(spFontList);
        EditorGUILayout.PropertyField(spFontProList);
        EditorGUILayout.PropertyField(spChangeFont);
        EditorGUILayout.PropertyField(spChangeFontPro);
        if (GUILayout.Button("Search All Fonts(Scene)"))
        {
            fontList = new List<UnityEngine.Font>();
            fontProList = new List<TMPro.TMP_FontAsset>();

            differTextList = new List<Text>();
            differTextProList = new List<TextMeshProUGUI>();

            for (int i = 0; i < spFontList.arraySize; i++)
            {
                Font font = spFontList.GetArrayElementAtIndex(i).objectReferenceValue as Font;
                fontList.Add(font);
            }
            for (int i = 0; i < spFontProList.arraySize; i++)
            {
                TMP_FontAsset font = spFontProList.GetArrayElementAtIndex(i).objectReferenceValue as TMP_FontAsset;
                fontProList.Add(font);
            }
            
            Text[] textComponents = Resources.FindObjectsOfTypeAll(typeof(Text)) as Text[];
            for (int i = 0; i < textComponents.Length; i++)
            {
                FontChecker(textComponents[i]);
            }
            TextMeshProUGUI[] textProComponents = Resources.FindObjectsOfTypeAll(typeof(TextMeshProUGUI)) as TextMeshProUGUI[];
            for (int i = 0; i < textProComponents.Length; i++)
            {
                FontChecker(textProComponents[i]);
            }
        }
        if (GUILayout.Button("Search All Fonts(PrefabMode)"))
        {
            fontList = new List<UnityEngine.Font>();
            fontProList = new List<TMPro.TMP_FontAsset>();

            differTextList = new List<Text>();
            differTextProList = new List<TextMeshProUGUI>();

            for (int i = 0; i < spFontList.arraySize; i++)
            {
                Font font = spFontList.GetArrayElementAtIndex(i).objectReferenceValue as Font;
                fontList.Add(font);
            }
            for (int i = 0; i < spFontProList.arraySize; i++)
            {
                TMP_FontAsset font = spFontProList.GetArrayElementAtIndex(i).objectReferenceValue as TMP_FontAsset;
                fontProList.Add(font);
            }
            var stage = PrefabStageUtility.GetCurrentPrefabStage();
            GameObject rootObject = stage.prefabContentsRoot;
            GetChildTransform(rootObject.transform);
            stage.ClearDirtiness();
        }
        if (GUILayout.Button("Change All Fonts"))
        {
            foreach (Text text in differTextList)
            {
                text.font = spChangeFont.objectReferenceValue as Font;
            }
            foreach (TextMeshProUGUI text in differTextProList)
            {
                text.font = spChangeFontPro.objectReferenceValue as TMP_FontAsset;
            }
            // ※追記 : シーンに変更があることをUnity側に通知しないと、シーンを切り替えたときに変更が破棄されてしまうので、↓が必要
            EditorSceneManager.MarkAllScenesDirty();
        }
    }

    public void FontChecker(Text text)
    {
        UnityEngine.Font font = text.font;
        if (!fontList.Contains(font))
        {
            string path = GetFullPath(text.gameObject.transform);
            Debug.Log("<color=red>Fontが違う！</color>:" + path);
            differTextList.Add(text);
        }
    }

    public void FontChecker(TextMeshProUGUI text)
    {
        TMPro.TMP_FontAsset font = text.font;
        if (!fontProList.Contains(font))
        {
            string path = GetFullPath(text.gameObject.transform);
            Debug.Log("<color=red>Fontが違う！</color>:" + path);
            differTextProList.Add(text);
        }
    }

    public void GetChildTransform(Transform trans)
    {
        Transform[] children = new Transform[trans.childCount];
        for (var i = 0; i < children.Length; ++i)
        {
            children[i] = trans.GetChild(i);
            Text text = children[i].gameObject.GetComponent<Text>();
            if (text != null)
            {
                FontChecker(text);
            }
            TextMeshProUGUI textPro = children[i].gameObject.GetComponent<TextMeshProUGUI>();
            if (textPro != null)
            {
                FontChecker(textPro);
            }
            GetChildTransform(children[i]);
        }
    }

    public static string GetFullPath(Transform t)
	{
		string path = t.name;
		var parent = t.parent;
		while (parent)
		{
			path = $"{parent.name}/{path}";
			parent = parent.parent;
		}
		return path;
	}
}
#endif
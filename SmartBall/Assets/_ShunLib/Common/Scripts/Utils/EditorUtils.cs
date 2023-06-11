#if UNITY_EDITOR
using System;
using System.IO;

using UnityEditor;

namespace ShunLib.Utils.Editor
{
    public class EditorUtils
    {
        // 現在のパスを取得
        public static string GetPath()
        {
            var instanceId = Selection.activeInstanceID;
            var path = AssetDatabase.GetAssetPath( instanceId );
            path = string.IsNullOrEmpty( path ) ? "Assets" : path;

            if ( Directory.Exists( path ) )
            {
                return path;
            }
            if ( System.IO.File.Exists( path ) )
            {
                var parent = Directory.GetParent( path );
                var fullName = parent.FullName;
                var unixFileName = fullName.Replace( "\\", "/" );
                return FileUtil.GetProjectRelativePath( unixFileName );
            }
            return string.Empty;
        }

        // 新規シーンの作成
        public static void CreateScene( 
            string filenameWithoutExtension,
            string saveFilePath,
            Action newSceneCallback 
        )
        {
            var filename = filenameWithoutExtension + ".unity";
            var path = saveFilePath + "/" + filename;
            path = AssetDatabase.GenerateUniqueAssetPath( path );

            newSceneCallback();
            
            EditorApplication.SaveScene( path );

            var scenes = EditorBuildSettings.scenes;
            ArrayUtility.Add( 
                ref scenes, 
                new EditorBuildSettingsScene( path, true )
            );
            EditorBuildSettings.scenes = scenes;
        }
    }
}
#endif
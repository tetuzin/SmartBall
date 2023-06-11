#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ShunLib.Btn.Common;

namespace ShunLib.Editor.Btn.Common
{
    [CustomEditor(typeof(CommonButton))]
    public class CommonButtonEditor : UnityEditor.UI.ButtonEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var component = (CommonButton) target;
            PropertyField(nameof(component.obj), "ButtonObject");
            PropertyField(nameof(component.eventTrigger), "EventTrigger");
            PropertyField(nameof(component.text), "Text");
            PropertyField(nameof(component.downWaitTime), "DownWaitTime");
            PropertyField(nameof(component.isPlaySE), "IsPlaySE");
            PropertyField(nameof(component.isHoverAnim), "IsHoverAnim");
            serializedObject.ApplyModifiedProperties();
        }

        private void PropertyField(string property, string label) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(property), new GUIContent(label));
        }
    }
}
#endif
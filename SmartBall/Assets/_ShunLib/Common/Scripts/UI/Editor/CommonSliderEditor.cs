#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

using ShunLib.UI.Slider;

namespace ShunLib.Editor.UI.Slider
{
    [CustomEditor(typeof(CommonSlider))]
    public class CommonSliderEditor : UnityEditor.UI.SliderEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var component = (CommonSlider) target;
            PropertyField(nameof(component.textGroup), "TextGroup");
            PropertyField(nameof(component.curText), "CurText");
            PropertyField(nameof(component.maxText), "MaxText");
            serializedObject.ApplyModifiedProperties();
        }

        private void PropertyField(string property, string label) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(property), new GUIContent(label));
        }
    }
}
#endif
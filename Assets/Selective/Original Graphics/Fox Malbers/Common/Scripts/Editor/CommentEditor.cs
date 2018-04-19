using UnityEngine;
using System.Collections;
using UnityEditor;

namespace MalbersAnimations.Utilities
{

    [CustomEditor(typeof(Comment))]
    public class CommentEditor : Editor
    {
        private Comment script { get { return target as Comment; } }
        GUIStyle style;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.Space();
            style = EditorStyles.helpBox;

            string text = EditorGUILayout.TextArea(script.text, style);
            if (text != script.text)
            {
                Undo.RecordObject(script, "Edit Comments");
                script.text = text;
            }
           
            serializedObject.ApplyModifiedProperties();
        }
    }
}

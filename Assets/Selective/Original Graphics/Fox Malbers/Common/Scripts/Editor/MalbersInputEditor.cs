using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace MalbersAnimations
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MalbersInput))]
    public class MalbersInputEditor : Editor
    {
        private ReorderableList list;
        private MalbersInput MInput;
        MonoScript script;
        private void OnEnable()
        {
            MInput = ((MalbersInput)target);
            script = MonoScript.FromMonoBehaviour(MInput);
            list = new ReorderableList(serializedObject, serializedObject.FindProperty("inputs"), true, true, true, true);
            list.drawElementCallback = drawElementCallback;
            list.drawHeaderCallback = HeaderCallbackDelegate;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Connects the INPUTS to the Locomotion System. The 'Name' is actually the Properties to access", MessageType.None);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            EditorGUI.BeginDisabledGroup(true);
            script = (MonoScript)EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            MInput.ActiveHorizontal = EditorGUILayout.Toggle("", MInput.ActiveHorizontal, GUILayout.MaxWidth(15));
            MInput.Horizontal = EditorGUILayout.TextField( "Horizontal Axis", MInput.Horizontal);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            MInput.ActiveVertical = EditorGUILayout.Toggle("", MInput.ActiveVertical, GUILayout.MaxWidth(15));
            MInput.Vertical = EditorGUILayout.TextField( "Vertical Axis",MInput.Vertical);
            EditorGUILayout.EndHorizontal();
            MInput.AxisRaw = EditorGUILayout.ToggleLeft(new GUIContent("Axis Raw","Uses Raw values from get input"), MInput.AxisRaw);
            EditorGUILayout.EndVertical();

            list.DoLayoutList();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            MInput.cameraBaseInput = EditorGUILayout.Toggle(new GUIContent("Camera Input", "The Character will follow the camera forward axis"), MInput.cameraBaseInput );
            MInput.alwaysForward = EditorGUILayout.Toggle(new GUIContent("Always Forward", "The Character will move forward forever"), MInput.alwaysForward);
            EditorGUILayout.EndVertical();
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Reordable List Header
        /// </summary>
        void HeaderCallbackDelegate(Rect rect)
        {
            Rect R_1 = new Rect(rect.x + 20, rect.y, (rect.width - 20) / 4 - 23, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(R_1, "Name");

            Rect R_2 = new Rect(rect.x + (rect.width - 20) / 4 + 15, rect.y, (rect.width - 20) / 4, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(R_2, "Type");

            Rect R_3 = new Rect(rect.x + ((rect.width - 20) / 4) * 2 + 18, rect.y, ((rect.width - 30) / 4) + 11, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(R_3, "Value");

            Rect R_4 = new Rect(rect.x + ((rect.width) / 4) * 3 + 15, rect.y, ((rect.width) / 4) - 15, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(R_4, "Button");
        }

        void drawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = MInput.inputs[index];
            rect.y += 2;
            element.active = EditorGUI.Toggle(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), element.active);

            Rect R_1 = new Rect(rect.x + 20, rect.y, (rect.width - 20) / 4 - 23, EditorGUIUtility.singleLineHeight);
            GUIStyle a = new GUIStyle();

            //This make the name a editable label
            a.fontStyle = FontStyle.Normal;
            element.name = EditorGUI.TextField(R_1, element.name, a);

            Rect R_2 = new Rect(rect.x + (rect.width - 20) / 4+15, rect.y, (rect.width - 20) / 4, EditorGUIUtility.singleLineHeight);
            element.type = (InputType)EditorGUI.EnumPopup(R_2, element.type);

            Rect R_3 = new Rect(rect.x + ((rect.width - 20) / 4) * 2 + 18, rect.y, ((rect.width - 30) / 4)+11 , EditorGUIUtility.singleLineHeight);
            if (element.type != InputType.Key)
                element.input = EditorGUI.TextField(R_3, element.input);
            else
                element.key = (KeyCode)EditorGUI.EnumPopup(R_3, element.key);

            Rect R_4 = new Rect(rect.x + ((rect.width) / 4) * 3 +15, rect.y, ((rect.width) / 4)-15 , EditorGUIUtility.singleLineHeight);
            element.GetPressed = (InputButton)EditorGUI.EnumPopup(R_4, element.GetPressed);
        }
    }
}
using UnityEngine;
using UnityEditor;

namespace MalbersAnimations
{
    [CustomPropertyDrawer(typeof(InputRow))]
    public class InputRowDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
           // EditorGUI.HelpBox(position, "",MessageType.None);

            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            property.FindPropertyRelative("name").stringValue = label.text;

            // Calculate rects
            var activeRect = new Rect(position.x, position.y, 15,position.height);
            var LabelRect = new Rect(position.x + 17, position.y,100, position.height);
           


            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(activeRect, property.FindPropertyRelative("active"), GUIContent.none);
            EditorGUI.LabelField(LabelRect, label, EditorStyles.miniBoldLabel);

            //Set Rect to the Parameters Values
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(" "));
           
            // Calculate rects
            var typeRect = new Rect(position.x-30, position.y, 44, position.height);
            var valueRect = new Rect(position.x-30 +45, position.y, position.width/2-7, position.height);
            var ActionRect = new Rect(position.width / 2 + position.x-30 +40 -1, position.y, position.width / 2 - 7, position.height);

            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), GUIContent.none);

            InputType current = (InputType)property.FindPropertyRelative("type").enumValueIndex;
            switch (current)
            {
                case InputType.Input:
                   EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("input"), GUIContent.none);
                    break;
                case InputType.Key:
                    EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("key"), GUIContent.none);
                    break;
                default:
                    break;
            }

            EditorGUI.PropertyField(ActionRect, property.FindPropertyRelative("GetPressed"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

    }
}
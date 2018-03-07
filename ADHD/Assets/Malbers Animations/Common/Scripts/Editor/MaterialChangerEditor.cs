using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace MalbersAnimations.Utilities
{
    [CustomEditor(typeof(MaterialChanger))]
    public class MaterialChangerEditor : Editor
    {
        private ReorderableList list;
        private SerializedProperty materialList;
        private MaterialChanger MyMaterialChanger;
        private MonoScript script;

        private void OnEnable()
        {
            MyMaterialChanger = ((MaterialChanger)target);
            script = MonoScript.FromMonoBehaviour(MyMaterialChanger);

            materialList = serializedObject.FindProperty("materialList");

            list = new ReorderableList(serializedObject, materialList, true, true, true, true);
            list.drawElementCallback = DrawElementCallback;
            list.drawHeaderCallback = HeaderCallbackDelegate;
            list.onAddCallback = OnAddCallBack;
        }

        void HeaderCallbackDelegate(Rect rect)
        {
            Rect R_0 = new Rect(rect.x, rect.y, 15, EditorGUIUtility.singleLineHeight);
            Rect R_01 = new Rect(rect.x+14, rect.y, 35, EditorGUIUtility.singleLineHeight);
            Rect R_1 = new Rect(rect.x + 14 + 25, rect.y, (rect.width - 10) / 2, EditorGUIUtility.singleLineHeight);
            Rect R_2 = new Rect(rect.x + 35 +((rect.width - 30) / 2), rect.y, rect.width - ((rect.width) / 2) - 25, EditorGUIUtility.singleLineHeight);

            MyMaterialChanger.showMeshesList = EditorGUI.ToggleLeft(R_0, new GUIContent("", "Show the Material Items when Selected"), MyMaterialChanger.showMeshesList);

            EditorGUI.LabelField(R_01,new GUIContent (" #","Index"), EditorStyles.miniLabel);
            EditorGUI.LabelField(R_1, "Materials Items", EditorStyles.miniLabel);
            EditorGUI.LabelField(R_2, "CURRENT", EditorStyles.centeredGreyMiniLabel);
        }

        void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = MyMaterialChanger.materialList[index];
            rect.y += 2;

            Rect R_0 = new Rect(rect.x, rect.y, (rect.width - 65) / 2, EditorGUIUtility.singleLineHeight);
            Rect R_1 = new Rect(rect.x + 25, rect.y, (rect.width - 65) / 2 , EditorGUIUtility.singleLineHeight);
            Rect R_2 = new Rect(rect.x + 25 + ((rect.width - 30) / 2), rect.y, rect.width - ((rect.width) / 2) - 8, EditorGUIUtility.singleLineHeight);

             EditorGUI.LabelField(R_0, "("+index.ToString()+")", EditorStyles.label);
            element.Name = EditorGUI.TextField(R_1, element.Name, EditorStyles.label);
            string buttonCap = "None";

            if (element.mesh != null)
            {
                EditorGUI.BeginDisabledGroup(!element.mesh.gameObject.activeSelf || element.materials.Length ==0);

                    if (element.materials.Length > element.current)
                    {
                        buttonCap = element.mesh.gameObject.activeSelf ? (element.materials[element.current] == null ? "None" : element.materials[element.current].name) + " ("+ element.current + ")" : "Is Hidden";
                    }

                    if (GUI.Button(R_2, buttonCap, EditorStyles.miniButton))
                    {
                        ToggleButton(index);
                    }
                EditorGUI.EndDisabledGroup();
            }
        }

        void ToggleButton(int index)
        {
            if (MyMaterialChanger.materialList[index].mesh != null)
            {
                MyMaterialChanger.materialList[index].ChangeMaterial();
            }
        }



        void OnAddCallBack(ReorderableList list)
        {
            if (MyMaterialChanger.materialList == null)
            {
                MyMaterialChanger.materialList = new System.Collections.Generic.List<MaterialItem>();
            }
            MyMaterialChanger.materialList.Add(new MaterialItem());
        }


     

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginVertical(MalbersEditor.StyleBlue);
            EditorGUILayout.HelpBox("Swap Materials", MessageType.None);
            EditorGUILayout.EndVertical();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginVertical(MalbersEditor.StyleGray);
            {
                EditorGUI.BeginDisabledGroup(true);
                script = (MonoScript)EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false);
                EditorGUI.EndDisabledGroup();
                list.DoLayoutList();
                EditorGUI.indentLevel++;

                if (MyMaterialChanger.showMeshesList)
                {
                    if (list.index != -1)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        SerializedProperty Element = materialList.GetArrayElementAtIndex(list.index);
                        EditorGUILayout.LabelField(MyMaterialChanger.materialList[list.index].Name, EditorStyles.boldLabel);

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(Element.FindPropertyRelative("mesh"), new GUIContent("Mesh","Mesh object to apply the Materials"));
                        EditorGUILayout.PropertyField(Element.FindPropertyRelative("indexM"), new GUIContent("ID","Material ID"));
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        EditorGUILayout.PropertyField(Element.FindPropertyRelative("materials"), new GUIContent("Materials"),true);
                        EditorGUILayout.EndVertical();

                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        SerializedProperty hasLODS = Element.FindPropertyRelative("HasLODs");
                        EditorGUILayout.PropertyField(hasLODS, new GUIContent("LODs", "Has Level of Detail Meshes"));
                        if (hasLODS.boolValue)
                        {
                            EditorGUILayout.PropertyField(Element.FindPropertyRelative("LODs"), new GUIContent("Meshes", "Has Level of Detail Meshes"),true);
                        }
                        EditorGUILayout.EndVertical();
                        //EditorGUILayout.PropertyField(Element, true);
                        EditorGUILayout.EndVertical();
                    }
                }


                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();


            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(MyMaterialChanger, "Material Changer");
                EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
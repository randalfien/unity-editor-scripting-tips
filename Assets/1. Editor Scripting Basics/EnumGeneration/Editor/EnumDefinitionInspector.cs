using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/*
 * This custom inspector will make an array of strings, from which
 * it will automatically generate an Enum
 */

[CustomEditor(typeof(EnumDefinition))]
public class EnumDefinitionInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EnumDefinition enumDef = (EnumDefinition) target;
        EditorGUI.BeginChangeCheck();
        
        SerializedProperty property = serializedObject.FindProperty("Values");
        EditorGUILayout.PropertyField(property, new GUIContent("Values"), true);
        
        ValidateInput(enumDef.Values);
        
        if (EditorGUI.EndChangeCheck()) // a value has been changed
        {
            serializedObject.ApplyModifiedProperties();
            var props = serializedObject.FindProperty("Values"); // using myTarget.Languages will return old version of data
            var list = new List<string>();
            for (int i = 0; i < props.arraySize; i++)
            {
                list.Add(props.GetArrayElementAtIndex(i).stringValue);
            }

            var newValue = list.ToArray();
            var isValid = ValidateInput(newValue); // important to validate, otherwise the generated Enum will not compile
            if (!isValid) return;
            
            string assetPath = "Assets/MyEnum.cs";
            var scripts = AssetDatabase.FindAssets("MyEnum t:Script"); // check if there already is MyEnum defined
            if (scripts.Length == 1)
            {
                var scriptsFileGuid = scripts[0];
                assetPath = AssetDatabase.GUIDToAssetPath(scriptsFileGuid);
            }

            File.WriteAllText(assetPath, GetEnumString(newValue));
            
            AssetDatabase.Refresh();
            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            EditorUtility.SetDirty(textAsset);
        }
    }
    private bool ValidateInput(string[] values)
    {
        if (values.Length < 1)
        {
            EditorGUILayout.HelpBox("Please add at least 1 value", MessageType.Error);
            return false;
        }
        
        foreach (var l in values)
        {
            if (l.Length == 0)
            {
                EditorGUILayout.HelpBox("Enums cannot be empty", MessageType.Error);
                return false;
            }
            if (!l.All(char.IsLetter))
            {
                EditorGUILayout.HelpBox("Enums must contain only letters", MessageType.Error);
                return false;
            }

        }
        if (values.Length != values.Distinct().Count())
        {
            EditorGUILayout.HelpBox("Enums must be unique", MessageType.Error);
            return false;
        }
        return true;
    }

    private string GetEnumString(string[] vals)
    {
        var start = "//Automatically Generated\n" +                    
                    "public enum MyEnum{\n";
        var content = "";
        for (int i = 0; i < vals.Length; i++)
        {
            content += "\t"+vals[i] + " = " + i + (i < vals.Length-1 ? "," : "") + "\n";
        }
        return start + content + "}";
    }
}

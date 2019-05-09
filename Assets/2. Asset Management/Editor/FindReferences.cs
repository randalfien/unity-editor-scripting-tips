using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FindReferences
{
    [MenuItem("Assets/Find References in Project",false, 28)] // With Assets/ here this will make it available on right click in Project window
    private static void FindReferencesToAsset()
    {
        var selected = Selection.activeObject;
        if (selected)
        {
            EditorUtility.DisplayProgressBar("Finding references in scenes", "", 0f);

            var searchedObjectPath = AssetDatabase.GetAssetPath(selected.GetInstanceID());
            
            Debug.Log("Asset ("+searchedObjectPath+") is referenced in:");
            var allScenes = AssetDatabase.FindAssets("t:scene");
            var allScenesPaths = allScenes.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            
            for (var i = 0; i < allScenesPaths.Length; i++)
            {
                var scenePath = allScenesPaths[i];
                var dep = AssetDatabase.GetDependencies(scenePath, false); // false is important here, otherwise it includes all assets reference in all scenes accessible from this one
                if(dep.Contains(searchedObjectPath))
                {
                    Debug.Log(scenePath);
                }
                EditorUtility.DisplayProgressBar("Finding references in scenes", "Searching", (float) i/allScenesPaths.Length );
            }
            
            EditorUtility.ClearProgressBar();
        }
    }
}

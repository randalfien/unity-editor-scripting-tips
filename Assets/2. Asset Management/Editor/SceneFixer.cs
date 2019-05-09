using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/*
 * This script shows how to go through all scenes in the project and
 * automatically detect and fix errors
 */

public class SceneFixer : MonoBehaviour
{
    [MenuItem("MyTools/Fix Scene References")]
    private static void FixSceneRefs()
    {
        EditorUtility.DisplayProgressBar("Fixing scene references", "", 0f);

        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");
        
        for (var i = 0; i < sceneGuids.Length; i++)
        {
            
            var scene = EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(sceneGuids[i]));
            GameObject[] rootObjects = scene.GetRootGameObjects();
            
            var change = false;
            foreach (GameObject rootObject in rootObjects)
            {
                var sceneRefs = rootObject.GetComponentsInChildren<TestBehaviour>(); // Find scripts to fix
                
                foreach (var testBehaviour in sceneRefs)
                {
                    if (testBehaviour.Variable1 % 2 == 1) // Detect error with the script
                    {
                        testBehaviour.Variable1 += 1; // Fix it
                        change = true;
                    }
                }
            }
            
            if (change)
            {
                EditorSceneManager.SaveScene(scene);
            }

            EditorUtility.DisplayProgressBar("Fixing scene references", "", (float) i / sceneGuids.Length);
        }

        EditorUtility.ClearProgressBar();
    }
    
    
    /*
     This is a handy way to iterate over all scenes, call like this: foreach (Scene scene in GetSavedScenes()) { ... }
			
     private static IEnumerable<Scene> GetSavedScenes() {
			string[] guids = AssetDatabase.FindAssets("t:Scene");
			foreach (string guid in guids) {
				yield return EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(guid));
			}
	 }
     */
}

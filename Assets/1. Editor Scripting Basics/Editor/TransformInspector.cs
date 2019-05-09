using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if USE_CUSTOM_INSPECTORS
[CustomEditor(typeof(Transform))]
public class TransformInspector : Editor
{

    public override void OnInspectorGUI()
    {
        Transform myTarget = (Transform)target;


        myTarget.position = EditorGUILayout.Vector2Field("Position", myTarget.position);
        myTarget.rotation = Quaternion.Euler(0, 0, EditorGUILayout.Slider("Rotation", myTarget.localRotation.eulerAngles.z, 0, 366));

        if (GUILayout.Button("Round this please"))
        {
            myTarget.position = new Vector3( Mathf.Round(myTarget.position.x),
                    Mathf.Round(myTarget.position.y),
                    Mathf.Round(myTarget.position.z)
                );
        }
    }
    
}
#endif
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Networking;
#endif

[CreateAssetMenu(fileName="MySO",menuName = "MyScriptableObject")]
public class MyScriptableObjectScript : ScriptableObject
{
    public int[] Value;
}

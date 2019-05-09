// Initial Concept by http://www.reddit.com/user/zaikman
// Revised by http://www.reddit.com/user/quarkism
// Revised by Ondrej Paska 2019

using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;
using Object = UnityEngine.Object;

/// <summary>
/// Put [EditorButton] on a method
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR

[CustomEditor(typeof(MonoBehaviour), true)]
public class EditorButtonInspector : Editor {
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Instance | BindingFlags.Static |
                                 BindingFlags.Public |
                                 BindingFlags.NonPublic;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        MonoBehaviour mono = (MonoBehaviour) target;

        MemberInfo[] methods = mono.GetType().GetMembers(Flags);

        for ( int i = 0 ; i < methods.Length ; i++ ) {
            MemberInfo memberInfo = methods [i];
            if (Attribute.IsDefined(memberInfo, typeof(EditorButtonAttribute)) == false) continue;

            if ( GUILayout.Button( memberInfo.Name ) ) {
                Undo.RecordObject( mono, mono.name );
                ((MethodInfo)memberInfo).Invoke( mono, null );
                EditorUtility.SetDirty(mono);
            }
        }
        
    }
}
#endif
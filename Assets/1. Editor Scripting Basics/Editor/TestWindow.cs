using System.Linq;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    
    [MenuItem ("MyTools/My Window")]
    public static void  ShowWindow () {
        GetWindow(typeof(TestWindow));
    }
    
    private void OnGUI()
    {
        EditorGUILayout.HelpBox("Hello World", MessageType.Info);

        var transforms = Selection.transforms;
        var count = transforms.Length;
        
        if (GUILayout.Button("Align Selection Horizontal"))
        {
            if (count == 0) return;
            var max = transforms.Max(t => t.position.x);
            var min = transforms.Min(t => t.position.x);
            int i = 0;
            float pad = (max - min) / (count-1);
            foreach (var t in transforms)
            {
                t.position = new Vector3(min+i*pad, t.position.y, t.position.z);
                i++;
            }
        }
        
        if (GUILayout.Button("Align Selection Vertical"))
        {
            if (count == 0) return;
            var max = transforms.Max(t => t.position.y);
            var min = transforms.Min(t => t.position.y);
            int i = 0;
            float pad = (max - min) / (count-1);
            foreach (var t in transforms)
            {
                t.position = new Vector3(t.position.x, min+i*pad, t.position.z);
                i++;
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class TestMenuItem
{
    public const string Flag = "USE_CUSTOM_INSPECTORS";

#if !USE_CUSTOM_INSPECTORS    
    [MenuItem("MyTools/Custom Inspectors On")]
    public static void CustomInspectorsOn()
    {
        RefreshCompilationFlag(true);
    }
#else    
    [MenuItem("MyTools/Custom Inspectors Off")]
    public static void CustomInspectorsOff()
    {
        RefreshCompilationFlag(false);
    }
#endif
    
    /**
     * Edits the custom symbols defined in Project settings. Triggers a recompile.
     */
    private static void RefreshCompilationFlag(bool value)
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        
        List<string> allDefines = definesString.Split(';').ToList();
            
        if (allDefines.Contains(Flag) && !value)
        {
            allDefines.Remove(Flag);
        }
        else if (!allDefines.Contains(Flag) && value)
        {
            allDefines.Add(Flag);
        }
           
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }
}

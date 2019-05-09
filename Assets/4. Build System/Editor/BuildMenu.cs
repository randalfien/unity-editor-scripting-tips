using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BuildMenu
{
    [MenuItem("MyTools/Build it")]   
    static void BuildIt()
    {
        string[] levels = EditorBuildSettings.scenes.Select(a => a.path).ToArray();
        
        // Let user choose a folder location
        string path = EditorUtility.SaveFolderPanel("Choose Location", "", "");
        
        // This works on Windows only
        var exePath = path + "/TestApp.exe";
        var br = BuildPipeline.BuildPlayer(levels, exePath, BuildTarget.StandaloneWindows, BuildOptions.Development);
        Debug.Log( "Built " + br.summary.result );
        
        var proc = new Process();
        proc.StartInfo.FileName = exePath;
        proc.Start();
    }
    
    [PostProcessBuild(1)] // this means this method will be called just after the build is finished
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject) {
        
        // This method will create a new text file to the build folder, that will have the current time
        // and last git commit information
        
        var infoPath = Path.GetDirectoryName(pathToBuiltProject) + "/info.txt";
            
        // Time
        StringBuilder info = new StringBuilder();
        info.AppendLine("-- Time --");
        info.AppendLine(DateTime.Now.ToString("MM.dd. yyyy HH:mm"));
        info.AppendLine("-");
        
        // Last Commit
        ShellHelper.ShellRequest req = ShellHelper.ProcessCommand("git log -1", Application.dataPath);
        int numLines = 0;
        req.onLog += (logType, log) =>
        {
            if (numLines == 0)
            {
                info.AppendLine("-- Last Commit --");
            }
            info.AppendLine(log);
            numLines++;
        }; 
            
        int numDone = 0;
        req.onDone += () =>
        {
            numDone++;
            if (numDone == 2) File.WriteAllText(infoPath, info.ToString(), Encoding.UTF8);
        };

    }
}

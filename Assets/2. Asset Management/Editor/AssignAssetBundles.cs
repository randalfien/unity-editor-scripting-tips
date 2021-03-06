﻿using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;

/*
This script gives you the option to assign assets to asset bundles based on the file names.
Change the string constants below to fit your needs
*/
public class AssignAssetBundles
{
   // [MenuItem("Assets/Assign AssetBundle Names")]
    static void AssignAssetBundleNames()
    {
        const string assetFilter = "t:TextAsset";   // filter which assets should be searched (leave empty for all) 
        const string nameRegExp = @"Item\d\d";      // regexp for asset names, this one takes all files that contain Item followed by two digits

        var allTexts = AssetDatabase.FindAssets(assetFilter);
        foreach (var tex in allTexts)
        {
            var path = AssetDatabase.GUIDToAssetPath(tex);
            var name = Path.GetFileName(path);
            var m = Regex.Match(name, nameRegExp);
            if (m.Success)
            {				
                var assetSettings = AssetImporter.GetAtPath(path);
                assetSettings.assetBundleName = m.Groups[0].Value;
            }
        }
    }
}

using UnityEditor;
using UnityEngine;

public class SpriteImporter : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        TextureImporter textureImporter  = (TextureImporter)assetImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
        textureImporter.filterMode = FilterMode.Point;
        textureImporter.spritePixelsPerUnit = 10;

        if (assetPath.Contains("@2"))
        {
            textureImporter.spritePixelsPerUnit = 20;    
        }
        
        Debug.Log(assetPath);
    }
}

using UnityEngine;
using System.Collections;
using UnityEditor;

public class spritesnotporter : AssetPostprocessor
{ 
     void OnPostProcessTexture (Texture2D texture)
     {
         string lowerCaseAssetPath = assetPath.ToLower ();
         bool isInDirectory = lowerCaseAssetPath.IndexOf ("/sprites2/") != -1;
         
         if (isInDirectory)
        {
           // Debug.Log("Importing Sprite @ " + lowerCaseAssetPath);
           // TextureImporter textureImporter = (TextureImporter) assetImporter;
           // textureImporter.cubemapConvolution = TextureImporterCubemapConvolution.Cylindrical;
           // TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
           // textureImporter.SetTextureSettings(textureImporterSettings);
           // TextureImporterType.Singl
         }
     }
}

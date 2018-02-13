using UnityEngine;
using System.Collections;
using UnityEditor;

public class TextureImportTest : AssetPostprocessor 
 {
     
     // Use this for initialization
     void OnPostprocessTexture (Texture2D texture)
     {
         string lowerCaseAssetPath = assetPath.ToLower ();
         bool isInDirectory = lowerCaseAssetPath.IndexOf ("/sprites/") != -1;
         
         if (isInDirectory) 
         {
             TextureImporter textureImporter = (TextureImporter) assetImporter;
             TextureImporterSettings textureImporterSettings = new TextureImporterSettings();
             textureImporterSettings.textureType = TextureImporterType.Cookie;
 
             textureImporterSettings.generateCubemap = TextureImporterGenerateCubemap.Cylindrical;
 
             textureImporterSettings.seamlessCubemap = true;
 
             textureImporterSettings.alphaSource = TextureImporterAlphaSource.FromGrayScale;
             textureImporterSettings.npotScale = TextureImporterNPOTScale.ToSmaller;
             textureImporterSettings.readable = true;
             textureImporterSettings.mipmapEnabled = true;
             textureImporterSettings.borderMipmap = true;
             textureImporterSettings.mipmapFilter = TextureImporterMipFilter.BoxFilter;
 
             textureImporterSettings.wrapMode = TextureWrapMode.Clamp;
             textureImporterSettings.filterMode = FilterMode.Point;
 
             textureImporter.SetTextureSettings(textureImporterSettings);
         }
     }
 }

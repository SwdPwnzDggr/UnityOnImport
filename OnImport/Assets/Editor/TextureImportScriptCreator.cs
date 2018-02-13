using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextureImportScriptCreator
{

    public void CreateScript(string scriptName, string directoryName, TextureImportGUI gUI)
    {
        string name = scriptName.Replace(" ", "_");
        name = name.Replace("-", "_");
        string copyPath = "Assets/Editor/" + name + ".cs";
        Debug.Log("Creating Classfile: " + copyPath);
        //if (File.Exists(copyPath) == false)
        //  { // do not overwrite
        using (StreamWriter outfile = new StreamWriter(copyPath))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("using UnityEditor;");
            outfile.WriteLine("");
            outfile.WriteLine("public class " + name + " : AssetPostprocessor ");
            outfile.WriteLine(" {");
            outfile.WriteLine("     ");
            outfile.WriteLine("     // Use this for initialization");
            outfile.WriteLine("     void OnPostprocessTexture (Texture2D texture)");
            outfile.WriteLine("     {");
            outfile.WriteLine("         string lowerCaseAssetPath = assetPath.ToLower ();");
            outfile.WriteLine("         bool isInDirectory = lowerCaseAssetPath.IndexOf (\"/" + directoryName + "/\") != -1;");
            outfile.WriteLine("         ");
            outfile.WriteLine("         if (isInDirectory) ");
            outfile.WriteLine("         {");
            outfile.WriteLine("             TextureImporter textureImporter = (TextureImporter) assetImporter;");
            outfile.WriteLine("             TextureImporterSettings textureImporterSettings = new TextureImporterSettings();");
            outfile.WriteLine(TextureTypeWriter(gUI));
            outfile.WriteLine(TextureShapeWriter(gUI));
            if (gUI.textureShape == TextureImporterShape.TextureCube)
            {
                outfile.WriteLine(CubeMappingWriter(gUI));
                outfile.WriteLine(CubeConvolutionTypeWriter(gUI));
                outfile.WriteLine(CubeFixupEdgeSeamsWriter(gUI));
            }
            outfile.WriteLine(TextureSRGBWriter(gUI));
            outfile.WriteLine(TextureAlphaSourceWriter(gUI));
            outfile.WriteLine(TextureNPOTWriter(gUI));
            outfile.WriteLine(TextureReadWriteWriter(gUI));
            outfile.WriteLine(TextureGenerateMipMapsWriter(gUI));
            outfile.WriteLine(TextureBorderMipMapsWriter(gUI));
            outfile.WriteLine(TextureMipMapFilterWriter(gUI));
            outfile.WriteLine(TextureFadeoutMipMapWriter(gUI));
            outfile.WriteLine(TextureWrapModeWriter(gUI));
            outfile.WriteLine(TextureFilterModeWriter(gUI));
            outfile.WriteLine(TextureAnisoLevelWriter(gUI));
            if (gUI.textureType == TextureImportGUI.TextureType.NormalMap)
            {
                outfile.WriteLine(NormalCreateFromGrayscaleWriter(gUI));
                outfile.WriteLine(NormalBumpinessWriter(gUI));
                outfile.WriteLine(NormalBumpFilterWriter(gUI));
            }
            if (gUI.textureType == TextureImportGUI.TextureType.Cookie)
            {
                // TODO Find the cookie stuff in TextureImporterSettings
            }
            if (gUI.textureType == TextureImportGUI.TextureType.Sprite)
            {
                outfile.WriteLine(SpriteModeWriter(gUI));
                outfile.WriteLine(SpritePackingTagWriter(gUI));
                outfile.WriteLine(SpritePPUWriter(gUI));
                outfile.WriteLine(SpriteMeshTypeWriter(gUI));
                outfile.WriteLine(SpriteExtrudeWriter(gUI));
                outfile.WriteLine(SpritePivotWriter(gUI));

            }
            outfile.WriteLine("             textureImporter.SetTextureSettings(textureImporterSettings);");
            outfile.WriteLine("         }");
            outfile.WriteLine("     }");
            outfile.WriteLine(" }");
            //  }//File written
        }
        AssetDatabase.Refresh();
    }

    private string SpriteModeWriter(TextureImportGUI gUI)
    {
        switch (gUI.spriteMode)
        {
            case SpriteImportMode.None:
                return ("             textureImporterSettings.spriteMode = SpriteImportMode.None;");
            case SpriteImportMode.Single:
                return ("             textureImporterSettings.spriteMode = SpriteImportMode.Single;");
            case SpriteImportMode.Multiple:
                return ("             textureImporterSettings.spriteMode = SpriteImportMode.Multiple;");
            case SpriteImportMode.Polygon:
                return ("             textureImporterSettings.spriteMode = SpriteImportMode.Polygon;");
            default:
                return (" ");
        }
    }

    private string SpritePackingTagWriter(TextureImportGUI gUI)
    {
        if (gUI.packingTag == "") return " ";
        return ("             textureImporter.spritePackingTag = " + gUI.packingTag + ";");
    }

    private string SpritePPUWriter(TextureImportGUI gUI)
    {
        return ("             textureImporter.spritePixelsPerUnit = " + gUI.pixelsPerUnit + ";");
    }

    private string SpriteMeshTypeWriter(TextureImportGUI gUI)
    {
        switch (gUI.meshType)
        {
            case SpriteMeshType.FullRect:
                return ("             textureImporterSettings.spriteMeshType = SpriteMeshType.FullRect;");
            case SpriteMeshType.Tight:
                return ("             textureImporterSettings.spriteMeshType = SpriteMeshType.Tight;");
            default:
                return (" ");
        }
    }

    private string SpriteExtrudeWriter(TextureImportGUI gUI)
    {
        return ("             textureImporter.spriteExtrude = " + (uint)gUI.extrudeEdges + ";");
    }

    private string SpritePivotWriter(TextureImportGUI gUI)
    {
        if (gUI.spriteMode != SpriteImportMode.Single) return " ";

        switch (gUI.pivot)
        {
            case TextureImportGUI.Pivot.Center:
                return ("             textureImporterSettings.spritePivot = new Vector2(0.5f, 0.5f);");
            case TextureImportGUI.Pivot.TopLeft:
                return ("             textureImporterSettings.spritePivot = new Vector2(0, 1);");
            case TextureImportGUI.Pivot.Top:
                return ("             textureImporterSettings.spritePivot = new Vector2(0.5f, 1);");
            case TextureImportGUI.Pivot.TopRight:
                return ("             textureImporterSettings.spritePivot = new Vector2(1, 1);");
            case TextureImportGUI.Pivot.Left:
                return ("             textureImporterSettings.spritePivot = new Vector2(0, 0.5f);");
            case TextureImportGUI.Pivot.Right:
                return ("             textureImporterSettings.spritePivot = new Vector2(1, 0.5f);");
            case TextureImportGUI.Pivot.BottomLeft:
                return ("             textureImporterSettings.spritePivot = new Vector2(0, 0);");
            case TextureImportGUI.Pivot.Bottom:
                return ("             textureImporterSettings.spritePivot = new Vector2(0.5f, 0);");
            case TextureImportGUI.Pivot.BottomRight:
                return ("             textureImporterSettings.spritePivot = new Vector2(1, 0);");
            case TextureImportGUI.Pivot.Custom:
                return ("             textureImporterSettings.spritePivot = new Vector2(" + gUI.pivotPoint.x + "f," + gUI.pivotPoint.y + "f);");
            default:
                return (" ");
        }
    }


    private string TextureTypeWriter(TextureImportGUI gUI)
    {
        switch (gUI.textureType)
        {
            case TextureImportGUI.TextureType.Default:
                return ("             textureImporterSettings.textureType = TextureImporterType.Default;");
            case TextureImportGUI.TextureType.NormalMap:
                return ("             textureImporterSettings.textureType = TextureImporterType.NormalMap;");
            case TextureImportGUI.TextureType.EditorGUIAndLegacyGUI:
                return ("             textureImporterSettings.textureType = TextureImporterType.GUI;");
            case TextureImportGUI.TextureType.Sprite:
                return ("             textureImporterSettings.textureType = TextureImporterType.Sprite+;");
            case TextureImportGUI.TextureType.Cursor:
                return ("             textureImporterSettings.textureType = TextureImporterType.Cursor;");
            case TextureImportGUI.TextureType.Cookie:
                return ("             textureImporterSettings.textureType = TextureImporterType.Cookie;");
            case TextureImportGUI.TextureType.Lightmap:
                return ("             textureImporterSettings.textureType = TextureImporterType.Lightmap;");
            case TextureImportGUI.TextureType.SingleChannel:
                return ("             textureImporterSettings.textureType = TextureImporterType.SingleChannel;");
            default:
                return (" ");
        }
    }

    private string TextureShapeWriter(TextureImportGUI gUI)
    {
        if (gUI.textureType != TextureImportGUI.TextureType.Default ||
            gUI.textureType != TextureImportGUI.TextureType.NormalMap ||
            gUI.textureType != TextureImportGUI.TextureType.Cookie ||
            gUI.textureType != TextureImportGUI.TextureType.SingleChannel)
        {
            return (" ");
        }

        switch (gUI.textureShape)
        {
            case TextureImporterShape.Texture2D:
                return ("             textureImporterSettings.textureShape = TextureImporterShape.Texture2D;");
            case TextureImporterShape.TextureCube:
                return ("             textureImporterSettings.textureShape = TextureImporterType.Default;");
            default:
                return ("");
        }
    }

    private string CubeMappingWriter(TextureImportGUI gUI)
    {
        switch (gUI.mappingType)
        {
            case TextureImportGUI.MappingType.Spheremap:
                return ("             textureImporterSettings.generateCubemap = TextureImporterGenerateCubemap.Spheremap;");
            case TextureImportGUI.MappingType.Cylindrical:
                return ("             textureImporterSettings.generateCubemap = TextureImporterGenerateCubemap.Cylindrical;");
            case TextureImportGUI.MappingType.Auto:
                return ("             textureImporterSettings.generateCubemap = TextureImporterGenerateCubemap.AutoCubemap;");
            case TextureImportGUI.MappingType.Cubic:
                return ("             textureImporterSettings.generateCubemap = TextureImporterGenerateCubemap.FullCubemap;");
            default:
                return (" ");
        }
    }

    private string CubeConvolutionTypeWriter(TextureImportGUI gUI)
    {
        if (gUI.textureType != TextureImportGUI.TextureType.Default) return " ";

        switch (gUI.convolutionType)
        {
            case TextureImporterCubemapConvolution.None:
                return ("             textureImporterSettings.cubemapConvolution = TextureImporterCubemapConvolution.None;");
            case TextureImporterCubemapConvolution.Specular:
                return ("             textureImporterSettings.cubemapConvolution = TextureImporterCubemapConvolution.Specular;");
            case TextureImporterCubemapConvolution.Diffuse:
                return ("             textureImporterSettings.cubemapConvolution = TextureImporterCubemapConvolution.Diffuse;");
            default:
                return (" ");
        }
    }

    private string CubeFixupEdgeSeamsWriter(TextureImportGUI gUI)
    {
        if (gUI.fixupEdgeSeams)
        {
            return ("             textureImporterSettings.seamlessCubemap = true;");
        }
        return ("             textureImporterSettings.seamlessCubemap = false;");
    }

    private string TextureSRGBWriter(TextureImportGUI gUI)
    {
        if (gUI.textureType != TextureImportGUI.TextureType.Default ||
            gUI.textureType != TextureImportGUI.TextureType.Sprite ||
            gUI.textureType != TextureImportGUI.TextureType.Cursor)
        {
            return (" ");
        }

        if (gUI.sRGB)
        {
            return ("             textureImporterSettings.sRGBTexture = true;");
        }
        return ("             textureImporterSettings.sRGBTexture = false;");
    }

    private string TextureAlphaSourceWriter(TextureImportGUI gUI)
    {
        if (gUI.textureType == TextureImportGUI.TextureType.Lightmap ||
            gUI.textureType == TextureImportGUI.TextureType.NormalMap)
        {
            return (" ");
        }
        switch (gUI.alphaSource)
        {
            case TextureImporterAlphaSource.None:
                return ("             textureImporterSettings.alphaSource = TextureImporterAlphaSource.None;");
            case TextureImporterAlphaSource.FromInput:
                return ("             textureImporterSettings.alphaSource = TextureImporterAlphaSource.FromInput;");
            case TextureImporterAlphaSource.FromGrayScale:
                return ("             textureImporterSettings.alphaSource = TextureImporterAlphaSource.FromGrayScale;");
            default:
                return (" ");
        }
    }

    private string TextureNPOTWriter(TextureImportGUI gUI)
    {
        switch (gUI.nonPowerofTwo)
        {
            case TextureImporterNPOTScale.None:
                return ("             textureImporterSettings.npotScale = TextureImporterNPOTScale.None;");
            case TextureImporterNPOTScale.ToNearest:
                return ("             textureImporterSettings.npotScale = TextureImporterNPOTScale.ToNearest;");
            case TextureImporterNPOTScale.ToLarger:
                return ("             textureImporterSettings.npotScale = TextureImporterNPOTScale.ToLarger;");
            case TextureImporterNPOTScale.ToSmaller:
                return ("             textureImporterSettings.npotScale = TextureImporterNPOTScale.ToSmaller;");
            default:
                return " ";
        }
    }

    private string TextureReadWriteWriter(TextureImportGUI gUI)
    {
        if (gUI.readWriteEnabled)
        {
            return ("             textureImporterSettings.readable = true;");
        }
        return ("             textureImporterSettings.readable = false;");
    }

    private string TextureGenerateMipMapsWriter(TextureImportGUI gUI)
    {
        if (gUI.generateMipMaps)
        {
            return ("             textureImporterSettings.mipmapEnabled = true;");
        }
        return ("             textureImporterSettings.mipmapEnabled = false;");
    }

    private string TextureBorderMipMapsWriter(TextureImportGUI gUI)
    {
        if (!gUI.generateMipMaps) return " ";

        if (gUI.borderMipMaps)
        {
            return ("             textureImporterSettings.borderMipmap = true;");
        }
        return ("             textureImporterSettings.borderMipmap = false;");
    }

    private string TextureMipMapFilterWriter(TextureImportGUI gUI)
    {
        if (!gUI.generateMipMaps) return " ";

        switch (gUI.mipMapFiltering)
        {
            case TextureImporterMipFilter.BoxFilter:
                return ("             textureImporterSettings.mipmapFilter = TextureImporterMipFilter.BoxFilter;");
            case TextureImporterMipFilter.KaiserFilter:
                return ("             textureImporterSettings.mipmapFilter = TextureImporterMipFilter.KaiserFilter;");
            default:
                return " ";
        }
    }

    private string TextureFadeoutMipMapWriter(TextureImportGUI gUI)
    {
        if (!gUI.fadeoutMipMaps) return " ";

        if (gUI.readWriteEnabled)
        {
            return ("             textureImporterSettings.fadeout = true;");
        }
        return ("             textureImporterSettings.fadeout = false;");
    }

    private string TextureWrapModeWriter(TextureImportGUI gUI)
    {
        switch (gUI.wrapMode)
        {
            case TextureWrapMode.Repeat:
                return ("             textureImporterSettings.wrapMode = TextureWrapMode.Repeat;");
            case TextureWrapMode.Clamp:
                return ("             textureImporterSettings.wrapMode = TextureWrapMode.Clamp;");
            default:
                return (" ");
        }
    }

    private string TextureFilterModeWriter(TextureImportGUI gUI)
    {
        switch (gUI.filterMode)
        {
            case FilterMode.Point:
                return ("             textureImporterSettings.filterMode = FilterMode.Point;");
            case FilterMode.Bilinear:
                return ("             textureImporterSettings.filterMode = FilterMode.Bilinear;");
            case FilterMode.Trilinear:
                return ("             textureImporterSettings.filterMode = FilterMode.Trilinear;");
            default:
                return (" ");
        }
    }

    private string TextureAnisoLevelWriter(TextureImportGUI gUI)
    {
        if (gUI.filterMode != FilterMode.Point)
        {
            return ("             textureImporterSettings.aniso = " + gUI.anisoLevel + ";");
        }
        return (" ");
    }

    private string NormalCreateFromGrayscaleWriter(TextureImportGUI gUI)
    {
        if (gUI.createAlphaFromGrayscale)
        {
            return ("             textureImporterSettings.convertToNormalMap = true;");
        }
        return ("             textureImporterSettings.convertToNormalMap = false;");
    }

    private string NormalBumpinessWriter(TextureImportGUI gUI)
    {
        if (!gUI.createAlphaFromGrayscale) return " ";

        return ("             textureImporterSettings.heightmapScale = " + gUI.bumpiness + "f;");
    }

    private string NormalBumpFilterWriter(TextureImportGUI gUI)
    {
        if (!gUI.createAlphaFromGrayscale) return " ";

        switch (gUI.normalBumpFiltering)
        {
            case TextureImporterNormalFilter.Sobel:
                return ("             textureImporterSettings.normalMapFilter = TextureImporterNormalFilter.Sobel;");
            case TextureImporterNormalFilter.Standard:
                return ("             textureImporterSettings.normalMapFilter = TextureImporterNormalFilter.Standard;");
            default:
                return (" ");
        }
    }
}

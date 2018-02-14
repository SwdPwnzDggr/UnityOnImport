using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureImportGUI
{
    TextureImportScriptCreator scriptCreator;

    //Base Texture Settings
    public enum TextureType { Default, NormalMap, EditorGUIAndLegacyGUI, Sprite, Cursor, Cookie, Lightmap, SingleChannel };
    public TextureType textureType = TextureType.Default;
    //enum TextureShape { TwoDimensional, Cube }
    public TextureImporterShape textureShape = TextureImporterShape.Texture2D;

    //Alpha and Texture Settings
    //enum AlphaSource { None, InputTextureAlpha, FromGrayscale }
    public TextureImporterAlphaSource alphaSource = TextureImporterAlphaSource.None;
    public bool alphaIsTransparency = false; //Goes to TextureImporter.alphaIsTransparency
    public bool sRGB = true; //Goes to TextureImporter.sRGBTexture

    //Advanced Settings
    //enum NonPowerofTwo { None, ToNearest, ToSmallest, ToLargest }
    public TextureImporterNPOTScale nonPowerofTwo = TextureImporterNPOTScale.None;
    public bool readWriteEnabled = true;
    public bool generateMipMaps = true;
    public bool borderMipMaps = true;
    public bool fadeoutMipMaps = false;
    public //enum MipMapFiltering { Box, Kaiser }
    TextureImporterMipFilter mipMapFiltering = TextureImporterMipFilter.BoxFilter;

    //Wrap and FilterMode
    //public enum WrapMode { Clamp, Repeat }
    public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
    //public enum FilterMode { Point, Bilinear, Triliner }
    public FilterMode filterMode = FilterMode.Point;
    public int anisoLevel = 0; // Max 16

    // Normal Map 
    public bool createAlphaFromGrayscale = false;
    public float bumpiness = 0.1f; //Max .3
                                   //public enum NormalBumpFiltering { Sharp, Smooth }
    public TextureImporterNormalFilter normalBumpFiltering = TextureImporterNormalFilter.Standard;

    // Sprite
    //public enum SpriteMode { Single, Multiple, Polygon }
    public SpriteImportMode spriteMode = SpriteImportMode.Single;
    public string packingTag;
    public float pixelsPerUnit = 100; //Min 0
                                      // public enum MeshType { Tight, FullRect }
    public SpriteMeshType meshType = SpriteMeshType.Tight;
    public int extrudeEdges = 0; //Max 32
    public enum Pivot { Center, TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight, Custom }
    public Pivot pivot = Pivot.Center;
    public Vector2 pivotPoint;

    // Cookie 
    public enum LightType { Spotlight, Directional, Point }
    public LightType lightType = LightType.Spotlight;

    // Cube Map
    public enum MappingType { Spheremap, Cylindrical, Cubic, Auto }
    public MappingType mappingType = MappingType.Spheremap;
    //public enum ConvolutionType { None, Specular, Diffuse }
    public TextureImporterCubemapConvolution convolutionType = TextureImporterCubemapConvolution.None;
    public bool fixupEdgeSeams = false;


    public void DisplayGUI()
    {
        GUILayout.Label("Texture Import Settings", EditorStyles.boldLabel);
        GUILayout.Label("Base Settings", EditorStyles.miniBoldLabel);

        textureType = (TextureType)EditorGUILayout.EnumPopup("Texture Type", textureType);

        DisplayTextureTypeSpecificOptions();

    }

    private void DisplayTextureTypeSpecificOptions()
    {
        switch (textureType)
        {
            case TextureType.Default:
                DisplayTextureShape();
                Display_sRGBSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.NormalMap:
                DisplayTextureShape();
                DisplayNormalAlphaFromGrayscaleSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.EditorGUIAndLegacyGUI:
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.Sprite:
                DisplaySpriteSettings();
                Display_sRGBSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.Cursor:
                Display_sRGBSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.Cookie:
                DisplayCubeMapSettings();
                DisplayCookieSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.Lightmap:
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.SingleChannel:
                DisplayTextureShape();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            default:
                Debug.LogError("No Texture Type");
                break;
        }
    }

    private void Display_sRGBSettings()
    {
        sRGB = EditorGUILayout.Toggle("sRGB (Color Texture)", sRGB);
    }

    private void DisplayAlphaSourceSettings()
    {
        alphaSource = (TextureImporterAlphaSource)EditorGUILayout.EnumPopup("Alpha Source", alphaSource);
        if (alphaSource != TextureImporterAlphaSource.None)
        {
            alphaIsTransparency = EditorGUILayout.Toggle("Alpha Is Transparency", alphaIsTransparency);
        }
    }

    private void DisplayTextureShape()
    {
        textureShape = (TextureImporterShape)EditorGUILayout.EnumPopup("Texture Shape", textureShape);
        DisplayCubeMapSettings();

    }

    private void DisplayCubeMapSettings()
    {
        if (textureShape == TextureImporterShape.TextureCube)
        {
            EditorGUI.indentLevel++;
            mappingType = (MappingType)EditorGUILayout.EnumPopup("Mapping", mappingType);
            if (textureType == TextureType.Default)
            {
                convolutionType = (TextureImporterCubemapConvolution)EditorGUILayout.EnumPopup("Convolution Type", convolutionType);
            }
            fixupEdgeSeams = EditorGUILayout.Toggle("Fixup Edge Seams", fixupEdgeSeams);
            EditorGUI.indentLevel--;
        }
    }

    private void DisplayAdvancedSettings()
    {
        GUILayout.Label("Advanced Settings", EditorStyles.miniBoldLabel);

        EditorGUI.indentLevel++;

        nonPowerofTwo = (TextureImporterNPOTScale)EditorGUILayout.EnumPopup("Non Power of 2", nonPowerofTwo);
        readWriteEnabled = EditorGUILayout.Toggle("Read/Write Enabled", readWriteEnabled);
        generateMipMaps = EditorGUILayout.Toggle("Generate Mip Maps", generateMipMaps);
        if (generateMipMaps)
        {
            EditorGUI.indentLevel++;
            borderMipMaps = EditorGUILayout.Toggle("Border Mip Maps", borderMipMaps);
            mipMapFiltering = (TextureImporterMipFilter)EditorGUILayout.EnumPopup("Mip Map Filtering", mipMapFiltering);
            fadeoutMipMaps = EditorGUILayout.Toggle("Fadeout Mip Maps", fadeoutMipMaps);
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
    }

    private void DisplayWrapAndFilterSettings()
    {
        wrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("Wrap Mode", wrapMode);
        filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", filterMode);
        if (filterMode != FilterMode.Point)
        {
            anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 16);
        }
    }

    private void DisplayNormalAlphaFromGrayscaleSettings()
    {
        createAlphaFromGrayscale = EditorGUILayout.Toggle("Create from Grayscale", createAlphaFromGrayscale);
        if (createAlphaFromGrayscale)
        {
            EditorGUI.indentLevel++;
            bumpiness = EditorGUILayout.Slider("Bumpiness", bumpiness, 0.0f, 0.3f);
            normalBumpFiltering = (TextureImporterNormalFilter)EditorGUILayout.EnumPopup("Filtering", normalBumpFiltering);
            EditorGUI.indentLevel--;
        }
    }

    private void DisplaySpriteSettings()
    {
        spriteMode = (SpriteImportMode)EditorGUILayout.EnumPopup("Sprite Mode", spriteMode);
        EditorGUI.indentLevel++;
        packingTag = EditorGUILayout.TextField("Packing Tag", packingTag);
        pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", pixelsPerUnit);
        meshType = (SpriteMeshType)EditorGUILayout.EnumPopup("Mesh Type", meshType);
        extrudeEdges = EditorGUILayout.IntSlider("Extrude Edges", extrudeEdges, 0, 32);

        if (spriteMode == SpriteImportMode.Single)
        {
            pivot = (Pivot)EditorGUILayout.EnumPopup("Pivot", pivot);
            if (pivot == Pivot.Custom)
            {
                pivotPoint = EditorGUILayout.Vector2Field("Custom Pivot Point", pivotPoint);
            }
        }
        EditorGUI.indentLevel--;
    }

    private void DisplayCookieSettings()
    {
        lightType = (LightType)EditorGUILayout.EnumPopup("Light Type", lightType);
        if (lightType == LightType.Point)
        {
            textureShape = TextureImporterShape.TextureCube;
        }
        else
        {
            textureShape = TextureImporterShape.Texture2D;
        }
    }

    void CreateScript()
    {
    }
}

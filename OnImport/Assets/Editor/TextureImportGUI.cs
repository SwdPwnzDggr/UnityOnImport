using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TextureImportGUI
{
    //Base Texture Settings
    //enum TextureType { Default, NormalMap, EditorGUIAndLegacyGUI, Sprite, Cursor, Cookie, Lightmap, SingleChannel };
    TextureImporterType textureType = TextureImporterType.Default;
    //enum TextureShape { TwoDimensional, Cube }
    TextureImporterShape textureShape = TextureImporterShape.Texture2D;

    //Alpha and Texture Settings
    //enum AlphaSource { None, InputTextureAlpha, FromGrayscale }
    TextureImporterAlphaSource alphaSource = TextureImporterAlphaSource.None;
    bool alphaIsTransparency = false; //Goes to TextureImporter.alphaIsTransparency
    bool sRGB = true; //Goes to TextureImporter.sRGBTexture
    
    //Advanced Settings
    //enum NonPowerofTwo { None, ToNearest, ToSmallest, ToLargest }
    TextureImporterNPOTScale nonPowerofTwo = TextureImporterNPOTScale.None;
    bool readWriteEnabled = true;
    bool generateMipMaps = true;
    bool borderMipMaps = true;
    bool fadeoutMipMaps = false;
    //enum MipMapFiltering { Box, Kaiser }
    TextureImporterMipFilter mipMapFiltering = TextureImporterMipFilter.BoxFilter;
    
    //Wrap and FilterMode
    enum WrapMode { Clamp, Repeat }
    WrapMode wrapMode = WrapMode.Clamp;
    enum FilterMode { Point, Bilinear, Triliner }
    FilterMode filterMode = FilterMode.Point;
    int anisoLevel = 0; // Max 16

    // Normal Map 
    bool createAlphaFromGrayscale = false;
    float bumpiness = 0.1f; //Max .3
    enum NormalBumpFiltering { Sharp, Smooth }
    NormalBumpFiltering normalBumpFiltering = NormalBumpFiltering.Sharp;

    // Sprite
    enum SpriteMode { Single, Multiple, Polygon }
    SpriteMode spriteMode = SpriteMode.Single;
    string packingTag;
    float pixelsPerUnit = 100; //Min 0
    enum MeshType { Tight, FullRect }
    MeshType meshType = MeshType.Tight;
    int extrudeEdges = 0; //Max 32
    enum Pivot { Center, TopLeft, Top, TopRight, Left, Right, BottomLeft, Bottom, BottomRight, Custom }
    Pivot pivot = Pivot.Center;
    Vector2 pivotPoint;

    // Cookie 
    enum LightType { Spotlight, Directional, Point }
    LightType lightType = LightType.Spotlight;

    // Cube Map
    enum MappingType { Spheremap, Cylindrical, Cubic, Auto }
    MappingType mappingType = MappingType.Spheremap;
    enum ConvolutionType { None, Specular, Diffuse }
    ConvolutionType convolutionType = ConvolutionType.None;
    bool fixupEdgeSeams = false;


    public void DisplayGUI()
    {
        GUILayout.Label("Texture Import Settings", EditorStyles.boldLabel);
        GUILayout.Label("Base Settings", EditorStyles.miniBoldLabel);

        textureType = (TextureImporterType)EditorGUILayout.EnumPopup("Texture Type", textureType);

        DisplayTextureTypeSpecificOptions();

        if (GUILayout.Button("Generate"))
        {
            CreateScript();
        }
    }

    private void DisplayTextureTypeSpecificOptions()
    {
        switch (textureType)
        {
            case TextureImporterType.Default:
                DisplayTextureShape();
                Display_sRGBSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureImporterType.NormalMap:
                DisplayTextureShape();
                DisplayNormalAlphaFromGrayscaleSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureImporterType.GUI:
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureImporterType.Sprite:
                DisplaySpriteSettings();
                Display_sRGBSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureImporterType.Cursor:
                Display_sRGBSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureImporterType.Cookie:
                DisplayCubeMapSettings();
                DisplayCookieSettings();
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureImporterType.Lightmap:
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureImporterType.SingleChannel:
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
        if(alphaSource != TextureImporterAlphaSource.None)
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
            if (textureType == TextureImporterType.Default)
            {
                convolutionType = (ConvolutionType)EditorGUILayout.EnumPopup("Convolution Type", convolutionType);
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
        if(generateMipMaps)
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
        wrapMode = (WrapMode)EditorGUILayout.EnumPopup("Wrap Mode", wrapMode);
        filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", filterMode);
        if(filterMode != FilterMode.Point)  
        {
            anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 16);
        }
    }

    private void DisplayNormalAlphaFromGrayscaleSettings()
    {
        createAlphaFromGrayscale = EditorGUILayout.Toggle("Create from Grayscale", createAlphaFromGrayscale);
        if(createAlphaFromGrayscale)
        {
            EditorGUI.indentLevel++;
            bumpiness = EditorGUILayout.Slider("Bumpiness", bumpiness, 0.0f, 0.3f);
            normalBumpFiltering = (NormalBumpFiltering)EditorGUILayout.EnumPopup("Filtering", normalBumpFiltering);
            EditorGUI.indentLevel--;
        }
    }

    private void DisplaySpriteSettings()
    {
        spriteMode = (SpriteMode)EditorGUILayout.EnumPopup("Sprite Mode", spriteMode);
        EditorGUI.indentLevel++;
        packingTag = EditorGUILayout.TextField("Packing Tag", packingTag);
        pixelsPerUnit = EditorGUILayout.FloatField("Pixels Per Unit", pixelsPerUnit);
        meshType = (MeshType)EditorGUILayout.EnumPopup("Mesh Type", meshType);
        extrudeEdges = EditorGUILayout.IntSlider("Extrude Edges", extrudeEdges, 0, 32);

        if (spriteMode == SpriteMode.Single)
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
        if(lightType == LightType.Point)
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

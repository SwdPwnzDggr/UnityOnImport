using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class TextureImportGUI
{
    enum TextureType { Default, NormalMap, EditorGUIAndLegacyGUI, Sprite, Cursor, Cookie, Lightmap, SingleChannel };
    TextureType type = TextureType.Default;

    enum TextureShape { TwoDimensional, Cube }
    TextureShape shape = TextureShape.TwoDimensional;

    enum AlphaSource { None, InputTextureAlpha, FromGrayscale }
    AlphaSource alphaSource = AlphaSource.None;

    bool alphaIsTransparency = false;

    bool sRGB = true;

    enum NonPowerofTwo { None, ToNearest, ToSmallest, ToLargest }
    NonPowerofTwo nonPowerofTwo = NonPowerofTwo.None;
    
    bool readWriteEnabled = true;
    bool generateMipMaps = true;
    bool borderMipMaps = true;
    bool fadeoutMipMaps = false;

    enum MipMapFiltering { Box, Kaiser }
    MipMapFiltering mipMapFiltering = MipMapFiltering.Box;

    enum WrapMode { Clamp, Repeat }
    WrapMode wrapMode = WrapMode.Clamp;

    enum FilterMode { Point, Bilinear, Triliner }
    FilterMode filterMode = FilterMode.Point;

    int anisoLevel = 0;



    public void DisplayGUI()
    {
        GUILayout.Label("Texture Import Settings", EditorStyles.boldLabel);
        GUILayout.Label("Base Settings", EditorStyles.miniBoldLabel);

        type = (TextureType)EditorGUILayout.EnumPopup("Texture Type", type);

        DisplayTextureTypeSpecificOptions();

        if (GUILayout.Button("Generate"))
        {
            CreateScript();
        }
    }

    private void DisplayTextureTypeSpecificOptions()
    {
        switch (type)
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
                //Create from Greyscale
                    //if yes
                    //Bumpiness
                    //Filtering
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.EditorGUIAndLegacyGUI:
                DisplayAlphaSourceSettings();
                DisplayAdvancedSettings();
                DisplayWrapAndFilterSettings();
                break;
            case TextureType.Sprite:
                //Sprite Mode
                //Packing Tag
                //Pixels Per Unit
                //MeshType
                //Extrude Edges
                //If Single Sprite Mode 
                    //Pivot
                    
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
                //LightType
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
        alphaSource = (AlphaSource)EditorGUILayout.EnumPopup("Alpha Source", alphaSource);
        if(alphaSource != AlphaSource.None)
        {
            alphaIsTransparency = EditorGUILayout.Toggle("Alpha Is Transparency", alphaIsTransparency);
        }
    }

    private void DisplayTextureShape()
    {
        shape = (TextureShape)EditorGUILayout.EnumPopup("Texture Shape", shape);
        //Display Cube Settings
    }

    private void DisplayAdvancedSettings()
    {
        GUILayout.Label("Advanced Settings", EditorStyles.miniBoldLabel);

        EditorGUI.indentLevel++;

        nonPowerofTwo = (NonPowerofTwo)EditorGUILayout.EnumPopup("Non Power of 2", nonPowerofTwo);
        readWriteEnabled = EditorGUILayout.Toggle("Read/Write Enabled", readWriteEnabled);
        generateMipMaps = EditorGUILayout.Toggle("Generate Mip Maps", generateMipMaps);
        if(generateMipMaps)
        {
            EditorGUI.indentLevel++;
            borderMipMaps = EditorGUILayout.Toggle("Border Mip Maps", borderMipMaps);
            mipMapFiltering = (MipMapFiltering)EditorGUILayout.EnumPopup("Mip Map Filtering", mipMapFiltering);
            fadeoutMipMaps = EditorGUILayout.Toggle("Fadeout Mip Maps", fadeoutMipMaps);
            EditorGUI.indentLevel--;
        }

        EditorGUI.indentLevel--;
    }

    private void DisplayWrapAndFilterSettings()
    {
        wrapMode = (WrapMode)EditorGUILayout.EnumPopup("Wrap Mode", wrapMode);
        filterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", filterMode);
        if(filterMode != FilterMode.Point)  //TODO update for CUBE Maps
        {
            anisoLevel = EditorGUILayout.IntSlider("Aniso Level", anisoLevel, 0, 16);
        }
    }

    void CreateScript()
    {

    }
}

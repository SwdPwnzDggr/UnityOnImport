using System.IO;
using UnityEditor;
using UnityEngine;

public class OnImport : EditorWindow
{
    string scriptName;
    string directoryName;
    bool groupEnabled;
    enum AssetType { NotSelected,Audio,Material,Model,Texture}
    AssetType assetType = AssetType.NotSelected;

    TextureImportGUI textureImporter;
    AudioImportGUI audioImporter;

    TextureImportScriptCreator textureScriptCreator;


    // Add menu item named "My Window" to the Window menu
    [MenuItem("Window/On Import")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(OnImport));
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        scriptName = EditorGUILayout.TextField("Class Name", scriptName);
        directoryName = EditorGUILayout.TextField("Directory Name", directoryName);
        assetType = (AssetType)EditorGUILayout.EnumPopup("Asset Type",assetType);
        switch(assetType)
        {
            case (AssetType.NotSelected):
                return;
            case (AssetType.Audio):
                DisplayAudioOptions();
                break;
            case (AssetType.Material):
                DisplayMaterialOptions();
                break;
            case (AssetType.Model):
                DisplayModelOptions();
                break;
            case (AssetType.Texture):
                DisplayTextureOptions();
                break;
            default:
                Debug.LogError("Somehow there is no type Selected on OnImport");
                break;
        }
    }

    void DisplayAudioOptions()
    {
        if (audioImporter == null)
        {
            audioImporter = new AudioImportGUI();
        }
        audioImporter.DisplayGUI();
    }

    void DisplayTextureOptions()
    {
        if (textureImporter == null) textureImporter = new TextureImportGUI();


        if (textureScriptCreator == null) textureScriptCreator = new TextureImportScriptCreator();

        textureImporter.DisplayGUI();
        if(GUILayout.Button("Generate"))
        {
            textureScriptCreator.CreateScript(scriptName, directoryName, textureImporter);
        }
    }
    void DisplayMaterialOptions()
    {
        GUILayout.Label("Material Import Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate"))
        {
        }
    }
    void DisplayModelOptions()
    {
        GUILayout.Label("Model Import Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate"))
        {
        }
    }

}

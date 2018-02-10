using System.IO;
using UnityEditor;
using UnityEngine;

public class OnImport : EditorWindow
{
    string scriptName;
    string directoryName;
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    enum AssetType { NotSelected,Audio,Material,Model,Texture}
    AssetType assetType = AssetType.NotSelected;

    TextureImportGUI textureImporter;


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
        GUILayout.Label("Audio Import Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate"))
        {
            CreateScript();
        }
    }

    void DisplayTextureOptions()
    {
        if (textureImporter == null)
        {
            textureImporter = new TextureImportGUI();
        }
        textureImporter.DisplayGUI();
    }
    void DisplayMaterialOptions()
    {
        GUILayout.Label("Material Import Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate"))
        {
            CreateScript();
        }
    }
    void DisplayModelOptions()
    {
        GUILayout.Label("Model Import Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate"))
        {
            CreateScript();
        }
    }

    void CreateScript()
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
                outfile.WriteLine("         bool isInDirectory = lowerCaseAssetPath.IndexOf (\"/"+ directoryName.ToString() + "/\") != -1;");
                outfile.WriteLine("         ");
                outfile.WriteLine("         if (isInDirectory) ");
                outfile.WriteLine("         {");
                outfile.WriteLine("             TextureImporter textureImporter = (TextureImporter) assetImporter;");
                outfile.WriteLine("             textureImporter.textureType = TextureImporterType.Sprite;");
                outfile.WriteLine("         }");
                outfile.WriteLine("     }");
                outfile.WriteLine(" }");
          //  }//File written
        }
        AssetDatabase.Refresh();
    }
}

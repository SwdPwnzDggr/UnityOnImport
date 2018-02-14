using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AudioImportScriptCreator
{
    public void CreateScript(string scriptName, string directoryName, AudioImportGUI gUI)
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
            outfile.WriteLine("     void OnPostprocessAudio (AudioClip audio)");
            outfile.WriteLine("     {");
            outfile.WriteLine("         string lowerCaseAssetPath = assetPath.ToLower ();");
            outfile.WriteLine("         bool isInDirectory = lowerCaseAssetPath.IndexOf (\"/" + directoryName + "/\") != -1;");
            outfile.WriteLine("         ");
            outfile.WriteLine("         if (isInDirectory) ");
            outfile.WriteLine("         {");
            outfile.WriteLine("             AudioImporter audioImporter = (AudioImporter)assetImporter;");
            outfile.WriteLine(AudioMonoWriter(gUI));
            outfile.WriteLine(AudioLoadInWriter(gUI));
            outfile.WriteLine("         }");
            outfile.WriteLine("     }");
            outfile.WriteLine(" }");
            //  }//File written
        }
        AssetDatabase.Refresh();
    }

    private string AudioLoadInWriter(AudioImportGUI gUI)
    {
        if (gUI.loadInBackground)
        {
            return ("             audioImporter.loadInBackground = true;");
        }
        return ("             audioImporter.loadInBackground = false;");
    }

    private string AudioMonoWriter(AudioImportGUI gUI)
    {
        if (gUI.forceToMono)
        {
            return ("             audioImporter.forceToMono = true;");
        }
        return ("             audioImporter.forceToMono = false;");
    }
}
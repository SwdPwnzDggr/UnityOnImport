using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioImportGUI
{
    public bool forceToMono = false;   // Goes to AudioImporter.forceToMono
    public bool normalize = true;  // TODO Find where this goes
    public bool loadInBackground = false; // Goes to AudioImporter.loadInBackground

    public void DisplayGUI()
    {
        GUILayout.Label("Audio Import Settings", EditorStyles.boldLabel);
        GUILayout.Label("Base Settings", EditorStyles.miniBoldLabel);

        forceToMono = EditorGUILayout.Toggle("Force To Mono", forceToMono);
        if(forceToMono)
        {
            EditorGUI.indentLevel++;
            // TODO Find where this goes
            //normalize = EditorGUILayout.Toggle("Normalize", normalize); 
            EditorGUI.indentLevel--;
        }
        loadInBackground = EditorGUILayout.Toggle("Load In Background", loadInBackground);
    }

}

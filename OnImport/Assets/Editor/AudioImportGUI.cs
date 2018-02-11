using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioImportGUI
{
    bool forceToMono = false;   // Goes to AudioImporter.forceToMono
    bool normalize = true;  // TODO Find where this goes
    bool loadInBackground = false; // Goes to AudioImporter.loadInBackground

    /* Audio Compression Settings
    AudioClipLoadType audioClipLoadType = AudioClipLoadType.DecompressOnLoad;
    AudioCompressionFormat audioCompressionFormat = AudioCompressionFormat.Vorbis;
    int quality = 100; 
    */
    public void DisplayGUI()
    {
        GUILayout.Label("Audio Import Settings", EditorStyles.boldLabel);
        GUILayout.Label("Base Settings", EditorStyles.miniBoldLabel);

        forceToMono = EditorGUILayout.Toggle("Force To Mono", forceToMono);
        if(forceToMono)
        {
            EditorGUI.indentLevel++;
            normalize = EditorGUILayout.Toggle("Normalize", normalize);
            EditorGUI.indentLevel--;
        }
        loadInBackground = EditorGUILayout.Toggle("Load In Background", loadInBackground);

        if (GUILayout.Button("Generate"))
        {
            CreateScript();
        }
    }

    private void CreateScript()
    {

    }
}

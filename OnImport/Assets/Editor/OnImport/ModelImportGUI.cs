using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ModelImportGUI
{
    public float globalScale = 1f;
    public bool useFileScale = true;
    public ModelImporterMeshCompression meshCompressionType = ModelImporterMeshCompression.Off;
    public bool readWriteEnabled = true;
    public bool optimizeMesh = true;
    public bool importBlendshapes = true;
    public bool generateColliders = false;
    public bool keepQuads = false; //TODO find where this goes
    public bool swapUVs = false;
    public bool lightmapUVs = false;

    public ModelImporterNormals importerNormals = ModelImporterNormals.Import;
    public int smoothingAngle = 60;
    public ModelImporterTangents importerTangents = ModelImporterTangents.Import;

    public bool importMaterials = true;
    public ModelImporterMaterialName materialNameMode = ModelImporterMaterialName.BasedOnMaterialName;
    public ModelImporterMaterialSearch materialSearch = ModelImporterMaterialSearch.RecursiveUp;

    public ModelImporterAnimationType animationType = ModelImporterAnimationType.Generic;
    public bool optimzeGameObjects = true;

    public bool importAnimation = true;

    public void DisplayGUI()
    {
        GUILayout.Label("Model Import Settings", EditorStyles.boldLabel);
        DisplayBaseSettings();
        DisplayNormalSettings();
        DisplayMaterialSettings();
        DisplayRigSettings();
        DisplayAnimationSettings();
    }

    private void DisplayNormalSettings()
    {
        importerNormals = (ModelImporterNormals)EditorGUILayout.EnumPopup("Normals", importerNormals);
        smoothingAngle = EditorGUILayout.IntSlider("Smoothing Angle", smoothingAngle, 0, 180);
        importerTangents = (ModelImporterTangents)EditorGUILayout.EnumPopup("Tangents", importerTangents);
    }

    private void DisplayMaterialSettings()
    {
        GUILayout.Label("Material Settings", EditorStyles.miniBoldLabel);
        importMaterials = EditorGUILayout.Toggle("Import Materials", importMaterials);
        materialNameMode = (ModelImporterMaterialName)EditorGUILayout.EnumPopup("Material Naming", materialNameMode);
        materialSearch = (ModelImporterMaterialSearch)EditorGUILayout.EnumPopup("Material Search", materialSearch);
    }

    private void DisplayRigSettings()
    {
        GUILayout.Label("Rig Settings", EditorStyles.miniBoldLabel);
        animationType = (ModelImporterAnimationType)EditorGUILayout.EnumPopup("Animation Type", animationType);
        optimzeGameObjects = EditorGUILayout.Toggle("Optimize Game Objects", optimzeGameObjects);
    }

    private void DisplayAnimationSettings()
    {
        GUILayout.Label("Animation Settings", EditorStyles.miniBoldLabel);
        importAnimation = EditorGUILayout.Toggle("Import Animation", importAnimation);
    }

    private void DisplayBaseSettings()
    {
        GUILayout.Label("Base Settings", EditorStyles.miniBoldLabel);
        globalScale = EditorGUILayout.FloatField("Scale Factor", globalScale);
        useFileScale = EditorGUILayout.Toggle("Use File Scale", useFileScale);
        meshCompressionType = (ModelImporterMeshCompression)EditorGUILayout.EnumPopup("Mesh Compression", meshCompressionType);
        readWriteEnabled = EditorGUILayout.Toggle("Read/Write Enabled", readWriteEnabled);
        optimizeMesh = EditorGUILayout.Toggle("Optimze Mesh", optimizeMesh);
        importBlendshapes = EditorGUILayout.Toggle("Import BlendShapes", importBlendshapes);
        generateColliders = EditorGUILayout.Toggle("Generate Colliders", generateColliders);
        //TODO find where this goes
        //keepQuads = EditorGUILayout.Toggle("Keep Quads", keepQuads);
        swapUVs = EditorGUILayout.Toggle("Swap UVs", swapUVs);
        lightmapUVs = EditorGUILayout.Toggle("Generate Lightmap UVs", lightmapUVs);
    }
}

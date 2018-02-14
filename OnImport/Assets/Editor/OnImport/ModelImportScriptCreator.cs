using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ModelImportScriptCreator
{
    public void CreateScript(string scriptName, string directoryName, ModelImportGUI gUI)
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
            outfile.WriteLine("     void OnPreprocessModel()");
            outfile.WriteLine("     {");
            outfile.WriteLine("         string lowerCaseAssetPath = assetPath.ToLower ();");
            outfile.WriteLine("         bool isInDirectory = lowerCaseAssetPath.IndexOf (\"/" + directoryName + "/\") != -1;");
            outfile.WriteLine("         ");
            outfile.WriteLine("         if (isInDirectory) ");
            outfile.WriteLine("         {");
            outfile.WriteLine("             ModelImporter modelImporter = (ModelImporter)assetImporter;");
            outfile.WriteLine(ModelGlobalScaleWriter(gUI));
            outfile.WriteLine(ModelFileScaleWriter(gUI));

            outfile.WriteLine(ModelReadableWriter(gUI));
            outfile.WriteLine(ModelOptimizeWriter(gUI));
            outfile.WriteLine(ModelBlendShapeWriter(gUI));
            outfile.WriteLine(ModelColliderWriter(gUI));
            outfile.WriteLine(ModelSwapUVWriter(gUI));
            outfile.WriteLine(ModelLightmapUVWriter(gUI));


            outfile.WriteLine(ModelImportNormalsWriter(gUI));
            outfile.WriteLine(ModelSmoothingAngleWriter(gUI));
            outfile.WriteLine(ModelTangentsWriter(gUI));


            outfile.WriteLine(ModelImportMaterialsWriter(gUI));
            outfile.WriteLine(ModelMaterialNameWriter(gUI));
            outfile.WriteLine(ModelMaterialSearchWriter(gUI));


            outfile.WriteLine(ModelAnimationTypeWriter(gUI));
            outfile.WriteLine(ModelLightmapUVWriter(gUI));

            outfile.WriteLine(ModelImportAnimationWriter(gUI));

            outfile.WriteLine("         }");
            outfile.WriteLine("     }");
            outfile.WriteLine(" }");
            //  }//File written
        }
        AssetDatabase.Refresh();
    }

    private string ModelImportNormalsWriter(ModelImportGUI gUI)
    {
        switch (gUI.importerNormals)
        {
            case ModelImporterNormals.Import:
                return ("             modelImporter.importNormals = ModelImporterNormals.Import;");
            case ModelImporterNormals.Calculate:
                return ("             modelImporter.importNormals = ModelImporterNormals.Calculate;");
            case ModelImporterNormals.None:
                return ("             modelImporter.importNormals = ModelImporterNormals.None;");
            default:
                return (" ");
        }
    }

    private string ModelSmoothingAngleWriter(ModelImportGUI gUI)
    {
        return ("             modelImporter.normalSmoothingAngle = " + gUI.smoothingAngle + "f;");
    }

    private string ModelTangentsWriter(ModelImportGUI gUI)
    {
        switch (gUI.importerTangents)
        {
            case ModelImporterTangents.Import:
                return ("             modelImporter.importTangents = ModelImporterTangents.Import;");
            case ModelImporterTangents.CalculateLegacy:
                return ("             modelImporter.importTangents = ModelImporterTangents.CalculateLegacy;");
            case ModelImporterTangents.CalculateLegacyWithSplitTangents:
                return ("             modelImporter.importTangents = ModelImporterTangents.CalculateLegacyWithSplitTangents;");
            case ModelImporterTangents.CalculateMikk:
                return ("             modelImporter.importTangents = ModelImporterTangents.CalculateMikk;");
            case ModelImporterTangents.None:
                return ("             modelImporter.importTangents = ModelImporterTangents.None;");
            default:
                return (" ");
        }
    }
    
    private string ModelImportMaterialsWriter(ModelImportGUI gUI)
    {
        if (gUI.importMaterials)
        {
            return ("             modelImporter.importMaterials = true;");
        }
        return ("             modelImporter.importMaterials = false;");
    }

    private string ModelMaterialNameWriter(ModelImportGUI gUI)
    {
        switch (gUI.materialNameMode)
        {
            case ModelImporterMaterialName.BasedOnTextureName:
                return ("             modelImporter.materialName = ModelImporterMaterialName.BasedOnTextureName;");
            case ModelImporterMaterialName.BasedOnMaterialName:
                return ("             modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;");
            case ModelImporterMaterialName.BasedOnModelNameAndMaterialName:
                return ("             modelImporter.materialName = ModelImporterMaterialName.BasedOnModelNameAndMaterialName;");
            case ModelImporterMaterialName.BasedOnTextureName_Or_ModelNameAndMaterialName:
                return ("             modelImporter.materialName = ModelImporterMaterialName.BasedOnTextureName_Or_ModelNameAndMaterialName;");
            default:
                return (" ");
        }
    }
    
    private string ModelMaterialSearchWriter(ModelImportGUI gUI)
    {
        switch (gUI.materialSearch)
        {
            case ModelImporterMaterialSearch.Local:
                return ("             modelImporter.materialSearch = ModelImporterMaterialSearch.Local;");
            case ModelImporterMaterialSearch.RecursiveUp:
                return ("             modelImporter.materialSearch = ModelImporterMaterialSearch.RecursiveUp;");
            case ModelImporterMaterialSearch.Everywhere:
                return ("             modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;");
            default:
                return (" ");
        }
    }
    
    private string ModelAnimationTypeWriter(ModelImportGUI gUI)
    {
        switch (gUI.animationType)
        {
            case ModelImporterAnimationType.None:
                return ("             modelImporter.animationType = ModelImporterAnimationType.None;");
            case ModelImporterAnimationType.Legacy:
                return ("             modelImporter.animationType = ModelImporterAnimationType.Legacy;");
            case ModelImporterAnimationType.Generic:
                return ("             modelImporter.animationType = ModelImporterAnimationType.Generic;");
            case ModelImporterAnimationType.Human:
                return ("             modelImporter.animationType = ModelImporterAnimationType.Human;");
            default:
                return (" ");
        }
    }

    private string ModelOptimizeGameObjectsWriter(ModelImportGUI gUI)
    {
        if (gUI.optimzeGameObjects)
        {
            return ("             modelImporter.optimizeGameObjects = true;");
        }
        return ("             modelImporter.optimizeGameObjects = false;");
    }

    private string ModelImportAnimationWriter(ModelImportGUI gUI)
    {
        if (gUI.importAnimation)
        {
            return ("             modelImporter.importAnimation = true;");
        }
        return ("             modelImporter.importAnimation = false;");
    }

    private string ModelGlobalScaleWriter(ModelImportGUI gUI)
    {
        return ("             modelImporter.globalScale = " + gUI.globalScale + "f;");
    }

    private string ModelFileScaleWriter(ModelImportGUI gUI)
    {
        if(gUI.useFileScale)
        {
            return ("             modelImporter.useFileUnits = true;");
        }
        return ("             modelImporter.useFileUnits = false;");
    }

    private string ModelCompressionWriter(ModelImportGUI gUI)
    {
        switch (gUI.meshCompressionType)
        {
            case ModelImporterMeshCompression.Off:
                return ("             modelImporter.meshCompression = ModelImporterMeshCompression.Off;");
            case ModelImporterMeshCompression.Low:
                return ("             modelImporter.meshCompression = ModelImporterMeshCompression.Low;");
            case ModelImporterMeshCompression.Medium:
                return ("             modelImporter.meshCompression = ModelImporterMeshCompression.Medium;");
            case ModelImporterMeshCompression.High:
                return ("             modelImporter.meshCompression = ModelImporterMeshCompression.High;");
            default:
                return (" ");
        }
    }

    private string ModelReadableWriter(ModelImportGUI gUI)
    {
        if (gUI.readWriteEnabled)
        {
            return ("             modelImporter.isReadable = true;");
        }
        return ("             modelImporter.isReadable = false;");
    }

    private string ModelOptimizeWriter(ModelImportGUI gUI)
    {
        if (gUI.optimizeMesh)
        {
            return ("             modelImporter.optimizeMesh = true;");
        }
        return ("             modelImporter.optimizeMesh = false;");
    }

    private string ModelBlendShapeWriter(ModelImportGUI gUI)
    {
        if (gUI.importBlendshapes)
        {
            return ("             modelImporter.importBlendShapes = true;");
        }
        return ("             modelImporter.importBlendShapes = false;");
    }

    private string ModelColliderWriter(ModelImportGUI gUI)
    {
        if (gUI.generateColliders)
        {
            return ("             modelImporter.addCollider = true;");
        }
        return ("             modelImporter.addCollider = false;");
    }

    private string ModelSwapUVWriter(ModelImportGUI gUI)
    {
        if (gUI.swapUVs)
        {
            return ("             modelImporter.swapUVChannels = true;");
        }
        return ("             modelImporter.swapUVChannels = false;");
    }

    private string ModelLightmapUVWriter(ModelImportGUI gUI)
    {
        if (gUI.lightmapUVs)
        {
            return ("             modelImporter.generateSecondaryUV = true;");
        }
        return ("             modelImporter.generateSecondaryUV = false;");
    }

}

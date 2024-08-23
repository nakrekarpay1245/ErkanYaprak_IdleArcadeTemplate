using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

/// <summary>
/// A custom editor window to change the Rig type of a model from Generic to Humanoid.
/// </summary>
public class RigTypeChangerWindow : EditorWindow
{
    private GameObject model;

    [MenuItem("Tools/Rig Type Changer")]
    public static void ShowWindow()
    {
        GetWindow<RigTypeChangerWindow>("Rig Type Changer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Change Rig Type to Humanoid", EditorStyles.boldLabel);

        model = (GameObject)EditorGUILayout.ObjectField("Model", model, typeof(GameObject), false);

        if (GUILayout.Button("Change Rig Type"))
        {
            if (model != null)
            {
                ChangeRigTypeToHumanoid(model);
            }
            else
            {
                Debug.LogWarning("Please assign a model to change its Rig type.");
            }
        }
    }

    /// <summary>
    /// Changes the Rig type of the specified model to Humanoid.
    /// </summary>
    /// <param name="model">The model whose Rig type is to be changed.</param>
    private void ChangeRigTypeToHumanoid(GameObject model)
    {
        string assetPath = AssetDatabase.GetAssetPath(model);
        ModelImporter modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;

        if (modelImporter == null)
        {
            Debug.LogError("The selected asset is not a model.");
            return;
        }

        // Check if the model is already set to Humanoid
        if (modelImporter.animationType == ModelImporterAnimationType.Human)
        {
            Debug.Log("The model is already set to Humanoid.");
            return;
        }

        // Set animation type to Humanoid
        modelImporter.animationType = ModelImporterAnimationType.Human;

        // Apply the changes and re-import the asset
        AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
        Debug.Log($"Successfully changed the Rig type of {model.name} to Humanoid.");
    }
}
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

/// <summary>
/// A custom editor window for converting a model's Rig type to Humanoid,
/// creating a Prefab, adding the model as a child, and adding an Animator component to the model.
/// </summary>
public class ModelSetupWindow : EditorWindow
{
    [Header("Model Settings")]
    [Tooltip("The model whose Rig type will be changed.")]
    private GameObject model;

    [Header("Prefab Settings")]
    [Tooltip("The name of the new Prefab to be created.")]
    private string prefabName = "NewPrefab";

    [Header("Save Settings")]
    [Tooltip("The directory where the Prefab will be saved.")]
    private string saveDirectory = "Assets/Prefabs";

    [MenuItem("Tools/Model Setup")]
    public static void ShowWindow()
    {
        GetWindow<ModelSetupWindow>("Model Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Setup Model", EditorStyles.boldLabel);

        model = (GameObject)EditorGUILayout.ObjectField("Model", model, typeof(GameObject), false);
        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);
        saveDirectory = EditorGUILayout.TextField("Save Directory", saveDirectory);

        if (GUILayout.Button("Create Prefab"))
        {
            if (model != null && !string.IsNullOrEmpty(prefabName) && !string.IsNullOrEmpty(saveDirectory))
            {
                CreatePrefab();
            }
            else
            {
                Debug.LogWarning("Please assign a model, provide a prefab name, and specify a save directory.");
            }
        }
    }

    /// <summary>
    /// Changes the Rig type of the model, creates a Prefab, adds the model as a child,
    /// and adds an Animator component to the model only.
    /// </summary>
    private void CreatePrefab()
    {
        // Change Rig type to Humanoid
        string modelPath = AssetDatabase.GetAssetPath(model);
        ModelImporter modelImporter = AssetImporter.GetAtPath(modelPath) as ModelImporter;

        if (modelImporter == null)
        {
            Debug.LogError("The selected asset is not a model or cannot be imported.");
            return;
        }

        if (modelImporter.animationType != ModelImporterAnimationType.Human)
        {
            modelImporter.animationType = ModelImporterAnimationType.Human;
            AssetDatabase.ImportAsset(modelPath, ImportAssetOptions.ForceUpdate);
        }

        // Create a new GameObject for the Prefab
        GameObject prefabObject = new GameObject(prefabName);

        // Instantiate the model and set as a child of prefabObject
        GameObject modelInstance = PrefabUtility.InstantiatePrefab(model) as GameObject;
        if (modelInstance == null)
        {
            Debug.LogError("Failed to instantiate the model.");
            DestroyImmediate(prefabObject);
            return;
        }
        modelInstance.transform.SetParent(prefabObject.transform);
        modelInstance.transform.localPosition = Vector3.zero; // Reset position

        // Check if Animator component already exists, if not, add it
        Animator animator = modelInstance.GetComponent<Animator>();
        if (animator == null)
        {
            animator = modelInstance.AddComponent<Animator>();
        }

        // Optionally set a specific controller
        animator.runtimeAnimatorController = null;

        // Save the Prefab
        string prefabPath = System.IO.Path.Combine(saveDirectory, prefabName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(prefabObject, prefabPath);

        // Cleanup: Destroy the temporary GameObject from the scene
        DestroyImmediate(prefabObject);

        Debug.Log($"Prefab '{prefabName}' created and saved at '{prefabPath}'.");
    }
}
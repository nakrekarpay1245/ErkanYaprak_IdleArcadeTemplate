using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

/// <summary>
/// A custom editor window for converting a model's Rig type to Humanoid,
/// creating a Prefab, adding the model as a child, and adding Animator and CharacterController components.
/// </summary>
public class ModelSetupWithControllerWindow : EditorWindow
{
    [Header("Model Settings")]
    [Tooltip("The model whose Rig type will be changed.")]
    [SerializeField] private GameObject model;

    [Header("Prefab Settings")]
    [Tooltip("The name of the new Prefab to be created.")]
    [SerializeField] private string prefabName = "NewCharacterPrefab";

    [Header("Save Settings")]
    [Tooltip("The directory where the Prefab will be saved.")]
    [SerializeField] private string saveDirectory = "Assets/Prefabs";

    [MenuItem("Tools/Character Setup")]
    public static void ShowWindow()
    {
        GetWindow<ModelSetupWithControllerWindow>("Character Setup");
    }

    private void OnGUI()
    {
        GUILayout.Label("Setup Character", EditorStyles.boldLabel);

        // Model, Prefab Name and Save Directory fields
        model = (GameObject)EditorGUILayout.ObjectField("Model", model, typeof(GameObject), false);
        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);
        saveDirectory = EditorGUILayout.TextField("Save Directory", saveDirectory);

        // Button to trigger the character setup
        if (GUILayout.Button("Create Character Prefab"))
        {
            if (model != null)
            {
                CreateCharacterPrefab();
            }
            else
            {
                Debug.LogWarning("Please assign a model to process.");
            }
        }
    }

    /// <summary>
    /// Changes the Rig type of the model to Humanoid, creates a Prefab,
    /// adds the model as a child of a new GameObject, and adds Animator and CharacterController components.
    /// </summary>
    private void CreateCharacterPrefab()
    {
        // Change Rig type to Humanoid
        string modelPath = AssetDatabase.GetAssetPath(model);
        ModelImporter modelImporter = AssetImporter.GetAtPath(modelPath) as ModelImporter;

        if (modelImporter == null)
        {
            Debug.LogError("The selected asset is not a model.");
            return;
        }

        if (modelImporter.animationType != ModelImporterAnimationType.Human)
        {
            modelImporter.animationType = ModelImporterAnimationType.Human;
            AssetDatabase.ImportAsset(modelPath, ImportAssetOptions.ForceUpdate);
        }

        // Create a new GameObject for the Prefab
        GameObject prefabObject = new GameObject(prefabName);

        // Instantiate the model as a child of the prefabObject
        GameObject modelInstance = Instantiate(model);
        modelInstance.transform.SetParent(prefabObject.transform);
        modelInstance.name = model.name; // Optionally rename the child

        // Add Animator component to the modelInstance (child)
        if (!modelInstance.TryGetComponent<Animator>(out Animator animator))
        {
            animator = modelInstance.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = null; // Optionally set a specific controller

        // Add CharacterController component to the prefabObject (parent)
        if (!prefabObject.TryGetComponent<CharacterController>(out CharacterController characterController))
        {
            characterController = prefabObject.AddComponent<CharacterController>();
        }

        // Optional: Customize the CharacterController properties
        characterController.center = Vector3.up * 1; // Adjust center as needed
        characterController.height = 2; // Adjust height as needed

        // Save the Prefab
        string prefabPath = System.IO.Path.Combine(saveDirectory, prefabName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(prefabObject, prefabPath);
        DestroyImmediate(prefabObject);

        Debug.Log($"Character Prefab '{prefabName}' created and saved at '{prefabPath}'.");
    }
}
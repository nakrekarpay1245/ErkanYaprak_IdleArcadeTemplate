using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using _Game.Scripts.TopDownCharacter;

/// <summary>
/// Custom editor window for setting up character prefabs with components based on a configuration.
/// </summary>
public class ModelSetupWithTopDownControllerWindow : EditorWindow
{
    [Header("Model Settings")]
    [Tooltip("The model whose Rig type will be changed to Humanoid.")]
    [SerializeField] private GameObject model;

    [Header("Prefab Settings")]
    [Tooltip("The name of the new Prefab to be created.")]
    [SerializeField] private string prefabName = "NewCharacterPrefab";

    [Tooltip("The directory where the Prefab will be saved.")]
    [SerializeField] private string saveDirectory = "Assets/Prefabs";

    [Header("Character Configuration")]
    [Tooltip("The character configuration ScriptableObject.")]
    [SerializeField] private TopDownCharacterConfigSO characterConfig;

    [MenuItem("Tools/Top Down Character Setup")]
    public static void ShowWindow()
    {
        GetWindow<ModelSetupWithTopDownControllerWindow>("Top Down Character Setup");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Setup Character", EditorStyles.boldLabel);

        // Draw fields for model, prefab name, save directory, and character config
        model = (GameObject)EditorGUILayout.ObjectField("Model", model, typeof(GameObject), false);
        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);
        saveDirectory = EditorGUILayout.TextField("Save Directory", saveDirectory);
        characterConfig = (TopDownCharacterConfigSO)EditorGUILayout.ObjectField("Character Config", characterConfig, typeof(TopDownCharacterConfigSO), false);

        // Button to create character prefab
        if (GUILayout.Button("Create Character Prefab"))
        {
            if (model != null && characterConfig != null)
            {
                CreateCharacterPrefab();
            }
            else
            {
                Debug.LogWarning("Please assign both a model and character configuration.");
            }
        }
    }

    /// <summary>
    /// Creates a character prefab with necessary components based on the configuration.
    /// </summary>
    private void CreateCharacterPrefab()
    {
        if (!TryChangeRigTypeToHumanoid(model))
        {
            return;
        }

        GameObject prefabObject = CreatePrefabRoot(prefabName);
        GameObject modelInstance = InstantiateModel(model, prefabObject);

        ConfigurePrefab(prefabObject, modelInstance);

        SaveAndCleanUpPrefab(prefabObject, saveDirectory, prefabName);
    }

    /// <summary>
    /// Changes the Rig type of the model to Humanoid if it's not already set.
    /// </summary>
    /// <param name="model">The model GameObject to check and modify.</param>
    /// <returns>True if the Rig type was successfully changed or already set, false otherwise.</returns>
    private bool TryChangeRigTypeToHumanoid(GameObject model)
    {
        string modelPath = AssetDatabase.GetAssetPath(model);
        ModelImporter modelImporter = AssetImporter.GetAtPath(modelPath) as ModelImporter;

        if (modelImporter == null)
        {
            Debug.LogError("Selected asset is not a valid model.");
            return false;
        }

        if (modelImporter.animationType != ModelImporterAnimationType.Human)
        {
            modelImporter.animationType = ModelImporterAnimationType.Human;
            AssetDatabase.ImportAsset(modelPath, ImportAssetOptions.ForceUpdate);
        }

        return true;
    }

    /// <summary>
    /// Creates the root object for the prefab.
    /// </summary>
    /// <param name="prefabName">The name of the prefab to create.</param>
    /// <returns>The newly created prefab root GameObject.</returns>
    private GameObject CreatePrefabRoot(string prefabName)
    {
        return new GameObject(prefabName);
    }

    /// <summary>
    /// Instantiates the model as a child of the prefab root.
    /// </summary>
    /// <param name="model">The model GameObject to instantiate.</param>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    /// <returns>The instantiated model GameObject.</returns>
    private GameObject InstantiateModel(GameObject model, GameObject prefabObject)
    {
        GameObject modelInstance = Instantiate(model);
        modelInstance.transform.SetParent(prefabObject.transform);
        modelInstance.name = model.name;
        return modelInstance;
    }

    /// <summary>
    /// Configures the prefab by adding necessary components.
    /// </summary>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    /// <param name="modelInstance">The instantiated model GameObject.</param>
    private void ConfigurePrefab(GameObject prefabObject, GameObject modelInstance)
    {
        AddCharacterController(prefabObject);
        AddAnimator(modelInstance);
        AddCustomComponents(prefabObject, modelInstance, characterConfig);
    }

    /// <summary>
    /// Adds a CharacterController component to the prefab root, ensuring it is present before setting properties.
    /// </summary>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    private void AddCharacterController(GameObject prefabObject)
    {
        CharacterController characterController = prefabObject.GetComponent<CharacterController>();
        if (characterController == null)
        {
            characterController = prefabObject.AddComponent<CharacterController>();
        }
        // Safeguard against possible null reference
        if (characterController != null)
        {
            characterController.center = Vector3.up * 1;
            characterController.height = 2;
        }
    }

    /// <summary>
    /// Adds an Animator component to the model instance.
    /// </summary>
    /// <param name="modelInstance">The instantiated model GameObject.</param>
    private void AddAnimator(GameObject modelInstance)
    {
        Animator animator = modelInstance.GetComponent<Animator>();
        if (animator == null)
        {
            animator = modelInstance.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = null; // You can set this to a specific animator controller if required
    }

    /// <summary>
    /// Adds custom components to the prefab based on the configuration.
    /// </summary>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    /// <param name="modelInstance">The instantiated model GameObject.</param>
    /// <param name="config">The character configuration scriptable object.</param>
    private void AddCustomComponents(GameObject prefabObject, GameObject modelInstance, TopDownCharacterConfigSO config)
    {
        if (config._showControllerParameters)
        {
            var controller = prefabObject.AddComponent<TopDownCharacterController>();
            controller._characterConfig = config;
        }

        if (config._showAttackParameters)
        {
            var attackHandler = prefabObject.AddComponent<TopDownCharacterAttackHandler>();
            attackHandler._characterConfig = config;
        }

        if (config._showHealthParameters)
        {
            var damageHandler = prefabObject.AddComponent<TopDownCharacterDamageHandler>();
            damageHandler._characterConfig = config;
        }

        if (config._showInteractorParameters)
        {
            var interactor = prefabObject.AddComponent<TopDownCharacterInteractor>();
            interactor._characterConfig = config;
        }

        if (config._showCollectorParameters)
        {
            var collector = prefabObject.AddComponent<TopDownCharacterCollector>();
            collector._characterConfig = config;
        }

        if (config._showAnimatorParameters)
        {
            var animator = modelInstance.AddComponent<TopDownCharacterAnimator>();
            animator._characterConfig = config;
        }
    }

    /// <summary>
    /// Saves the prefab and performs cleanup.
    /// </summary>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    /// <param name="directory">The directory where the prefab will be saved.</param>
    /// <param name="prefabName">The name of the prefab.</param>
    private void SaveAndCleanUpPrefab(GameObject prefabObject, string directory, string prefabName)
    {
        string prefabPath = System.IO.Path.Combine(directory, prefabName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(prefabObject, prefabPath);
        Debug.Log($"Character Prefab '{prefabName}' created and saved at '{prefabPath}'.");
        DestroyImmediate(prefabObject);
    }
}
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using _Game.Scripts.TopDownCharacter;
using _Game.Scripts.InputHandling;
using UnityEditor.Animations;
using System.Linq;

/// <summary>
/// Custom editor window for setting up character prefabs with components based on a configuration.
/// </summary>
public class TopDownCharacterCreator : EditorWindow
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

    [Tooltip("The player input configuration ScriptableObject.")]
    [SerializeField] private PlayerInputSO playerInputConfig;

    private const string PlayerTag = "Player";
    private const int PlayerLayer = 8; // Typically layer 8 is used for "Player". Adjust as needed.

    [MenuItem("Tools/Top Down Character Creator")]
    public static void ShowWindow()
    {
        GetWindow<TopDownCharacterCreator>("Top Down Character Creator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Setup Character", EditorStyles.boldLabel);

        // Draw fields for model, prefab name, save directory, and character config
        model = (GameObject)EditorGUILayout.ObjectField("Model", model, typeof(GameObject), false);
        prefabName = EditorGUILayout.TextField("Prefab Name", prefabName);
        saveDirectory = EditorGUILayout.TextField("Save Directory", saveDirectory);
        characterConfig = (TopDownCharacterConfigSO)EditorGUILayout.ObjectField("Character Config", characterConfig, typeof(TopDownCharacterConfigSO), false);
        playerInputConfig = (PlayerInputSO)EditorGUILayout.ObjectField("Player Input Config", playerInputConfig, typeof(PlayerInputSO), false);

        // Button to create character prefab
        if (GUILayout.Button("Create Character Prefab"))
        {
            if (model != null && characterConfig != null && playerInputConfig != null)
            {
                CreateCharacterPrefab();
            }
            else
            {
                Debug.LogWarning("Please assign all required fields.");
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

        SetTagAndLayer(prefabObject, PlayerTag, PlayerLayer);
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
    /// Instantiates the model as a child of the prefab root and sets its position.
    /// </summary>
    /// <param name="model">The model GameObject to instantiate.</param>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    /// <returns>The instantiated model GameObject.</returns>
    private GameObject InstantiateModel(GameObject model, GameObject prefabObject)
    {
        GameObject modelInstance = Instantiate(model);
        modelInstance.transform.SetParent(prefabObject.transform);

        // Set the position of the model instance
        modelInstance.transform.localPosition = new Vector3(0, -1, 0);
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
        AddCustomComponents(prefabObject, modelInstance, characterConfig, playerInputConfig);
    }

    /// <summary>
    /// Adds a CharacterController component to the prefab root if it does not already exist.
    /// </summary>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    private void AddCharacterController(GameObject prefabObject)
    {
        var characterController = prefabObject.GetComponent<CharacterController>();
        if (characterController == null)
        {
            characterController = prefabObject.AddComponent<CharacterController>();
            characterController.center = Vector3.zero;
            characterController.height = 2;
        }
    }

    /// <summary>
    /// Adds an Animator component to the model instance if it does not already exist.
    /// </summary>
    /// <param name="modelInstance">The instantiated model GameObject.</param>
    private void AddAnimator(GameObject modelInstance)
    {
        var animator = modelInstance.GetComponent<Animator>();
        if (animator == null)
        {
            animator = modelInstance.AddComponent<Animator>();
        }
        animator.runtimeAnimatorController = null; // Set this to a specific animator controller if needed
    }

    /// <summary>
    /// Adds custom components to the prefab based on the configuration.
    /// </summary>
    /// <param name="prefabObject">The prefab root GameObject.</param>
    /// <param name="modelInstance">The instantiated model GameObject.</param>
    /// <param name="config">The character configuration ScriptableObject.</param>
    /// <param name="playerInputConfig">The player input configuration ScriptableObject.</param>
    private void AddCustomComponents(GameObject prefabObject, GameObject modelInstance, TopDownCharacterConfigSO config, PlayerInputSO playerInputConfig)
    {
        if (config._showControllerParameters)
        {
            var controller = prefabObject.AddComponent<TopDownCharacterController>();
            controller.CharacterConfig = config;
            controller.PlayerInput = playerInputConfig;
        }

        if (config._showAttackParameters)
        {
            var attackHandler = prefabObject.AddComponent<TopDownCharacterAttackHandler>();
            attackHandler.CharacterConfig = config;
        }

        if (config._showHealthParameters)
        {
            var damageHandler = prefabObject.AddComponent<TopDownCharacterDamageHandler>();
            damageHandler.CharacterConfig = config;
        }

        if (config._showInteractorParameters)
        {
            var interactor = prefabObject.AddComponent<TopDownCharacterInteractor>();
            interactor.CharacterConfig = config;
        }

        if (config._showCollectorParameters)
        {
            var collector = prefabObject.AddComponent<TopDownCharacterCollector>();
            collector.CharacterConfig = config;
        }

        if (config._showAnimatorParameters)
        {
            var animator = modelInstance.AddComponent<TopDownCharacterAnimator>();
            animator.CharacterConfig = config;

            var animatorComponent = modelInstance.GetComponent<Animator>();
            if (animatorComponent == null)
            {
                animatorComponent = modelInstance.AddComponent<Animator>();
            }

            animatorComponent.applyRootMotion = false;
            animatorComponent.updateMode = AnimatorUpdateMode.Normal;
            animatorComponent.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            // Assign the AnimatorController
            string controllerPath = "Assets/_Game/Animations/MaleCharacter/MaleCharacter.controller";
            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if (controller != null)
            {
                animatorComponent.runtimeAnimatorController = controller;
            }
            else
            {
                Debug.LogError($"AnimatorController not found at path: {controllerPath}");
            }
        }
    }

    /// <summary>
    /// Sets the tag and layer of the GameObject. If the tag or layer does not exist, creates them.
    /// </summary>
    /// <param name="gameObject">The GameObject to set tag and layer for.</param>
    /// <param name="tag">The tag to be applied.</param>
    /// <param name="layer">The layer index to be applied.</param>
    private void SetTagAndLayer(GameObject gameObject, string tag, int layer)
    {
        if (!UnityEditorInternal.InternalEditorUtility.tags.Contains(tag))
        {
            AddTag(tag);
        }

        if (!UnityEditorInternal.InternalEditorUtility.layers.Contains(LayerMask.LayerToName(layer)))
        {
            AddLayer(layer);
        }

        gameObject.tag = tag;
        gameObject.layer = layer;
    }

    /// <summary>
    /// Adds a new tag to the project.
    /// </summary>
    /// <param name="newTag">The tag name to be added.</param>
    private void AddTag(string newTag)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag)) return;
        }

        tagsProp.InsertArrayElementAtIndex(0);
        SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
        newTagProp.stringValue = newTag;

        tagManager.ApplyModifiedProperties();
    }

    /// <summary>
    /// Adds a new layer to the project.
    /// </summary>
    /// <param name="layerIndex">The layer index to add.</param>
    private void AddLayer(int layerIndex)
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layersProp = tagManager.FindProperty("layers");

        if (layersProp.GetArrayElementAtIndex(layerIndex).stringValue == "")
        {
            layersProp.GetArrayElementAtIndex(layerIndex).stringValue = "Player";
            tagManager.ApplyModifiedProperties();
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
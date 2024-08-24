using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Custom editor window for creating multiple TopDownCharacterConfigSO ScriptableObjects with customizable names and parameters.
/// </summary>
public class TopDownCharacterConfigCreator : EditorWindow
{
    // File name for the new ScriptableObject
    private string _assetName = "NewTopDownCharacterConfig";

    // Visibility toggles for creating the ScriptableObject
    private bool _useControllerParameters = true;
    private bool _useCollectorParameters = true;
    private bool _useAnimatorParameters = true;
    private bool _useAttackParameters = true;
    private bool _useHealthParameters = true;
    private bool _useInteractorParameters = true;

    // Controller Parameters
    [Header("Controller Parameters")]
    [Tooltip("Movement speed of the character.")]
    [SerializeField] private float _movementSpeed = 5f;
    [Tooltip("Acceleration rate of the character.")]
    [SerializeField] private float _acceleration = 2f;
    [Tooltip("Threshold for rotation speed.")]
    [SerializeField] private float _rotationSpeedThreshold = 0.1f;
    [Tooltip("Rotation speed of the character.")]
    [SerializeField] private float _rotationSpeed = 720;
    [Tooltip("Gravity applied to the character.")]
    [SerializeField] private float _gravity = -9.81f;

    // Collector Parameters
    [Header("Collector Parameters")]
    [Tooltip("Radius within which items can be collected.")]
    [SerializeField] private float _collectRadius = 2f;
    [Tooltip("Whether to stop movement on special collection.")]
    [SerializeField] private bool _stopMovementOnSpecialCollect = true;
    [Tooltip("Duration of stopping movement on special collection.")]
    [SerializeField] private float _stopDurationOnSpecialCollect = 3f;

    // Animator Parameters
    [Header("Animator Parameters")]
    [Tooltip("Animator parameter key for speed.")]
    [SerializeField] private string _speedAnimatorParameterKey = "Speed";
    [Tooltip("Animator parameter key for is hurt.")]
    [SerializeField] private string _isHurtAnimatorParameterKey = "IsHurt";
    [Tooltip("Animator parameter key for is dead.")]
    [SerializeField] private string _isDeadAnimatorParameterKey = "IsDead";
    [Tooltip("Animator parameter key for is attack.")]
    [SerializeField] private string _isAttackAnimatorParameterKey = "IsAttack";
    [Tooltip("Animator parameter key for is win.")]
    [SerializeField] private string _isWinAnimatorParameterKey = "IsWin";

    // Attack Parameters
    [Header("Attack Parameters")]
    [Tooltip("Amount of damage dealt by attacks.")]
    [SerializeField] private float _damageAmount = 1f;
    [Tooltip("Range of the attack.")]
    [SerializeField] private float _attackRange = 1f;
    [Tooltip("Radius within which enemies can be detected.")]
    [SerializeField] private float _detectionRadius = 1.5f;
    [Tooltip("Delay before the attack starts.")]
    [SerializeField] private float _attackDelay = 1f;
    [Tooltip("Duration of the attack.")]
    [SerializeField] private float _attackDuration = 0.1f;
    [Tooltip("Offset before the attack.")]
    [SerializeField] private float _attackOffset = 0.5f;
    [Tooltip("Layer mask for damageable targets.")]
    [SerializeField] private LayerMask _damageableLayerMask;
    [Tooltip("Interval between attacks.")]
    [SerializeField] private float _attackInterval = 2f;
    [Tooltip("Minimum movement speed required for an attack.")]
    [SerializeField] private float _minimumMovementSpeedForAttack = 0.25f;
    [Tooltip("Rotation speed for facing the nearest target.")]
    [SerializeField] private float _rotationSpeedForFaceToNearestTarget = 25f;

    // Health Parameters
    [Header("Health Parameters")]
    [Tooltip("Maximum health of the character.")]
    [SerializeField] private float _maxHealth = 5f;
    [Tooltip("Duration of stopping on taking damage.")]
    [SerializeField] private float _stopDurationOnDamage = 2f;
    [Tooltip("Time to wait before the character dies.")]
    [SerializeField] private float _deathWaitTime = 2f;

    // Interactor Parameters
    [Header("Interactor Parameters")]
    [Tooltip("Radius within which interactions can occur.")]
    [SerializeField] private float _interactionRadius = 2f;
    [Tooltip("Layer mask for interactable objects.")]
    [SerializeField] private LayerMask _interactableLayerMask;

    [MenuItem("Tools/TopDownCharacterConfigCreator")]
    public static void ShowWindow()
    {
        GetWindow<TopDownCharacterConfigCreator>("TopDownCharacterConfigCreator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create TopDownCharacterConfigSO", EditorStyles.boldLabel);

        // Input field for asset name
        _assetName = EditorGUILayout.TextField("Asset Name", _assetName);

        // Toggle fields for each category
        _useControllerParameters = EditorGUILayout.Toggle("Use Controller Parameters", _useControllerParameters);
        _useCollectorParameters = EditorGUILayout.Toggle("Use Collector Parameters", _useCollectorParameters);
        _useAnimatorParameters = EditorGUILayout.Toggle("Use Animator Parameters", _useAnimatorParameters);
        _useAttackParameters = EditorGUILayout.Toggle("Use Attack Parameters", _useAttackParameters);
        _useHealthParameters = EditorGUILayout.Toggle("Use Health Parameters", _useHealthParameters);
        _useInteractorParameters = EditorGUILayout.Toggle("Use Interactor Parameters", _useInteractorParameters);

        // Draw fields for each category based on their toggles
        if (_useControllerParameters)
        {
            DrawControllerParameters();
        }

        if (_useCollectorParameters)
        {
            DrawCollectorParameters();
        }

        if (_useAnimatorParameters)
        {
            DrawAnimatorParameters();
        }

        if (_useAttackParameters)
        {
            DrawAttackParameters();
        }

        if (_useHealthParameters)
        {
            DrawHealthParameters();
        }

        if (_useInteractorParameters)
        {
            DrawInteractorParameters();
        }

        // Create button to generate the ScriptableObject
        if (GUILayout.Button("Create"))
        {
            CreateTopDownCharacterConfig();
        }
    }

    private void DrawControllerParameters()
    {
        GUILayout.Label("Controller Parameters", EditorStyles.boldLabel);
        _movementSpeed = EditorGUILayout.FloatField("Movement Speed", _movementSpeed);
        _acceleration = EditorGUILayout.FloatField("Acceleration", _acceleration);
        _rotationSpeedThreshold = EditorGUILayout.FloatField("Rotation Speed Threshold", _rotationSpeedThreshold);
        _rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", _rotationSpeed);
        _gravity = EditorGUILayout.FloatField("Gravity", _gravity);
    }

    private void DrawCollectorParameters()
    {
        GUILayout.Label("Collector Parameters", EditorStyles.boldLabel);
        _collectRadius = EditorGUILayout.FloatField("Collect Radius", _collectRadius);
        _stopMovementOnSpecialCollect = EditorGUILayout.Toggle("Stop Movement On Special Collect", _stopMovementOnSpecialCollect);
        _stopDurationOnSpecialCollect = EditorGUILayout.FloatField("Stop Duration On Special Collect", _stopDurationOnSpecialCollect);
    }

    private void DrawAnimatorParameters()
    {
        GUILayout.Label("Animator Parameters", EditorStyles.boldLabel);
        _speedAnimatorParameterKey = EditorGUILayout.TextField("Speed Animator Parameter Key", _speedAnimatorParameterKey);
        _isHurtAnimatorParameterKey = EditorGUILayout.TextField("Is Hurt Animator Parameter Key", _isHurtAnimatorParameterKey);
        _isDeadAnimatorParameterKey = EditorGUILayout.TextField("Is Dead Animator Parameter Key", _isDeadAnimatorParameterKey);
        _isAttackAnimatorParameterKey = EditorGUILayout.TextField("Is Attack Animator Parameter Key", _isAttackAnimatorParameterKey);
        _isWinAnimatorParameterKey = EditorGUILayout.TextField("Is Win Animator Parameter Key", _isWinAnimatorParameterKey);
    }

    private void DrawAttackParameters()
    {
        GUILayout.Label("Attack Parameters", EditorStyles.boldLabel);
        _damageAmount = EditorGUILayout.FloatField("Damage Amount", _damageAmount);
        _attackRange = EditorGUILayout.FloatField("Attack Range", _attackRange);
        _detectionRadius = EditorGUILayout.FloatField("Detection Radius", _detectionRadius);
        _attackDelay = EditorGUILayout.FloatField("Attack Delay", _attackDelay);
        _attackDuration = EditorGUILayout.FloatField("Attack Duration", _attackDuration);
        _attackOffset = EditorGUILayout.FloatField("Attack Offset", _attackOffset);
        _damageableLayerMask = EditorGUILayout.LayerField("Damageable Layer Mask", _damageableLayerMask);
        _attackInterval = EditorGUILayout.FloatField("Attack Interval", _attackInterval);
        _minimumMovementSpeedForAttack = EditorGUILayout.FloatField("Minimum Movement Speed For Attack", _minimumMovementSpeedForAttack);
        _rotationSpeedForFaceToNearestTarget = EditorGUILayout.FloatField("Rotation Speed For Facing Target", _rotationSpeedForFaceToNearestTarget);
    }

    private void DrawHealthParameters()
    {
        GUILayout.Label("Health Parameters", EditorStyles.boldLabel);
        _maxHealth = EditorGUILayout.FloatField("Max Health", _maxHealth);
        _stopDurationOnDamage = EditorGUILayout.FloatField("Stop Duration On Damage", _stopDurationOnDamage);
        _deathWaitTime = EditorGUILayout.FloatField("Death Wait Time", _deathWaitTime);
    }

    private void DrawInteractorParameters()
    {
        GUILayout.Label("Interactor Parameters", EditorStyles.boldLabel);
        _interactionRadius = EditorGUILayout.FloatField("Interaction Radius", _interactionRadius);
        _interactableLayerMask = EditorGUILayout.LayerField("Interactable Layer Mask", _interactableLayerMask);
    }

    private void CreateTopDownCharacterConfig()
    {
        // Ensure the asset name is valid
        if (string.IsNullOrWhiteSpace(_assetName))
        {
            EditorUtility.DisplayDialog("Error", "Asset name cannot be empty.", "OK");
            return;
        }

        // Create the ScriptableObject
        TopDownCharacterConfigSO config = ScriptableObject.CreateInstance<TopDownCharacterConfigSO>();

        // Assign values to the ScriptableObject based on toggles
        config.SetParameters(
            _useControllerParameters, _movementSpeed, _acceleration, _rotationSpeedThreshold, _rotationSpeed, _gravity,
            _useCollectorParameters, _collectRadius, _stopMovementOnSpecialCollect, _stopDurationOnSpecialCollect,
            _useAnimatorParameters, _speedAnimatorParameterKey, _isHurtAnimatorParameterKey, _isDeadAnimatorParameterKey, _isAttackAnimatorParameterKey, _isWinAnimatorParameterKey,
            _useAttackParameters, _damageAmount, _attackRange, _detectionRadius, _attackDelay, _attackDuration, _attackOffset, _damageableLayerMask, _attackInterval, _minimumMovementSpeedForAttack, _rotationSpeedForFaceToNearestTarget,
            _useHealthParameters, _maxHealth, _stopDurationOnDamage, _deathWaitTime,
            _useInteractorParameters, _interactionRadius, _interactableLayerMask
        );

        // Save the ScriptableObject in the Assets folder with the specified name
        string path = $"Assets/_Game/_Data/Player/{_assetName}.asset";
        AssetDatabase.CreateAsset(config, path);
        AssetDatabase.SaveAssets();

        // Ensure "Player" tag and layer exist
        EnsureTagAndLayerExist("Player", "Player");

        // Assign the tag and layer to the created asset
        GameObject tempGO = new GameObject(); // Create a temporary GameObject to apply the tag and layer
        tempGO.tag = "Player";
        tempGO.layer = LayerMask.NameToLayer("Player");
        EditorUtility.SetDirty(config); // Mark the asset as dirty so changes are saved

        DestroyImmediate(tempGO); // Clean up the temporary GameObject

        // Focus on the newly created asset
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = config;

        Debug.Log($"TopDownCharacterConfigSO created and saved at {path} with tag and layer set to 'Player'.");
    }

    private void EnsureTagAndLayerExist(string tagName, string layerName)
    {
        // Ensure the tag exists
        if (!UnityEditorInternal.InternalEditorUtility.tags.Contains(tagName))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Insert new tag if it doesn't exist
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue == "")
                {
                    t.stringValue = tagName;
                    tagManager.ApplyModifiedProperties();
                    break;
                }
            }
        }

        // Ensure the layer exists
        if (!UnityEditorInternal.InternalEditorUtility.layers.Contains(layerName))
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = tagManager.FindProperty("layers");

            // Insert new layer if it doesn't exist
            for (int i = 8; i < layersProp.arraySize; i++) // Unity layers 0-7 are reserved
            {
                SerializedProperty l = layersProp.GetArrayElementAtIndex(i);
                if (l.stringValue == "")
                {
                    l.stringValue = layerName;
                    tagManager.ApplyModifiedProperties();
                    break;
                }
            }
        }
    }
}
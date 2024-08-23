using UnityEditor;
using UnityEngine;

public class TopDownCharacterConfigSOEditorWindow : EditorWindow
{
    // Visibility toggles for creating the scriptable object
    private bool _useControllerParameters = true;
    private bool _useCollectorParameters = true;
    private bool _useAnimatorParameters = true;
    private bool _useAttackParameters = true;
    private bool _useHealthParameters = true;
    private bool _useInteractorParameters = true;

    // Controller Parameters
    private float _movementSpeed = 5f;
    private float _acceleration = 2f;
    private float _rotationSpeedThreshold = 0.1f;
    private float _rotationSpeed = 720;
    private float _gravity = -9.81f;

    // Collector Parameters
    private float _collectRadius = 2f;
    private bool _stopMovementOnSpecialCollect = true;
    private float _stopDurationOnSpecialCollect = 3f;

    // Animator Parameters
    private string _speedAnimatorParameterKey = "Speed";
    private string _isHurtAnimatorParameterKey = "IsHurt";
    private string _isDeadAnimatorParameterKey = "IsDead";
    private string _isAttackAnimatorParameterKey = "IsAttack";
    private string _isWinAnimatorParameterKey = "IsWin";

    // Attack Parameters
    private float _damageAmount = 1f;
    private float _attackRange = 1f;
    private float _detectionRadius = 1.5f;
    private float _attackDelay = 1f;
    private float _attackDuration = 0.1f;
    private float _attackOffset = 0.5f;
    private LayerMask _damageableLayerMask;
    private float _attackInterval = 2f;
    private float _minimumMovementSpeedForAttack = 0.25f;
    private float _rotationSpeedForFaceToNearestTarget = 25f;

    // Health Parameters
    private float _maxHealth = 5f;
    private float _stopDurationOnDamage = 2f;
    private float _deathWaitTime = 2f;

    // Interactor Parameters
    private float _interactionRadius = 2f;
    private LayerMask _interactableLayerMask;

    [MenuItem("Tools/TopDownCharacterConfigSO Creator")]
    public static void ShowWindow()
    {
        GetWindow<TopDownCharacterConfigSOEditorWindow>("Character Config Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create TopDownCharacterConfigSO", EditorStyles.boldLabel);

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
            GUILayout.Label("Controller Parameters", EditorStyles.boldLabel);
            _movementSpeed = EditorGUILayout.FloatField("Movement Speed", _movementSpeed);
            _acceleration = EditorGUILayout.FloatField("Acceleration", _acceleration);
            _rotationSpeedThreshold = EditorGUILayout.FloatField("Rotation Speed Threshold", _rotationSpeedThreshold);
            _rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", _rotationSpeed);
            _gravity = EditorGUILayout.FloatField("Gravity", _gravity);
        }

        if (_useCollectorParameters)
        {
            GUILayout.Label("Collector Parameters", EditorStyles.boldLabel);
            _collectRadius = EditorGUILayout.FloatField("Collect Radius", _collectRadius);
            _stopMovementOnSpecialCollect = EditorGUILayout.Toggle("Stop Movement On Special Collect", _stopMovementOnSpecialCollect);
            _stopDurationOnSpecialCollect = EditorGUILayout.FloatField("Stop Duration On Special Collect", _stopDurationOnSpecialCollect);
        }

        if (_useAnimatorParameters)
        {
            GUILayout.Label("Animator Parameters", EditorStyles.boldLabel);
            _speedAnimatorParameterKey = EditorGUILayout.TextField("Speed Animator Parameter Key", _speedAnimatorParameterKey);
            _isHurtAnimatorParameterKey = EditorGUILayout.TextField("Is Hurt Animator Parameter Key", _isHurtAnimatorParameterKey);
            _isDeadAnimatorParameterKey = EditorGUILayout.TextField("Is Dead Animator Parameter Key", _isDeadAnimatorParameterKey);
            _isAttackAnimatorParameterKey = EditorGUILayout.TextField("Is Attack Animator Parameter Key", _isAttackAnimatorParameterKey);
            _isWinAnimatorParameterKey = EditorGUILayout.TextField("Is Win Animator Parameter Key", _isWinAnimatorParameterKey);
        }

        if (_useAttackParameters)
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

        if (_useHealthParameters)
        {
            GUILayout.Label("Health Parameters", EditorStyles.boldLabel);
            _maxHealth = EditorGUILayout.FloatField("Max Health", _maxHealth);
            _stopDurationOnDamage = EditorGUILayout.FloatField("Stop Duration On Damage", _stopDurationOnDamage);
            _deathWaitTime = EditorGUILayout.FloatField("Death Wait Time", _deathWaitTime);
        }

        if (_useInteractorParameters)
        {
            GUILayout.Label("Interactor Parameters", EditorStyles.boldLabel);
            _interactionRadius = EditorGUILayout.FloatField("Interaction Radius", _interactionRadius);
            _interactableLayerMask = EditorGUILayout.LayerField("Interactable Layer Mask", _interactableLayerMask);
        }

        // Create button to generate the ScriptableObject
        if (GUILayout.Button("Create"))
        {
            CreateTopDownCharacterConfig();
        }
    }

    private void CreateTopDownCharacterConfig()
    {
        TopDownCharacterConfigSO config = ScriptableObject.CreateInstance<TopDownCharacterConfigSO>();

        // Assign values to the scriptable object based on toggles
        config._showControllerParameters = _useControllerParameters;
        config._showCollectorParameters = _useCollectorParameters;
        config._showAnimatorParameters = _useAnimatorParameters;
        config._showAttackParameters = _useAttackParameters;
        config._showHealthParameters = _useHealthParameters;
        config._showInteractorParameters = _useInteractorParameters;

        if (_useControllerParameters)
        {
            config.GetType().GetField("_movementSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _movementSpeed);
            config.GetType().GetField("_acceleration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _acceleration);
            config.GetType().GetField("_rotationSpeedThreshold", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _rotationSpeedThreshold);
            config.GetType().GetField("_rotationSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _rotationSpeed);
            config.GetType().GetField("_gravity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _gravity);
        }

        if (_useCollectorParameters)
        {
            config.GetType().GetField("_collectRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _collectRadius);
            config.GetType().GetField("_stopMovementOnSpecialCollect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _stopMovementOnSpecialCollect);
            config.GetType().GetField("_stopDurationOnSpecialCollect", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _stopDurationOnSpecialCollect);
        }

        if (_useAnimatorParameters)
        {
            config.GetType().GetField("_speedAnimatorParameterKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _speedAnimatorParameterKey);
            config.GetType().GetField("_isHurtAnimatorParameterKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _isHurtAnimatorParameterKey);
            config.GetType().GetField("_isDeadAnimatorParameterKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _isDeadAnimatorParameterKey);
            config.GetType().GetField("_isAttackAnimatorParameterKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _isAttackAnimatorParameterKey);
            config.GetType().GetField("_isWinAnimatorParameterKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _isWinAnimatorParameterKey);
        }

        if (_useAttackParameters)
        {
            config.GetType().GetField("_damageAmount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _damageAmount);
            config.GetType().GetField("_attackRange", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _attackRange);
            config.GetType().GetField("_detectionRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _detectionRadius);
            config.GetType().GetField("_attackDelay", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _attackDelay);
            config.GetType().GetField("_attackDuration", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _attackDuration);
            config.GetType().GetField("_attackOffset", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _attackOffset);
            config.GetType().GetField("_damageableLayerMask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _damageableLayerMask);
            config.GetType().GetField("_attackInterval", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _attackInterval);
            config.GetType().GetField("_minimumMovementSpeedForAttack", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _minimumMovementSpeedForAttack);
            config.GetType().GetField("_rotationSpeedForFaceToNearestTarget", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _rotationSpeedForFaceToNearestTarget);
        }

        if (_useHealthParameters)
        {
            config.GetType().GetField("_maxHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _maxHealth);
            config.GetType().GetField("_stopDurationOnDamage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _stopDurationOnDamage);
            config.GetType().GetField("_deathWaitTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _deathWaitTime);
        }

        if (_useInteractorParameters)
        {
            config.GetType().GetField("_interactionRadius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _interactionRadius);
            config.GetType().GetField("_interactableLayerMask", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(config, _interactableLayerMask);
        }

        // Save the scriptable object in the Assets folder
        AssetDatabase.CreateAsset(config, "Assets/_Game/_Data/Player/TopDownCharacterData.asset");
        AssetDatabase.SaveAssets();

        // Focus on the newly created asset
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = config;

        Debug.Log("TopDownCharacterConfigSO created and saved at Assets/_Game/_Data/Player/");
    }
}
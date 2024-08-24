using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TopDownCharacterConfigSO))]
public class TopDownCharacterConfigSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the reference to the ScriptableObject
        TopDownCharacterConfigSO config = (TopDownCharacterConfigSO)target;

        // Draw default inspector for the visibility toggles
        DrawVisibilityToggles(config);

        // Conditional display based on visibility toggles
        if (config._showControllerParameters)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Controller Parameters", EditorStyles.boldLabel);
            DrawControllerParameters(config);
        }

        if (config._showCollectorParameters)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Collector Parameters", EditorStyles.boldLabel);
            DrawCollectorParameters(config);
        }

        if (config._showAnimatorParameters)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Animator Parameters", EditorStyles.boldLabel);
            DrawAnimatorParameters(config);
        }

        if (config._showAttackParameters)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Attack Parameters", EditorStyles.boldLabel);
            DrawAttackParameters(config);
        }

        if (config._showHealthParameters)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Health Parameters", EditorStyles.boldLabel);
            DrawHealthParameters(config);
        }

        if (config._showInteractorParameters)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Interactor Parameters", EditorStyles.boldLabel);
            DrawInteractorParameters(config);
        }

        // Apply changes to the serializedObject
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawVisibilityToggles(TopDownCharacterConfigSO config)
    {
        config._showControllerParameters = EditorGUILayout.Toggle("Show Controller Parameters", config._showControllerParameters);
        config._showCollectorParameters = EditorGUILayout.Toggle("Show Collector Parameters", config._showCollectorParameters);
        config._showAnimatorParameters = EditorGUILayout.Toggle("Show Animator Parameters", config._showAnimatorParameters);
        config._showAttackParameters = EditorGUILayout.Toggle("Show Attack Parameters", config._showAttackParameters);
        config._showHealthParameters = EditorGUILayout.Toggle("Show Health Parameters", config._showHealthParameters);
        config._showInteractorParameters = EditorGUILayout.Toggle("Show Interactor Parameters", config._showInteractorParameters);
    }

    private void DrawControllerParameters(TopDownCharacterConfigSO config)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_movementSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_acceleration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_rotationSpeedThreshold"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_rotationSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_gravity"));
    }

    private void DrawCollectorParameters(TopDownCharacterConfigSO config)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_collectRadius"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_stopMovementOnSpecialCollect"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_stopDurationOnSpecialCollect"));
    }

    private void DrawAnimatorParameters(TopDownCharacterConfigSO config)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_speedAnimatorParameterKey"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_isHurtAnimatorParameterKey"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_isDeadAnimatorParameterKey"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_isAttackAnimatorParameterKey"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_isWinAnimatorParameterKey"));
    }

    private void DrawAttackParameters(TopDownCharacterConfigSO config)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_damageAmount"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_detectionRadius"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackDelay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackOffset"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_damageableLayerMask"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackInterval"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_minimumMovementSpeedForAttack"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_rotationSpeedForFaceToNearestTarget"));
    }

    private void DrawHealthParameters(TopDownCharacterConfigSO config)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_maxHealth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_stopDurationOnDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_deathWaitTime"));
    }

    private void DrawInteractorParameters(TopDownCharacterConfigSO config)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_interactionRadius"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_interactableLayerMask"));
    }
}
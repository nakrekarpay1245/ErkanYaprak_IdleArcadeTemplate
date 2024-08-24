using UnityEngine;

/// <summary>
/// ScriptableObject to store the character's configuration, 
/// including movement, attack, health, collection, and interaction parameters.
/// This class follows SOLID principles and focuses on performance, readability,
/// and encapsulation.
/// </summary>
[CreateAssetMenu(fileName = "TopDownCharacterData", menuName = "Data/TopDownCharacterData")]
public class TopDownCharacterConfigSO : ScriptableObject
{
    // Visibility toggles for different categories
    [Header("Visibility Toggles")]
    [Tooltip("Show/Hide Controller Parameters")]
    public bool _showControllerParameters = true;

    [Tooltip("Show/Hide Collector Parameters")]
    public bool _showCollectorParameters = true;

    [Tooltip("Show/Hide Animator Parameters")]
    public bool _showAnimatorParameters = true;

    [Tooltip("Show/Hide Attack Parameters")]
    public bool _showAttackParameters = true;

    [Tooltip("Show/Hide Health Parameters")]
    public bool _showHealthParameters = true;

    [Tooltip("Show/Hide Interactor Parameters")]
    public bool _showInteractorParameters = true;

    // Controller Parameters
    [Header("Controller Parameters")]
    [Tooltip("Movement speed of the character.")]
    [SerializeField] private float _movementSpeed = 5f;

    [Tooltip("Acceleration of the character's movement.")]
    [SerializeField] private float _acceleration = 2f;

    [Tooltip("Threshold for rotation speed before character starts rotating.")]
    [SerializeField] private float _rotationSpeedThreshold = 0.1f;

    [Tooltip("Rotation speed of the character.")]
    [SerializeField] private float _rotationSpeed = 720;

    [Tooltip("Gravity affecting the character.")]
    [SerializeField] private float _gravity = -9.81f;

    // Collector Parameters
    [Header("Collector Parameters")]
    [Tooltip("Radius for detecting collectibles.")]
    [SerializeField] private float _collectRadius = 2;

    [Tooltip("Should the character stop moving when a special item is collected.")]
    [SerializeField] private bool _stopMovementOnSpecialCollect = true;

    [Tooltip("Duration to stop movement when a special item is collected.")]
    [SerializeField] private float _stopDurationOnSpecialCollect = 3;

    // Animator Parameters
    [Header("Animator Parameters")]
    [Tooltip("Key for the speed parameter in the animator.")]
    [SerializeField] private string _speedAnimatorParameterKey = "Speed";

    [Tooltip("Key for the hurt state in the animator.")]
    [SerializeField] private string _isHurtAnimatorParameterKey = "IsHurt";

    [Tooltip("Key for the dead state in the animator.")]
    [SerializeField] private string _isDeadAnimatorParameterKey = "IsDead";

    [Tooltip("Key for the attack state in the animator.")]
    [SerializeField] private string _isAttackAnimatorParameterKey = "IsAttack";

    [Tooltip("Key for the win state in the animator.")]
    [SerializeField] private string _isWinAnimatorParameterKey = "IsWin";

    // Attack Parameters
    [Header("Attack Parameters")]
    [Tooltip("Damage amount dealt by the character.")]
    [SerializeField] private float _damageAmount = 1f;

    [Tooltip("Range within which the character can attack.")]
    [SerializeField] private float _attackRange = 1f;

    [Tooltip("Detection radius for nearby enemies.")]
    [SerializeField] private float _detectionRadius = 1.5f;

    [Tooltip("Delay before an attack begins.")]
    [SerializeField] private float _attackDelay = 1f;

    [Tooltip("Duration of the attack animation.")]
    [SerializeField] private float _attackDuration = 0.1f;

    [Tooltip("Offset for the attack hitbox.")]
    [SerializeField] private float _attackOffset = 0.5f;

    [Tooltip("Layers that can be damaged.")]
    [SerializeField] private LayerMask _damageableLayerMask;

    [Tooltip("Interval between consecutive attacks.")]
    [SerializeField] private float _attackInterval = 2f;

    [Tooltip("Minimum movement speed required for an attack.")]
    [SerializeField] private float _minimumMovementSpeedForAttack = 0.25f;

    [Tooltip("Rotation speed when facing the nearest target.")]
    [SerializeField] private float _rotationSpeedForFaceToNearestTarget = 25f;

    // Health Parameters
    [Header("Health Parameters")]
    [Tooltip("Maximum health of the character.")]
    [SerializeField] private float _maxHealth = 5;

    [Tooltip("Duration to stop movement when the character is damaged.")]
    [SerializeField] private float _stopDurationOnDamage = 2f;

    [Tooltip("Time to wait after death before transitioning.")]
    [SerializeField] private float _deathWaitTime = 2f;

    // Interactor Parameters
    [Header("Interactor Parameters")]
    [Tooltip("Interaction radius for detecting interactable objects.")]
    [SerializeField] private float _interactionRadius = 2f;

    [Tooltip("Layers that are interactable.")]
    [SerializeField] private LayerMask _interactableLayerMask;

    // Encapsulation - Public Getters for each parameter

    // Controller Parameters
    public float MovementSpeed => _movementSpeed;
    public float Acceleration => _acceleration;
    public float RotationSpeedThreshold => _rotationSpeedThreshold;
    public float RotationSpeed => _rotationSpeed;
    public float Gravity => _gravity;

    // Collector Parameters
    public float CollectRadius => _collectRadius;
    public bool StopMovementOnSpecialCollect => _stopMovementOnSpecialCollect;
    public float StopDurationOnSpecialCollect => _stopDurationOnSpecialCollect;

    // Animator Parameters
    public string SpeedAnimatorParameterKey => _speedAnimatorParameterKey;
    public string IsHurtAnimatorParameterKey => _isHurtAnimatorParameterKey;
    public string IsDeadAnimatorParameterKey => _isDeadAnimatorParameterKey;
    public string IsAttackAnimatorParameterKey => _isAttackAnimatorParameterKey;
    public string IsWinAnimatorParameterKey => _isWinAnimatorParameterKey;

    // Attack Parameters
    public float DamageAmount => _damageAmount;
    public float AttackRange => _attackRange;
    public float DetectionRadius => _detectionRadius;
    public float AttackDelay => _attackDelay;
    public float AttackDuration => _attackDuration;
    public float AttackOffset => _attackOffset;
    public LayerMask DamageableLayerMask => _damageableLayerMask;
    public float AttackInterval => _attackInterval;
    public float MinimumMovementSpeedForAttack => _minimumMovementSpeedForAttack;
    public float RotationSpeedForFaceToNearestTarget => _rotationSpeedForFaceToNearestTarget;

    // Health Parameters
    public float MaxHealth => _maxHealth;
    public float StopDurationOnDamage => _stopDurationOnDamage;
    public float DeathWaitTime => _deathWaitTime;

    // Interactor Parameters
    public float InteractionRadius => _interactionRadius;
    public LayerMask InteractableLayerMask => _interactableLayerMask;

    /// <summary>
    /// Sets the parameters for the TopDownCharacterConfigSO ScriptableObject based on the provided values.
    /// This method updates all the internal fields of the ScriptableObject to configure the character's movement, 
    /// attack, health, collection, and interaction settings according to the specified parameters.
    /// </summary>
    /// <param name="showControllerParameters">Whether to show controller parameters in the editor.</param>
    /// <param name="movementSpeed">The movement speed of the character.</param>
    /// <param name="acceleration">The acceleration of the character's movement.</param>
    /// <param name="rotationSpeedThreshold">The threshold for rotation speed before the character starts rotating.</param>
    /// <param name="rotationSpeed">The rotation speed of the character.</param>
    /// <param name="gravity">The gravity affecting the character.</param>
    /// <param name="showCollectorParameters">Whether to show collector parameters in the editor.</param>
    /// <param name="collectRadius">The radius for detecting collectibles.</param>
    /// <param name="stopMovementOnSpecialCollect">Whether the character should stop moving when a special item is collected.</param>
    /// <param name="stopDurationOnSpecialCollect">The duration to stop movement when a special item is collected.</param>
    /// <param name="showAnimatorParameters">Whether to show animator parameters in the editor.</param>
    /// <param name="speedAnimatorParameterKey">The key for the speed parameter in the animator.</param>
    /// <param name="isHurtAnimatorParameterKey">The key for the hurt state in the animator.</param>
    /// <param name="isDeadAnimatorParameterKey">The key for the dead state in the animator.</param>
    /// <param name="isAttackAnimatorParameterKey">The key for the attack state in the animator.</param>
    /// <param name="isWinAnimatorParameterKey">The key for the win state in the animator.</param>
    /// <param name="showAttackParameters">Whether to show attack parameters in the editor.</param>
    /// <param name="damageAmount">The damage amount dealt by the character.</param>
    /// <param name="attackRange">The range within which the character can attack.</param>
    /// <param name="detectionRadius">The detection radius for nearby enemies.</param>
    /// <param name="attackDelay">The delay before an attack begins.</param>
    /// <param name="attackDuration">The duration of the attack animation.</param>
    /// <param name="attackOffset">The offset for the attack hitbox.</param>
    /// <param name="damageableLayerMask">The layers that can be damaged.</param>
    /// <param name="attackInterval">The interval between consecutive attacks.</param>
    /// <param name="minimumMovementSpeedForAttack">The minimum movement speed required for an attack.</param>
    /// <param name="rotationSpeedForFaceToNearestTarget">The rotation speed when facing the nearest target.</param>
    /// <param name="showHealthParameters">Whether to show health parameters in the editor.</param>
    /// <param name="maxHealth">The maximum health of the character.</param>
    /// <param name="stopDurationOnDamage">The duration to stop movement when the character is damaged.</param>
    /// <param name="deathWaitTime">The time to wait after death before transitioning.</param>
    /// <param name="showInteractorParameters">Whether to show interactor parameters in the editor.</param>
    /// <param name="interactionRadius">The interaction radius for detecting interactable objects.</param>
    /// <param name="interactableLayerMask">The layers that are interactable.</param>
    public void SetParameters(
        bool showControllerParameters, float movementSpeed, float acceleration,
        float rotationSpeedThreshold, float rotationSpeed, float gravity,
        bool showCollectorParameters, float collectRadius, bool stopMovementOnSpecialCollect,
        float stopDurationOnSpecialCollect, bool showAnimatorParameters,
        string speedAnimatorParameterKey, string isHurtAnimatorParameterKey,
        string isDeadAnimatorParameterKey, string isAttackAnimatorParameterKey,
        string isWinAnimatorParameterKey, bool showAttackParameters, float damageAmount,
        float attackRange, float detectionRadius, float attackDelay, float attackDuration,
        float attackOffset, LayerMask damageableLayerMask, float attackInterval,
        float minimumMovementSpeedForAttack, float rotationSpeedForFaceToNearestTarget,
        bool showHealthParameters, float maxHealth, float stopDurationOnDamage,
        float deathWaitTime, bool showInteractorParameters, float interactionRadius,
        LayerMask interactableLayerMask
    )
    {
        // Set visibility toggles
        _showControllerParameters = showControllerParameters;
        _showCollectorParameters = showCollectorParameters;
        _showAnimatorParameters = showAnimatorParameters;
        _showAttackParameters = showAttackParameters;
        _showHealthParameters = showHealthParameters;
        _showInteractorParameters = showInteractorParameters;

        // Set Controller Parameters
        _movementSpeed = movementSpeed;
        _acceleration = acceleration;
        _rotationSpeedThreshold = rotationSpeedThreshold;
        _rotationSpeed = rotationSpeed;
        _gravity = gravity;

        // Set Collector Parameters
        _collectRadius = collectRadius;
        _stopMovementOnSpecialCollect = stopMovementOnSpecialCollect;
        _stopDurationOnSpecialCollect = stopDurationOnSpecialCollect;

        // Set Animator Parameters
        _speedAnimatorParameterKey = speedAnimatorParameterKey;
        _isHurtAnimatorParameterKey = isHurtAnimatorParameterKey;
        _isDeadAnimatorParameterKey = isDeadAnimatorParameterKey;
        _isAttackAnimatorParameterKey = isAttackAnimatorParameterKey;
        _isWinAnimatorParameterKey = isWinAnimatorParameterKey;

        // Set Attack Parameters
        _damageAmount = damageAmount;
        _attackRange = attackRange;
        _detectionRadius = detectionRadius;
        _attackDelay = attackDelay;
        _attackDuration = attackDuration;
        _attackOffset = attackOffset;
        _damageableLayerMask = damageableLayerMask;
        _attackInterval = attackInterval;
        _minimumMovementSpeedForAttack = minimumMovementSpeedForAttack;
        _rotationSpeedForFaceToNearestTarget = rotationSpeedForFaceToNearestTarget;

        // Set Health Parameters
        _maxHealth = maxHealth;
        _stopDurationOnDamage = stopDurationOnDamage;
        _deathWaitTime = deathWaitTime;

        // Set Interactor Parameters
        _interactionRadius = interactionRadius;
        _interactableLayerMask = interactableLayerMask;
    }
}
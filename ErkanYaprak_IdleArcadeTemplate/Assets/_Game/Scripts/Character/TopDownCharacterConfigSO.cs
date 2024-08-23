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
}
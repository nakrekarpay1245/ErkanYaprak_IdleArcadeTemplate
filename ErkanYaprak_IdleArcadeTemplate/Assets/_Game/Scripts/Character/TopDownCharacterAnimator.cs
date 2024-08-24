using UnityEngine;
using _Game.Scripts.TopDownCharacter;

/// <summary>
/// Controls the character's animation states and parameters, 
/// fetching necessary data from the TopDownCharacterConfigSO.
/// </summary>
public class TopDownCharacterAnimator : MonoBehaviour
{
    [Header("TopDownCharacterAnimator Parameters")]
    [Tooltip("Reference to the Animator component.")]
    [SerializeField] private Animator _animator;

    [Tooltip("Reference to the character controller.")]
    [SerializeField] private TopDownCharacterController _controller;

    [Tooltip("Reference to the character configuration ScriptableObject.")]
    [SerializeField] private TopDownCharacterConfigSO _characterConfig;

    // Animator Parameter Hashcodes
    private int _speedHashCode;
    private int _isHurtHashCode;
    private int _isDeadHashCode;
    private int _isAttackHashCode;
    private int _isWinHashCode;

    public Animator Animator { get => _animator; set => _animator = value; }
    public TopDownCharacterConfigSO CharacterConfig { get => _characterConfig; set => _characterConfig = value; }

    /// <summary>
    /// Initializes the animator and controller references, 
    /// and caches animator parameter hash codes.
    /// </summary>
    private void Awake()
    {
        // Fetching animator parameter keys from the configuration
        _speedHashCode = Animator.StringToHash(_characterConfig.SpeedAnimatorParameterKey);
        _isHurtHashCode = Animator.StringToHash(_characterConfig.IsHurtAnimatorParameterKey);
        _isDeadHashCode = Animator.StringToHash(_characterConfig.IsDeadAnimatorParameterKey);
        _isAttackHashCode = Animator.StringToHash(_characterConfig.IsAttackAnimatorParameterKey);
        _isWinHashCode = Animator.StringToHash(_characterConfig.IsWinAnimatorParameterKey);

        // Component references
        _animator = GetComponent<Animator>();
        _controller = GetComponentInParent<TopDownCharacterController>();
    }

    /// <summary>
    /// Updates the character's animator parameters each frame, 
    /// like the speed based on the controller's movement.
    /// </summary>
    private void Update()
    {
        SetAnimatorSpeed();
    }

    /// <summary>
    /// Sets the speed parameter in the animator to match the controller's speed.
    /// </summary>
    private void SetAnimatorSpeed()
    {
        _animator.SetFloat(_speedHashCode, _controller.Speed);
    }

    /// <summary>
    /// Triggers the hurt animation based on the configured hash code.
    /// </summary>
    public void PlayHurtAnimation()
    {
        _animator.SetTrigger(_isHurtHashCode);
    }

    /// <summary>
    /// Triggers the death animation based on the configured hash code.
    /// </summary>
    public void PlayDeadAnimation()
    {
        _animator.SetTrigger(_isDeadHashCode);
    }

    /// <summary>
    /// Triggers the attack animation based on the configured hash code.
    /// </summary>
    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(_isAttackHashCode);
    }

    /// <summary>
    /// Triggers the win animation based on the configured hash code.
    /// </summary>
    public void PlayWinAnimation()
    {
        _animator.SetTrigger(_isWinHashCode);
    }
}
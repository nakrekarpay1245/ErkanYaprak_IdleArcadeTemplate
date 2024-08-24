using System.Collections;
using System.Linq;
using UnityEngine;
using _Game.Scripts._Interfaces;
using _Game.Scripts._Abstracts;
using System.Collections.Generic;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// Manages the attack functionality of the character, including detection and attack mechanisms with specified delays and durations.
    /// The configuration is pulled from a ScriptableObject to follow SOLID principles and improve flexibility and performance.
    /// </summary>
    public class TopDownCharacterAttackHandler : AbstractDamagerBase
    {
        [Header("References")]
        [Tooltip("ScriptableObject that holds all character configuration data.")]
        [SerializeField] private TopDownCharacterConfigSO _characterConfig;

        [Tooltip("Reference to the character's controller script.")]
        [SerializeField] private TopDownCharacterController _characterController;

        [Tooltip("Reference to the character's animator script.")]
        [SerializeField] private TopDownCharacterAnimator _characterAnimator;

        [Tooltip("Reference to the character's equipped weapon.")]
        [SerializeField] private Weapon _weapon;

        private float _nextAttackTime = 0f;
        private bool _isAttacking = false;
        private IDamageable _currentTarget;

        public TopDownCharacterConfigSO CharacterConfig { get => _characterConfig; set => _characterConfig = value; }

        private void Awake()
        {
            // Ensure dependencies are assigned
            _characterController = GetComponent<TopDownCharacterController>();
            _characterAnimator = GetComponentInChildren<TopDownCharacterAnimator>();
            _weapon = GetComponentInChildren<Weapon>();
        }

        private void Update()
        {
            // Perform detection and attack only when the character's speed is below the minimum attack speed threshold
            if (_characterController.Speed < _characterConfig.MinimumMovementSpeedForAttack)
            {
                if (Time.time >= _nextAttackTime)
                {
                    PerformDetection();
                    if (_isAttacking)
                    {
                        _nextAttackTime = Time.time + _characterConfig.AttackInterval;
                        StartCoroutine(PerformAttackWithDelay());
                    }
                }

                // Continuously face the current target if available
                if (_currentTarget != null)
                {
                    FaceTarget(_currentTarget);
                }
            }
            else
            {
                StopAllCoroutines();
                _weapon.StopTrailEffect();
                _currentTarget = null;
            }
        }

        /// <summary>
        /// Detects IDamageable targets within the detection radius and updates the nearest target.
        /// </summary>
        private void PerformDetection()
        {
            // Find all colliders within the detection radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _characterConfig.DetectionRadius);

            // Filter out IDamageable targets and include only those on the Damageable layer
            var damageableTargets = hitColliders
                .Where(collider => IsOnDamageableLayer(collider))
                .Select(collider => collider.GetComponent<IDamageable>())
                .Where(damageable => damageable != null && damageable.IsAlive())
                .ToList();

            // Update the attacking state and nearest target
            _isAttacking = damageableTargets.Count > 0;
            _currentTarget = _isAttacking ? FindNearestTarget(damageableTargets) : null;
        }

        /// <summary>
        /// Finds the nearest target from the list of detected IDamageable targets.
        /// </summary>
        /// <param name="targets">The list of detected targets.</param>
        /// <returns>The nearest IDamageable target.</returns>
        private IDamageable FindNearestTarget(List<IDamageable> targets)
        {
            return targets
                .OrderBy(target => Vector3.Distance(transform.position, (target as MonoBehaviour).transform.position))
                .FirstOrDefault();
        }

        /// <summary>
        /// Rotates the character to smoothly face the nearest target using Quaternion calculations.
        /// </summary>
        /// <param name="target">The nearest IDamageable target.</param>
        private void FaceTarget(IDamageable target)
        {
            // Get the position of the target
            Vector3 targetPosition = (target as MonoBehaviour).transform.position;

            // Calculate the direction to the target
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;

            // Calculate the rotation needed to face the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _characterConfig.RotationSpeedForFaceToNearestTarget);
        }

        /// <summary>
        /// Coroutine to handle the delay and execution of the attack.
        /// </summary>
        private IEnumerator PerformAttackWithDelay()
        {
            _characterAnimator.PlayAttackAnimation();
            _weapon.PlayTrailEffect();

            // Wait for the attack delay
            yield return new WaitForSeconds(_characterConfig.AttackDelay);

            // Start the attack
            ExecuteAttack();

            // Wait for the attack duration to end
            yield return new WaitForSeconds(_characterConfig.AttackDuration);

            _weapon.StopTrailEffect();
        }

        /// <summary>
        /// Performs an attack by applying damage in front of the character.
        /// </summary>
        public override void ExecuteAttack()
        {
            // Calculate the attack position
            Vector3 attackPosition = transform.position + transform.forward * _characterConfig.AttackOffset;

            // Find all colliders within the attack radius
            Collider[] hitColliders = Physics.OverlapSphere(attackPosition, _characterConfig.AttackRange, _characterConfig.DamageableLayerMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageable damageable) && damageable.IsAlive())
                {
                    // Apply damage to each IDamageable component found
                    DealDamage(damageable, _characterConfig.DamageAmount);

                    _weapon.StopTrailEffect();
                }
            }
        }

        /// <summary>
        /// Applies damage to a target that implements IDamageable.
        /// </summary>
        /// <param name="target">The target to damage.</param>
        /// <param name="damageAmount">The amount of damage to deal.</param>
        public override void DealDamage(IDamageable target, float damageAmount)
        {
            target.TakeDamage(damageAmount);
        }

        /// <summary>
        /// Checks if the collider is on a Damageable layer.
        /// </summary>
        /// <param name="collider">The collider to check.</param>
        /// <returns>True if the collider is on the Damageable layer; otherwise, false.</returns>
        private bool IsOnDamageableLayer(Collider collider)
        {
            return (_characterConfig.DamageableLayerMask & (1 << collider.gameObject.layer)) != 0;
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize the detection and attack radii.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _characterConfig.DetectionRadius);

            Gizmos.color = Color.red;
            Vector3 attackPosition = transform.position + transform.forward * _characterConfig.AttackOffset;
            Gizmos.DrawWireSphere(attackPosition, _characterConfig.AttackRange);
        }
    }
}
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
    /// </summary>
    public class TopDownCharacterAttackHandler : AbstractDamagerBase
    {
        [Header("Detection Settings")]
        [Tooltip("Radius within which to detect IDamageable targets.")]
        [SerializeField] private float _detectionRadius = 5f;

        [Header("Attack Settings")]
        [Tooltip("Delay before attack is performed after detection.")]
        [SerializeField] private float _attackDelay = 0.5f;

        [Tooltip("Duration for the attack to be active.")]
        [SerializeField] private float _attackDuration = 0.2f;

        [Tooltip("Offset distance in front of the character where the attack is applied.")]
        [SerializeField] private float _attackOffset = 1f;

        [Tooltip("Layer mask to specify which layers are considered for attack.")]
        [SerializeField] private LayerMask _damageableLayerMask;

        [Tooltip("Interval between consecutive attacks.")]
        [SerializeField] private float _attackInterval = 1f;

        [SerializeField] private float _minimumSpeedForAttack = 0.25f;

        [SerializeField] private Weapon _weapon;

        [Header("Rotation Settings")]
        [Tooltip("Speed at which the character rotates to face the target.")]
        [SerializeField] private float _rotationSpeed = 5f;

        private TopDownCharacterController _characterController;
        private TopDownCharacterAnimator _characterAnimator;
        private float _nextAttackTime = 0f;
        private bool _isAttacking = false;
        private IDamageable _currentTarget;

        private void Awake()
        {
            _characterController = GetComponent<TopDownCharacterController>();
            _characterAnimator = GetComponentInChildren<TopDownCharacterAnimator>();
            _weapon = GetComponentInChildren<Weapon>();
        }

        private void Update()
        {
            if (_characterController.Speed < _minimumSpeedForAttack)
            {
                if (Time.time >= _nextAttackTime)
                {
                    PerformDetection();
                    if (_isAttacking)
                    {
                        _nextAttackTime = Time.time + _attackInterval;
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
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRadius);

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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }

        /// <summary>
        /// Coroutine to handle the delay and execution of the attack.
        /// </summary>
        private IEnumerator PerformAttackWithDelay()
        {
            _characterAnimator.PlayAttackAnimation();
            _weapon.PlayTrailEffect();

            // Wait for the attack delay
            yield return new WaitForSeconds(_attackDelay);

            // Start the attack
            ExecuteAttack();

            // Wait for the attack duration to end
            yield return new WaitForSeconds(_attackDuration);

            _weapon.StopTrailEffect();
        }

        /// <summary>
        /// Performs an attack by applying damage in front of the character.
        /// </summary>
        public override void ExecuteAttack()
        {
            // Calculate the attack position
            Vector3 attackPosition = transform.position + transform.forward * _attackOffset;

            // Find all colliders within the attack radius
            Collider[] hitColliders = Physics.OverlapSphere(attackPosition, _attackRange, _damageableLayerMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageable damageable) && damageable.IsAlive())
                {
                    Debug.Log(hitCollider.name);
                    // Apply damage to each IDamageable component found
                    DealDamage(damageable, _damageAmount);

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
            return (_damageableLayerMask & (1 << collider.gameObject.layer)) != 0;
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize the detection and attack radii.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);

            Gizmos.color = Color.red;
            Vector3 attackPosition = transform.position + transform.forward * _attackOffset;
            Gizmos.DrawWireSphere(attackPosition, _attackRange);
        }
    }
}
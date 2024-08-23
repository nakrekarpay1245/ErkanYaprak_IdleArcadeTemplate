using System.Collections;
using System.Linq;
using UnityEngine;
using _Game.Scripts._Interfaces;
using _Game.Scripts._Abstracts;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// This class handles the attack functionality of the character,
    /// including detection and attack mechanisms with specified delays and durations.
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

        private TopDownCharacterController _characterController;
        private TopDownCharacterAnimator _characterAnimator;
        private float _nextAttackTime = 0f;
        private bool _isAttacking = false;

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
            }
            else
            {
                StopAllCoroutines();
                _weapon.StopTrailEffect();
            }
        }

        /// <summary>
        /// Detects IDamageable targets within the detection radius.
        /// </summary>
        private void PerformDetection()
        {
            // Find all colliders within the detection radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position,
                _detectionRadius);

            // Filter out IDamageable targets and include only those on the Damageable layer
            var damageableTargets = hitColliders
                .Where(collider => IsOnDamageableLayer(collider))
                .Select(collider => collider.GetComponent<IDamageable>())
                .Where(damageable => damageable != null)
                .ToList();

            _isAttacking = damageableTargets.Count > 0;
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

            // Optional: reset attack state if needed
            _isAttacking = false;
        }

        /// <summary>
        /// Performs an attack by applying damage in front of the character.
        /// </summary>
        public override void ExecuteAttack()
        {
            // Calculate the attack position
            Vector3 attackPosition = transform.position + transform.forward * _attackOffset;

            // Find all colliders within the attack radius
            Collider[] hitColliders = Physics.OverlapSphere(attackPosition, _attackRange,
                _damageableLayerMask);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out IDamageable damageable))
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
using System.Collections;
using UnityEngine;
using _Game.Scripts._Interfaces;

namespace _Game.Scripts.Test
{
    /// <summary>
    /// A MonoBehaviour that demonstrates how to use the IDamager interface with an OverlapBox to apply damage.
    /// This script also handles temporarily disabling and re-enabling Collider and MeshRenderer components.
    /// </summary>
    public class TestDamageDealer : MonoBehaviour, IDamager
    {
        [Header("Damage Settings")]
        [Tooltip("Amount of damage to deal.")]
        [Range(0f, 10f)]
        [SerializeField]
        private float _damageAmount = 10f;

        [Header("Disable Settings")]
        [Tooltip("Duration in seconds to disable Collider and MeshRenderer.")]
        [SerializeField]
        [Range(5f, 10f)]
        private float _disableDuration = 5f;

        private Collider _collider;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            // Cache the Collider and MeshRenderer components
            _collider = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        /// <summary>
        /// Deals damage to the specified target.
        /// </summary>
        /// <param name="target">The target to damage.</param>
        /// <param name="damageAmount">The amount of damage to deal.</param>
        public void DealDamage(IDamageable target, float damageAmount)
        {
            if (target == null)
            {
                Debug.LogWarning("Target is null. Cannot deal damage.");
                return;
            }

            target.TakeDamage(damageAmount);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                DealDamage(damageable, _damageAmount);
                StartCoroutine(DisableComponentsCoroutine());
            }
        }

        /// <summary>
        /// Coroutine that disables Collider and MeshRenderer components for a specified duration.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisableComponentsCoroutine()
        {
            // Check if components exist before disabling
            if (_collider != null)
            {
                _collider.enabled = false;
            }

            if (_meshRenderer != null)
            {
                _meshRenderer.enabled = false;
            }

            // Wait for the specified duration
            yield return new WaitForSeconds(_disableDuration);

            // Re-enable components
            if (_collider != null)
            {
                _collider.enabled = true;
            }

            if (_meshRenderer != null)
            {
                _meshRenderer.enabled = true;
            }
        }
    }
}
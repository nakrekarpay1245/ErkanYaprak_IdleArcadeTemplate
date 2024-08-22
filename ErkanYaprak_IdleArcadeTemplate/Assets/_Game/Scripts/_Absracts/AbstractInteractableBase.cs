using _Game.Scripts._Interfaces;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace _Game.Scripts._Abstracts
{
    /// <summary>
    /// Base class for interactable objects with progress bar and SphereCollider functionality.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractInteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [Tooltip("The duration for the interaction.")]
        [SerializeField, Range(1f, 10f)]
        private float _interactionDuration = 5f;

        [Tooltip("The UnityEvent to trigger when interaction is completed.")]
        [SerializeField]
        private UnityEvent _onInteractionComplete;

        [Header("Sphere Collider Settings")]
        [Tooltip("The initial radius of the SphereCollider.")]
        [SerializeField]
        private float _initialColliderRadius = 1f;

        [Tooltip("The delay before deactivating the GameObject.")]
        [SerializeField, Range(0f, 5f)]
        private float _deactivationDelay = 1f;

        private SphereCollider _sphereCollider;
        private float _interactionTimeRemaining;
        private bool _isInteracting;

        private void Awake()
        {
            // Ensure the SphereCollider is attached and update its radius
            _sphereCollider = GetComponent<SphereCollider>();
            if (_sphereCollider != null)
            {
                // Set the initial radius of the SphereCollider
                _sphereCollider.radius = _initialColliderRadius;
            }
            else
            {
                Debug.LogError("SphereCollider component is missing.");
            }
        }

        private void Update()
        {
            if (_isInteracting)
            {
                // Update the interaction progress
                _interactionTimeRemaining -= Time.deltaTime;
                if (_interactionTimeRemaining <= 0)
                {
                    CompleteInteraction();
                }
            }
        }

        public virtual void StartInteraction()
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                _interactionTimeRemaining = _interactionDuration;
                // Start filling the progress bar here
            }
        }

        public virtual void CompleteInteraction()
        {
            if (_isInteracting)
            {
                _isInteracting = false;
                // Trigger the event
                _onInteractionComplete?.Invoke();
                // Start the coroutine to deactivate the object after a delay
                StartCoroutine(DeactivateAfterDelay());
            }
        }

        private IEnumerator DeactivateAfterDelay()
        {
            // Deactivate the SphereCollider
            if (_sphereCollider != null)
            {
                _sphereCollider.enabled = false;
            }

            // Wait for the specified delay
            yield return new WaitForSeconds(_deactivationDelay);

            // Deactivate this object
            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_sphereCollider != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _sphereCollider.radius);
            }
        }
#endif
    }
}
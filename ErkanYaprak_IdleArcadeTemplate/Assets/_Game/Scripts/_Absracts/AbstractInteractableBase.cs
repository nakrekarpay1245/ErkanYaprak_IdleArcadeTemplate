using _Game.Scripts._Interfaces;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace _Game.Scripts._Abstracts
{
    /// <summary>
    /// Base class for interactable objects that include progress functionality and use a SphereCollider
    /// for interaction detection. Handles interaction start, completion, and cancellation, with optional deactivation.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractInteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [Tooltip("The duration for the interaction.")]
        [SerializeField, Range(1f, 10f)]
        protected float _interactionDuration = 5f;

        [Tooltip("Event triggered when the interaction is completed.")]
        [SerializeField]
        private UnityEvent _onInteractionComplete;

        [Tooltip("Event triggered when the interaction is canceled.")]
        [SerializeField]
        private UnityEvent _onInteractionCancel;

        [Header("Sphere Collider Settings")]
        [Tooltip("The initial radius of the SphereCollider.")]
        [SerializeField]
        private float _initialColliderRadius = 1f;

        [Tooltip("The delay before deactivating the GameObject after interaction is completed.")]
        [SerializeField, Range(0f, 5f)]
        private float _deactivationDelay = 1f;

        protected SphereCollider _sphereCollider;
        protected float _interactionTimeRemaining;
        protected bool _isInteracting;
        private Coroutine _interactionCoroutine;

        private void Awake()
        {
            // Ensure the SphereCollider is attached and set its radius
            _sphereCollider = GetComponent<SphereCollider>();
            if (_sphereCollider != null)
            {
                _sphereCollider.radius = _initialColliderRadius;
            }
            else
            {
                Debug.LogError("SphereCollider component is missing.");
            }
        }

        private void Update()
        {
            // Manage the interaction timer
            if (_isInteracting)
            {
                _interactionTimeRemaining -= Time.deltaTime;
                if (_interactionTimeRemaining <= 0)
                {
                    CompleteInteraction();
                }
            }
        }

        /// <summary>
        /// Starts the interaction process, setting interaction time and flag.
        /// </summary>
        public virtual void StartInteraction()
        {
            if (!_isInteracting)
            {
                _isInteracting = true;
                _interactionTimeRemaining = _interactionDuration;
                // Additional logic for starting interaction (e.g. UI updates) can be placed here
            }
        }

        /// <summary>
        /// Completes the interaction, triggers the completion event, and deactivates the GameObject after a delay.
        /// </summary>
        public virtual void CompleteInteraction()
        {
            if (_isInteracting)
            {
                _isInteracting = false;
                _onInteractionComplete?.Invoke(); // Trigger completion event
                StartCoroutine(DeactivateAfterDelay());
            }
        }

        /// <summary>
        /// Cancels the interaction process, resets interaction state, and triggers cancellation event.
        /// </summary>
        public virtual void CancelInteraction()
        {
            if (_isInteracting)
            {
                _isInteracting = false;
                _onInteractionCancel?.Invoke(); // Trigger cancellation event

                // Stop any ongoing interaction coroutine
                if (_interactionCoroutine != null)
                {
                    StopCoroutine(_interactionCoroutine);
                    _interactionCoroutine = null;
                }

                // Reset interaction state and related variables (e.g. UI)
            }
        }

        /// <summary>
        /// Deactivates the GameObject after the specified delay, disables the SphereCollider.
        /// </summary>
        private IEnumerator DeactivateAfterDelay()
        {
            if (_sphereCollider != null)
            {
                _sphereCollider.enabled = false;
            }

            yield return new WaitForSeconds(_deactivationDelay);

            gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws the Gizmos in the editor for visualizing the SphereCollider radius.
        /// </summary>
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
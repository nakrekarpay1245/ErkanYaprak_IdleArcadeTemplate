using _Game.Scripts._Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts._Base
{
    /// <summary>
    /// Base class for interactable objects with progress bar and SphereCollider functionality.
    /// </summary>
    public abstract class AbstractInteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interaction Settings")]
        [Tooltip("The duration for the interaction.")]
        [SerializeField, Range(1f, 10f)]
        private float interactionDuration = 5f;

        [Tooltip("The UnityEvent to trigger when interaction is completed.")]
        [SerializeField]
        private UnityEvent onInteractionComplete;

        [Header("Sphere Collider Settings")]
        [Tooltip("The initial radius of the SphereCollider.")]
        [SerializeField]
        private float initialColliderRadius = 1f;

        private SphereCollider sphereCollider;
        private float interactionTimeRemaining;
        private bool isInteracting;

        private void Awake()
        {
            // Ensure the SphereCollider is attached and update its radius
            sphereCollider = GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                // Set the initial radius of the SphereCollider
                sphereCollider.radius = initialColliderRadius;
            }
            else
            {
                Debug.LogError("SphereCollider component is missing.");
            }
        }

        private void Update()
        {
            if (isInteracting)
            {
                // Update the interaction progress
                interactionTimeRemaining -= Time.deltaTime;
                if (interactionTimeRemaining <= 0)
                {
                    CompleteInteraction();
                }
            }
        }

        public void StartInteraction()
        {
            if (!isInteracting)
            {
                isInteracting = true;
                interactionTimeRemaining = interactionDuration;
                // Start filling the progress bar here
            }
        }

        public void CompleteInteraction()
        {
            if (isInteracting)
            {
                isInteracting = false;
                // Trigger the event
                onInteractionComplete?.Invoke();
                // Deactivate the SphereCollider
                if (sphereCollider != null)
                {
                    sphereCollider.enabled = false;
                }
                // Deactivate this object
                gameObject.SetActive(false);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (sphereCollider != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
            }
        }
#endif
    }
}
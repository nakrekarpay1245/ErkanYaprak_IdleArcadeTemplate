using UnityEngine;
using System.Linq;
using _Game.Scripts._Interfaces;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// Handles interaction with the nearest IInteractable objects within a specified radius.
    /// Starts and cancels interactions based on proximity to the player character.
    /// </summary>
    public class TopDownCharacterInteractor : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [Tooltip("The radius within which to detect interactable objects.")]
        [SerializeField, Range(1f, 10f)]
        private float _interactionRadius = 5f;

        [Tooltip("The layer mask to filter interactable objects.")]
        [SerializeField]
        private LayerMask _interactableLayerMask;

        private IInteractable _currentInteractable;

        private void Update()
        {
            HandleInteraction();
        }

        /// <summary>
        /// Finds the closest interactable object within range, starts interaction with it,
        /// and cancels the interaction if the object moves out of range.
        /// </summary>
        private void HandleInteraction()
        {
            IInteractable closestInteractable = FindClosestInteractable();

            // Handle new interactions
            if (closestInteractable != _currentInteractable)
            {
                CancelCurrentInteraction();

                if (closestInteractable != null)
                {
                    StartInteractionWith(closestInteractable);
                }
            }

            // Cancel the interaction if the current interactable is out of range
            if (_currentInteractable != null && !IsWithinInteractionRadius(_currentInteractable))
            {
                CancelCurrentInteraction();
            }
        }

        /// <summary>
        /// Finds the closest interactable object within the specified radius.
        /// </summary>
        /// <returns>The closest interactable object, or null if none are within range.</returns>
        private IInteractable FindClosestInteractable()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _interactionRadius, _interactableLayerMask);

            return colliders
                .Select(collider => collider.GetComponent<IInteractable>())
                .Where(interactable => interactable != null)
                .OrderBy(interactable => Vector3.Distance(transform.position, (interactable as Component).transform.position))
                .FirstOrDefault();
        }

        /// <summary>
        /// Starts interaction with the specified interactable object.
        /// </summary>
        /// <param name="interactable">The interactable object to start interaction with.</param>
        private void StartInteractionWith(IInteractable interactable)
        {
            _currentInteractable = interactable;
            _currentInteractable.StartInteraction();
        }

        /// <summary>
        /// Cancels the current interaction if an interactable is being interacted with.
        /// </summary>
        private void CancelCurrentInteraction()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.CancelInteraction();
                _currentInteractable = null;
            }
        }

        /// <summary>
        /// Checks if the specified interactable object is within the interaction radius.
        /// </summary>
        /// <param name="interactable">The interactable object to check.</param>
        /// <returns>True if the object is within the interaction radius, false otherwise.</returns>
        private bool IsWithinInteractionRadius(IInteractable interactable)
        {
            if (interactable == null) return false;

            float distance = Vector3.Distance(transform.position, (interactable as Component).transform.position);
            return distance <= _interactionRadius;
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws a wire sphere in the Unity Editor to visualize the interaction radius.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _interactionRadius);
        }
#endif
    }
}
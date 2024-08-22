using UnityEngine;
using System.Linq;
using _Game.Scripts._Interfaces;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// Handles interaction with the nearest IInteractable objects within a specified radius.
    /// </summary>
    public class TopDownCharacterInteractor : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [Tooltip("The radius within which to find interactable objects.")]
        [SerializeField, Range(1f, 10f)]
        private float interactionRadius = 5f;

        [Tooltip("The layer mask to use for detecting interactable objects.")]
        [SerializeField]
        private LayerMask interactableLayerMask;

        private IInteractable currentInteractable;

        private void Update()
        {
            FindAndInteractWithClosestInteractable();
        }

        /// <summary>
        /// Finds the closest interactable object within the interaction radius and starts interaction with it.
        /// </summary>
        private void FindAndInteractWithClosestInteractable()
        {
            // Find all interactable objects within the interaction radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius, interactableLayerMask);

            // Filter and find the closest interactable
            var interactables = colliders
                .Select(collider => collider.GetComponent<IInteractable>())
                .Where(interactable => interactable != null)
                .ToList();

            if (interactables.Count > 0)
            {
                IInteractable closestInteractable = interactables
                    .OrderBy(interactable => Vector3.Distance(transform.position, (interactable as Component).transform.position))
                    .First();

                if (currentInteractable != closestInteractable)
                {
                    currentInteractable = closestInteractable;
                    currentInteractable.StartInteraction();
                }
            }
            else
            {
                // No interactables within range, reset current interactable
                if (currentInteractable != null)
                {
                    currentInteractable = null;
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
#endif
    }
}
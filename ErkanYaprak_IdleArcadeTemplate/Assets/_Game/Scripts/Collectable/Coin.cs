using UnityEngine;
using System.Collections;

namespace _Game.Scripts.Collectable
{
    /// <summary>
    /// Represents a collectible coin item in the game.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class Coin : MonoBehaviour, ICollectable
    {
        [Header("Coin Settings")]
        [SerializeField, Tooltip("The value of the coin when collected.")]
        private int coinValue = 1;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("The duration of the scaling and moving animation.")]
        private float animationDuration = 1f;

        [SerializeField, Tooltip("Scale factor when the coin is collected.")]
        private float scaleFactor = 1.5f;

        [SerializeField, Tooltip("Height to move the coin up during animation.")]
        private float moveUpHeight = 2f;

        [SerializeField, Tooltip("Time to wait between animation phases.")]
        private float waitTime = 0.33f; // Default to 1/3 of the animation duration

        private Vector3 originalPosition;
        private Vector3 targetPosition;
        private SphereCollider sphereCollider;

        private void Awake()
        {
            // Store the original position for animation purposes
            originalPosition = transform.position;

            // Calculate the target position based on moveUpHeight
            targetPosition = originalPosition + Vector3.up * moveUpHeight;

            // Get the SphereCollider component
            sphereCollider = GetComponent<SphereCollider>();
        }

        /// <summary>
        /// Initiates the collection process, including animation and value handling.
        /// </summary>
        /// <param name="collector">The object collecting this coin as a Transform.</param>
        public void OnCollect(ICollector collector)
        {
            // Disable the collider to prevent further collisions
            if (sphereCollider != null)
            {
                sphereCollider.enabled = false;
            }

            // Get the Transform from the GameObject of the collector
            Transform collectorTransform = (collector as MonoBehaviour)?.transform;
            if (collectorTransform != null)
            {
                StartCoroutine(CollectAnimation(collectorTransform));
            }
            else
            {
                Debug.LogError("Collector does not have a valid Transform.");
            }
        }

        /// <summary>
        /// Plays the collection animation for the coin.
        /// </summary>
        /// <param name="collector">The object collecting this coin as a Transform.</param>
        private IEnumerator CollectAnimation(Transform collector)
        {
            // Scale up
            float elapsedTime = 0f;
            Vector3 initialScale = transform.localScale;
            Vector3 targetScale = initialScale * scaleFactor; // Use scaleFactor parameter

            while (elapsedTime < animationDuration / 3f)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale,
                    elapsedTime / (animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;

            // Wait
            yield return new WaitForSeconds(waitTime);

            // Move up
            elapsedTime = 0f;
            Vector3 initialPosition = transform.position;

            while (elapsedTime < animationDuration / 3f)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition,
                    elapsedTime / (animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPosition;

            // Wait
            yield return new WaitForSeconds(waitTime);

            // Scale down and move towards the collector's position
            elapsedTime = 0f;
            Vector3 downScale = initialScale;
            Vector3 downPosition = collector.position;

            while (elapsedTime < animationDuration / 3f)
            {
                transform.localScale = Vector3.Lerp(targetScale, downScale,
                    elapsedTime / (animationDuration / 3f));
                transform.position = Vector3.Lerp(targetPosition, downPosition,
                    elapsedTime / (animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = downScale;
            transform.position = downPosition;

            // Deactivate the coin object after animation
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Draws a visual representation of the coin's animation parameters in the Scene view for debugging.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            // Example size of the coin
            Gizmos.DrawWireSphere(transform.position, 0.5f);

            // Visualize the target position
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * moveUpHeight);
        }
    }
}
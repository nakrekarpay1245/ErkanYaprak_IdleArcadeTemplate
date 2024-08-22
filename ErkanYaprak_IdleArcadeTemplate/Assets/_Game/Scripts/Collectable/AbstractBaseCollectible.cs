using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.Collectable
{
    /// <summary>
    /// Abstract base class for all collectible items in the game.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractBaseCollectible : MonoBehaviour, ICollectable
    {
        [Header("Collectible Settings")]
        [SerializeField, Tooltip("The value of the collectible when collected.")]
        protected int _collectibleValue = 1;

        [SerializeField, Tooltip("Event triggered when this item is collected.")]
        private UnityEvent _onCollectEvent;

        [Header("Animation Settings")]
        [SerializeField, Tooltip("The duration of the scaling and moving animation.")]
        protected float _animationDuration = 0.5f;

        [SerializeField, Tooltip("Scale factor when the collectible is collected.")]
        protected float _scaleFactor = 1.5f;

        [SerializeField, Tooltip("Height to move the collectible up during animation.")]
        protected float _moveUpHeight = 2f;

        [SerializeField, Tooltip("Time to wait between animation phases.")]
        protected float _waitTime = 0.1f; // Default to 1/5 of the animation duration

        protected Vector3 _originalPosition;
        protected Vector3 _targetPosition;
        protected SphereCollider _sphereCollider;

        protected virtual void Awake()
        {
            // Store the original position for animation purposes
            _originalPosition = transform.position;

            // Calculate the target position based on moveUpHeight
            _targetPosition = _originalPosition + Vector3.up * _moveUpHeight;

            // Get the SphereCollider component
            _sphereCollider = GetComponent<SphereCollider>();
        }

        /// <summary>
        /// Called when the object is collected.
        /// </summary>
        /// <param name="collector">The object collecting this item.</param>
        public virtual void OnCollect(ICollector collector)
        {
            if (_sphereCollider != null)
            {
                _sphereCollider.enabled = false;
            }

            _onCollectEvent?.Invoke();

            // Start the collection animation
            StartCoroutine(CollectAnimation((collector as MonoBehaviour)?.transform));
        }

        /// <summary>
        /// Defines the collection animation, which can be overridden for specific behaviors.
        /// </summary>
        protected virtual IEnumerator CollectAnimation(Transform collector)
        {
            // Animation logic (scaling and movement) can be implemented here or overridden in derived classes.
            yield return null;
        }

        /// <summary>
        /// Draws a visual representation of the collectible's animation parameters in the Scene view.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * _moveUpHeight);
        }
    }
}
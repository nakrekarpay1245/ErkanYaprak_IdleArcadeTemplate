using _Game.Scripts._Interfaces;
using _Game.Scripts.Collectable;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// Handles the collection of items by the character in a top-down game.
    /// Detects nearby collectable items and triggers events when they are collected.
    /// Configuration is loaded from a ScriptableObject for improved flexibility.
    /// </summary>
    public class TopDownCharacterCollector : MonoBehaviour, ICollector
    {
        [Header("Configuration")]
        [Tooltip("Reference to the character's configuration ScriptableObject.")]
        [SerializeField] private TopDownCharacterConfigSO _characterConfig;

        [Header("Events")]
        [Tooltip("Action triggered when an item is collected.")]
        [SerializeField] private UnityEvent<ICollectable> _onCollect;

        private Collider[] _collidersInRange;

        [Header("References")]
        [Tooltip("Reference to the character animator for playing animations.")]
        [SerializeField] private TopDownCharacterAnimator _characterAnimator;

        [Tooltip("Reference to the TopDownCharacterController for controlling movement.")]
        [SerializeField] private TopDownCharacterController _characterController;

        public TopDownCharacterConfigSO CharacterConfig { get => _characterConfig; set => _characterConfig = value; }

        private void Awake()
        {
            // Ensure dependencies are assigned
            _characterAnimator = GetComponentInChildren<TopDownCharacterAnimator>();
            _characterController = GetComponent<TopDownCharacterController>();

            // Initialize the collider array based on expected size
            _collidersInRange = new Collider[10]; // Adjust size based on expected number of items

            // Validate that the ScriptableObject is assigned
            if (_characterConfig == null)
            {
                Debug.LogWarning("TopDownCharacterConfigSO is not assigned in" +
                    " TopDownCharacterCollector.");
            }

            // Validate that the TopDownCharacterAnimator is assigned
            if (_characterAnimator == null)
            {
                Debug.LogWarning("TopDownCharacterAnimator is not assigned in" +
                    " TopDownCharacterCollector.");
            }

            // Validate that the TopDownCharacterController is assigned
            if (_characterController == null)
            {
                Debug.LogWarning("TopDownCharacterController is not assigned in" +
                    " TopDownCharacterCollector.");
            }
        }

        private void Update()
        {
            // Check for collectable items within the specified radius
            DetectAndCollectItems();
        }

        /// <summary>
        /// Detects and collects items within the collect radius.
        /// </summary>
        private void DetectAndCollectItems()
        {
            // Use Physics.OverlapSphere to find colliders within the collect radius
            int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, _characterConfig.CollectRadius, _collidersInRange);

            for (int i = 0; i < colliderCount; i++)
            {
                Collider collider = _collidersInRange[i];
                if (collider != null && collider.TryGetComponent(out ICollectable collectable))
                {
                    Collect(collectable);
                }
            }
        }

        /// <summary>
        /// Collects a given collectable item and triggers the onCollect event.
        /// </summary>
        /// <param name="collectable">The collectable item to collect.</param>
        public void Collect(ICollectable collectable)
        {
            collectable.OnCollect(this);
            _onCollect?.Invoke(collectable);

            // Check if the collectable is a SpecialCollectible
            if (collectable is SpecialCollectible)
            {
                if (_characterAnimator != null)
                {
                    _characterAnimator.PlayWinAnimation(); // Trigger the win animation for special items
                }
                // Pause movement if enabled in the character configuration
                if (_characterConfig.StopMovementOnSpecialCollect)
                {

                    if (_characterController != null)
                    {
                        _characterController.PauseMovement(_characterConfig.StopDurationOnSpecialCollect);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the collect radius in the Scene view for debugging purposes.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _characterConfig.CollectRadius);
        }
    }
}
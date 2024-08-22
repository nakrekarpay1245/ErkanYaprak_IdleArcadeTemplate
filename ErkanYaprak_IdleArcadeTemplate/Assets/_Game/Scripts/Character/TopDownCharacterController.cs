using UnityEngine;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// This class handles the character's movement and rotation in an idle-arcade style game.
    /// It uses Unity's CharacterController component to manage physics-based movement.
    /// The character's speed is dynamically set in the Animator based on movement input.
    /// Additionally, it implements smooth acceleration and deceleration for more natural movement.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class TopDownCharacterController : MonoBehaviour
    {
        /// <summary>
        /// The speed at which the character moves.
        /// This value can be adjusted in the Unity Inspector.
        /// </summary>
        [Header("Movement Settings")]
        [Tooltip("The maximum speed at which the character moves.")]
        [SerializeField] private float _movementSpeed = 5f;

        /// <summary>
        /// The acceleration and deceleration rate.
        /// </summary>
        [Tooltip("The rate at which the character accelerates and decelerates.")]
        [SerializeField] private float _acceleration = 2f;

        /// <summary>
        /// The speed threshold for determining when to rotate the character.
        /// </summary>
        [Tooltip("The minimum speed required to rotate the character.")]
        [SerializeField] private float _rotationSpeedThreshold = 0.1f;

        /// <summary>
        /// The rotation speed for smoothing the character's rotation.
        /// </summary>
        [Tooltip("Speed at which the character rotates to face movement direction.")]
        [SerializeField] private float _rotationSpeed = 720f;

        /// <summary>
        /// The gravity force applied to the character.
        /// </summary>
        [Tooltip("The gravity force applied to the character.")]
        [SerializeField] private float _gravity = -9.81f;

        /// <summary>
        /// Reference to the CharacterController component.
        /// </summary>
        private CharacterController _characterController;

        /// <summary>
        /// The velocity of the character in the vertical direction.
        /// </summary>
        private Vector3 _velocity;

        /// <summary>
        /// The current movement velocity in the horizontal plane.
        /// </summary>
        private Vector3 _currentVelocity;

        [Header("Publics")]
        [HideInInspector]
        public float Speed;

        [Header("References")]
        [Tooltip("")]
        [SerializeField]
        private TopDownCharacterAnimator _animator;
        [Tooltip("")]
        [SerializeField]
        private Joystick _joystick;

        /// <summary>
        /// Initialize components.
        /// </summary>
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<TopDownCharacterAnimator>();
        }

        /// <summary>
        /// Handles character movement and rotation based on player input.
        /// </summary>
        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        /// <summary>
        /// Handles the character's movement based on player input.
        /// Implements smooth acceleration and deceleration.
        /// </summary>
        private void HandleMovement()
        {
            // Get input from keyboard
            float horizontal = Input.GetAxis("Horizontal") + _joystick.Horizontal;
            float vertical = Input.GetAxis("Vertical") + _joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            // Calculate target velocity based on input direction and speed
            Vector3 targetVelocity = direction * _movementSpeed;

            // Smoothly adjust current velocity towards target velocity
            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, _acceleration * Time.deltaTime);
            Speed = _currentVelocity.magnitude / _movementSpeed;

            // Move character
            _characterController.Move(_currentVelocity * Time.deltaTime);

            // Apply gravity
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0f;
            }

            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }

        /// <summary>
        /// Handles the character's rotation to face the movement direction.
        /// </summary>
        private void HandleRotation()
        {
            // Get input from keyboard
            float horizontal = Input.GetAxis("Horizontal") + _joystick.Horizontal;
            float vertical = Input.GetAxis("Vertical") + _joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= _rotationSpeedThreshold)
            {
                // Determine the target rotation based on movement direction
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Smoothly rotate the character towards the target rotation
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }
}
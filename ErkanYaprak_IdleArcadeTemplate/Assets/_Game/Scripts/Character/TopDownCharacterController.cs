using System.Collections;
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
        [Header("Movement Settings")]
        [Tooltip("The maximum speed at which the character moves.")]
        [SerializeField] private float _movementSpeed = 5f;

        [Tooltip("The rate at which the character accelerates and decelerates.")]
        [SerializeField] private float _acceleration = 2f;

        [Tooltip("The minimum speed required to rotate the character.")]
        [SerializeField] private float _rotationSpeedThreshold = 0.1f;

        [Tooltip("Speed at which the character rotates to face movement direction.")]
        [SerializeField] private float _rotationSpeed = 720f;

        [Tooltip("The gravity force applied to the character.")]
        [SerializeField] private float _gravity = -9.81f;

        private CharacterController _characterController;
        private Vector3 _velocity;
        private Vector3 _currentVelocity;
        private bool _isMovementPaused = false; // Track if movement is paused
        private float _savedMovementSpeed; // Store the original speed

        [Header("Publics")]
        [HideInInspector]
        public float Speed;

        [Header("References")]
        [SerializeField] private TopDownCharacterAnimator _animator;
        [SerializeField] private Joystick _joystick;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<TopDownCharacterAnimator>();
        }

        private void Update()
        {
            if (!_isMovementPaused)
            {
                HandleMovement();
                HandleRotation();
            }
        }

        /// <summary>
        /// Handles the character's movement based on player input.
        /// Implements smooth acceleration and deceleration.
        /// </summary>
        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal") + _joystick.Horizontal;
            float vertical = Input.GetAxis("Vertical") + _joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 targetVelocity = direction * _movementSpeed;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, _acceleration * Time.deltaTime);
            Speed = _currentVelocity.magnitude / _movementSpeed;

            _characterController.Move(_currentVelocity * Time.deltaTime);

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
            float horizontal = Input.GetAxis("Horizontal") + _joystick.Horizontal;
            float vertical = Input.GetAxis("Vertical") + _joystick.Vertical;

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            if (direction.magnitude >= _rotationSpeedThreshold)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Pauses the character's movement for a specified duration.
        /// The movement speed is restored after the duration ends.
        /// </summary>
        /// <param name="duration">The time in seconds to pause movement.</param>
        public void PauseMovement(float duration)
        {
            if (!_isMovementPaused)
            {
                StartCoroutine(PauseMovementCoroutine(duration));
            }
        }

        /// <summary>
        /// Coroutine that pauses movement for the given duration.
        /// </summary>
        /// <param name="duration">The pause duration in seconds.</param>
        /// <returns></returns>
        private IEnumerator PauseMovementCoroutine(float duration)
        {
            _isMovementPaused = true;
            _savedMovementSpeed = _movementSpeed; // Store the original speed
            _movementSpeed = 0f; // Set movement speed to 0

            yield return new WaitForSeconds(duration);

            _movementSpeed = _savedMovementSpeed; // Restore the original speed
            _isMovementPaused = false;
        }
    }
}
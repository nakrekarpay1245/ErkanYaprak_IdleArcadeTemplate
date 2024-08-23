using _Game.Scripts.InputHandling;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// This class manages the character's movement and rotation in an idle-arcade style game.
    /// It uses Unity's CharacterController for physics-based movement and 
    /// leverages a PlayerInputSO to handle all input logic.
    /// Implements smooth acceleration, deceleration, and gravity, while dynamically 
    /// adjusting animation.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class TopDownCharacterController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("The maximum speed at which the character moves.")]
        [SerializeField] private float _movementSpeed = 5f;

        [Tooltip("The rate at which the character accelerates and decelerates.")]
        [SerializeField] private float _acceleration = 2f;

        [Tooltip("Minimum speed required for rotation.")]
        [SerializeField] private float _rotationSpeedThreshold = 0.1f;

        [Tooltip("Speed at which the character rotates to face movement direction.")]
        [SerializeField] private float _rotationSpeed = 720f;

        [Tooltip("The gravity force applied to the character.")]
        [SerializeField] private float _gravity = -9.81f;

        [Header("Input Settings")]
        [Tooltip("The scriptable object for handling player input.")]
        [SerializeField] private PlayerInputSO _playerInput;

        private CharacterController _characterController;
        private Vector3 _currentVelocity;
        private Vector3 _verticalVelocity;

        private bool _isMovementPaused = false;
        private bool _isCoroutineRunning = false;
        private float _savedMovementSpeed;

        [HideInInspector] public float Speed;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _playerInput.OnMoveInput.AddListener(HandleMovementInput);
        }

        private void OnDisable()
        {
            _playerInput.OnMoveInput.RemoveListener(HandleMovementInput);
        }

        private void Update()
        {
            if (!_isMovementPaused)
            {
                ApplyGravity();
                UpdateCharacterRotation();
            }
        }

        /// <summary>
        /// Receives the movement input from PlayerInputSO and applies smooth movement logic.
        /// </summary>
        /// <param name="input">The movement input vector (Vector2).</param>
        private void HandleMovementInput(Vector2 input)
        {
            if (_isMovementPaused) return;

            Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
            Vector3 targetVelocity = direction * _movementSpeed;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity,
                _acceleration * Time.deltaTime);
            Speed = _currentVelocity.magnitude / _movementSpeed;

            _characterController.Move(_currentVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Applies gravity to the character, ensuring they remain grounded.
        /// </summary>
        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _verticalVelocity.y < 0)
            {
                _verticalVelocity.y = 0f;
            }

            _verticalVelocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_verticalVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Rotates the character smoothly to face the movement direction.
        /// </summary>
        private void UpdateCharacterRotation()
        {
            if (_currentVelocity.sqrMagnitude >= _rotationSpeedThreshold *
                _rotationSpeedThreshold)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_currentVelocity);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Pauses the character's movement for a specified duration, restoring it afterward.
        /// </summary>
        /// <param name="duration">The time in seconds to pause movement.</param>
        public void PauseMovement(float duration)
        {
            if (!_isCoroutineRunning)
            {
                StartCoroutine(PauseMovementCoroutine(duration));
            }
        }

        /// <summary>
        /// Coroutine that pauses the character's movement for the given duration.
        /// </summary>
        /// <param name="duration">The pause duration in seconds.</param>
        /// <returns></returns>
        private IEnumerator PauseMovementCoroutine(float duration)
        {
            _isCoroutineRunning = true;
            _isMovementPaused = true;
            _savedMovementSpeed = _movementSpeed;
            _movementSpeed = 0f;

            yield return new WaitForSeconds(duration);

            _movementSpeed = _savedMovementSpeed;
            _isMovementPaused = false;
            _isCoroutineRunning = false;
        }
    }
}
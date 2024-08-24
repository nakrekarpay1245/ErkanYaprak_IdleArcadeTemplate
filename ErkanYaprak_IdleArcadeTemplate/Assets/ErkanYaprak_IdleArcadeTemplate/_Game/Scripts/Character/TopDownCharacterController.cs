using _Game.Scripts.InputHandling;
using UnityEngine;
using System.Collections;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// Manages the character's movement and rotation in an idle-arcade style game.
    /// Uses CharacterController for physics-based movement and gets configuration 
    /// parameters from TopDownCharacterConfigSO.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class TopDownCharacterController : MonoBehaviour
    {
        [Header("Input Settings")]
        [Tooltip("ScriptableObject for handling player input.")]
        [SerializeField] private PlayerInputSO _playerInput;

        [Tooltip("ScriptableObject for character configuration.")]
        [SerializeField] private TopDownCharacterConfigSO _characterConfig;

        private CharacterController _characterController;
        private Vector3 _currentVelocity;
        private Vector3 _verticalVelocity;

        private bool _isMovementPaused = false;
        private bool _isCoroutineRunning = false;

        [HideInInspector] public float Speed;

        public TopDownCharacterConfigSO CharacterConfig { get => _characterConfig; set => _characterConfig = value; }
        public PlayerInputSO PlayerInput { get => _playerInput; set => _playerInput = value; }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();

            // Validate that the ScriptableObjects are assigned
            if (_characterConfig == null)
            {
                Debug.LogWarning("TopDownCharacterConfigSO is not assigned in" +
                    " TopDownCharacterController.");
            }

            if (_playerInput == null)
            {
                Debug.LogWarning("PlayerInputSO is not assigned in" +
                    " TopDownCharacterController.");
            }
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
        /// Movement speed and acceleration values are fetched from the config.
        /// </summary>
        /// <param name="input">The movement input vector (Vector2).</param>
        private void HandleMovementInput(Vector2 input)
        {
            if (_isMovementPaused) return;

            Vector3 direction = new Vector3(input.x, 0f, input.y).normalized;
            Vector3 targetVelocity = direction * _characterConfig.MovementSpeed;

            _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity,
                _characterConfig.Acceleration * Time.deltaTime);

            Speed = _currentVelocity.magnitude / _characterConfig.MovementSpeed;
            _characterController.Move(_currentVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Applies gravity to the character, ensuring they remain grounded.
        /// Gravity value is fetched from the config.
        /// </summary>
        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _verticalVelocity.y < 0)
            {
                _verticalVelocity.y = 0f;
            }

            _verticalVelocity.y += _characterConfig.Gravity * Time.deltaTime;
            _characterController.Move(_verticalVelocity * Time.deltaTime);
        }

        /// <summary>
        /// Rotates the character smoothly to face the movement direction.
        /// Rotation speed and threshold values are fetched from the config.
        /// </summary>
        private void UpdateCharacterRotation()
        {
            if (_currentVelocity.sqrMagnitude >=
                _characterConfig.RotationSpeedThreshold * _characterConfig.RotationSpeedThreshold)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_currentVelocity);
                transform.rotation = Quaternion.RotateTowards(transform.rotation,
                    targetRotation, _characterConfig.RotationSpeed * Time.deltaTime);
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
        /// Movement speed is restored from the config after the duration ends.
        /// </summary>
        /// <param name="duration">The pause duration in seconds.</param>
        /// <returns></returns>
        private IEnumerator PauseMovementCoroutine(float duration)
        {
            _isCoroutineRunning = true;
            _isMovementPaused = true;

            yield return new WaitForSeconds(duration);

            _isMovementPaused = false;
            _isCoroutineRunning = false;
        }
    }
}
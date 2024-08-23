using UnityEngine;

namespace _Game.Scripts.InputHandling
{
    /// <summary>
    /// Handles user input and forwards the input data to the PlayerInput scriptable object.
    /// Supports both joystick and keyboard input.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField, Tooltip("Scriptable object for handling player input.")]
        private PlayerInputSO _playerInput;

        [Header("Joystick Reference")]
        [SerializeField, Tooltip("Reference to the joystick for input.")]
        private Joystick _joystick; // Reference to your joystick class

        private void Update()
        {
            Vector2 moveInput = GetMoveInput();
            _playerInput.OnMoveInput.Invoke(moveInput);
        }

        /// <summary>
        /// Retrieves movement input from either the keyboard or joystick.
        /// </summary>
        /// <returns>Vector2 representing movement input.</returns>
        private Vector2 GetMoveInput()
        {
            // Keyboard input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Joystick input (if available)
            if (_joystick != null)
            {
                horizontal += _joystick.Horizontal;
                vertical += _joystick.Vertical;
            }

            return new Vector2(horizontal, vertical);
        }
    }
}
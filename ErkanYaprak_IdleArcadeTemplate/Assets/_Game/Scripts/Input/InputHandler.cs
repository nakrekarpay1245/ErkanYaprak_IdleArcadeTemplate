using UnityEngine;

namespace _Game.Scripts.InputHandling
{
    /// <summary>
    /// Handles user input and forwards the input data to the PlayerInput scriptable object.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField, Tooltip("Scriptable object for handling player input.")]
        private PlayerInput _playerInput;
    }
}
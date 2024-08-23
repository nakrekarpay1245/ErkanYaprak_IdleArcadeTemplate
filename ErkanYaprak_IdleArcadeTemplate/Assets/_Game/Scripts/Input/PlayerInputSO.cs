using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts.InputHandling
{
    /// <summary>
    /// ScriptableObject that stores and handles player input data using events.
    /// Handles input from both joystick and keyboard.
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "Data/PlayerInput")]
    public class PlayerInputSO : ScriptableObject
    {
        public UnityEvent<Vector2> OnMoveInput = new UnityEvent<Vector2>();
    }
}
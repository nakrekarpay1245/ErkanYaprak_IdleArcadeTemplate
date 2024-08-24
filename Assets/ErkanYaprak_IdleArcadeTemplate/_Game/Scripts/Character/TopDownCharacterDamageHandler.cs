using UnityEngine;
using System.Collections;
using _Game.Scripts._Abstracts;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// Handles damage logic for the TopDownCharacter, including taking damage and death sequences.
    /// This class uses values from TopDownCharacterConfigSO to maintain consistency across configurations.
    /// </summary>
    public class TopDownCharacterDamageHandler : AbstractDamageableBase
    {
        [Header("Character Configuration")]
        [Tooltip("ScriptableObject storing all character configuration parameters.")]
        [SerializeField] private TopDownCharacterConfigSO _characterConfig;

        [Tooltip("Reference to the character's controller script.")]
        [SerializeField] private TopDownCharacterController _characterController;

        [Tooltip("Reference to the character's animator script.")]
        [SerializeField] private TopDownCharacterAnimator _characterAnimator;
        public TopDownCharacterConfigSO CharacterConfig { get => _characterConfig; set => _characterConfig = value; }

        private void Awake()
        {
            _characterAnimator = GetComponentInChildren<TopDownCharacterAnimator>();
            _characterController = GetComponent<TopDownCharacterController>();

            // Validate that the ScriptableObject is assigned
            if (_characterConfig == null)
            {
                Debug.LogWarning("TopDownCharacterConfigSO is not assigned in " +
                    "TopDownCharacterDamageHandler.");
            }

            // Validate that the TopDownCharacterController is assigned
            if (_characterController == null)
            {
                Debug.LogWarning("TopDownCharacterController is not assigned in" +
                    " TopDownCharacterDamageHandler.");
            }

            // Validate that the TopDownCharacterAnimator is assigned
            if (_characterAnimator == null)
            {
                Debug.LogWarning("TopDownCharacterAnimator is not assigned in" +
                    " TopDownCharacterDamageHandler.");
            }
        }

        /// <summary>
        /// Apply damage to the character and check for death.
        /// Uses configuration values for stopping movement and triggering animations.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);

            if (!_isDie)
            {
                if (_characterAnimator != null)
                {
                    // Play the hurt animation when damage is taken
                    _characterAnimator.PlayHurtAnimation();
                }
            }

            if (_characterController != null)
            {
                // Pause movement for a configured duration after taking damage
                _characterController.PauseMovement(_characterConfig.StopDurationOnDamage);
            }
        }

        /// <summary>
        /// Triggers the character's death sequence using configuration for animations and wait times.
        /// </summary>
        public override void Die()
        {
            base.Die();
            StartCoroutine(DeathSequence());
        }

        /// <summary>
        /// The death sequence disables movement, plays the death animation,
        /// and waits before disabling the character, based on configuration settings.
        /// </summary>
        private IEnumerator DeathSequence()
        {
            if (_characterAnimator != null)
            {
                // Play the death animation
                _characterAnimator.PlayDeadAnimation();
            }

            if (_characterController != null)
            {
                // Disable the character controller to prevent any movement or interaction
                _characterController.enabled = false;
            }

            // Wait for a configured duration before continuing the death process
            yield return new WaitForSeconds(_characterConfig.DeathWaitTime);

            // Optionally: Respawn character or trigger game over logic here
            // For now, the character is disabled after the wait period
            gameObject.SetActive(false);
        }
    }
}
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
        [SerializeField] public TopDownCharacterConfigSO _characterConfig;

        private TopDownCharacterAnimator _characterAnimator;
        private TopDownCharacterController _characterController;

        private void Awake()
        {
            _characterAnimator = GetComponentInChildren<TopDownCharacterAnimator>();
            _characterController = GetComponent<TopDownCharacterController>();

            // Validate that the ScriptableObject is assigned
            if (_characterConfig == null)
            {
                Debug.LogError("TopDownCharacterConfigSO is not assigned in TopDownCharacterDamageHandler.");
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
                // Play the hurt animation when damage is taken
                _characterAnimator.PlayHurtAnimation();
            }

            // Pause movement for a configured duration after taking damage
            _characterController.PauseMovement(_characterConfig.StopDurationOnDamage);
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
            // Play the death animation
            _characterAnimator.PlayDeadAnimation();

            // Disable the character controller to prevent any movement or interaction
            _characterController.enabled = false;

            // Wait for a configured duration before continuing the death process
            yield return new WaitForSeconds(_characterConfig.DeathWaitTime);

            // Optionally: Respawn character or trigger game over logic here
            // For now, the character is disabled after the wait period
            gameObject.SetActive(false);
        }
    }
}
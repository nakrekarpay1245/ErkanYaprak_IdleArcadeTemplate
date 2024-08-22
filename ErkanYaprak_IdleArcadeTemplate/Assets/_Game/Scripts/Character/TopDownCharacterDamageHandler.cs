using UnityEngine;
using System.Collections;
using _Game.Scripts._Abstracts;

namespace _Game.Scripts.TopDownCharacter
{
    /// <summary>
    /// Handles damage logic for the TopDownCharacter, including taking damage and death sequences.
    /// </summary>
    public class TopDownCharacterDamageHandler : AbstractDamageableBase
    {
        [Header("Damage Settings")]
        [Tooltip("Duration of pause in movement after taking damage.")]
        [SerializeField, Range(0.1f, 5f)] private float _stopDuration = 2f;

        [SerializeField, Tooltip("How long to wait before character respawns or game ends.")]
        private float _deathWaitTime = 2f;

        private TopDownCharacterAnimator _characterAnimator;
        private TopDownCharacterController _characterController;

        private void Awake()
        {
            _characterAnimator = GetComponentInChildren<TopDownCharacterAnimator>();
            _characterController = GetComponent<TopDownCharacterController>();
        }

        /// <summary>
        /// Apply damage to the character and check for death.
        /// </summary>
        /// <param name="amount">The amount of damage to apply.</param>
        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            if (!_isDie)
            {
                _characterAnimator.PlayHurtAnimation();
            }
            // Pause movement briefly upon taking damage
            _characterController.PauseMovement(_stopDuration);
        }

        /// <summary>
        /// Triggers the character's death sequence.
        /// </summary>
        public override void Die()
        {
            base.Die();
            StartCoroutine(DeathSequence());
        }

        /// <summary>
        /// The death sequence, which disables the character and waits before respawning or ending the game.
        /// </summary>
        private IEnumerator DeathSequence()
        {
            // Trigger death animation
            _characterAnimator.PlayDeadAnimation();

            // Disable movement or any other interaction
            _characterController.enabled = false;

            // Wait for a defined period
            yield return new WaitForSeconds(_deathWaitTime);

            // Optionally: Reset character or trigger game over logic
            // Respawn or game end logic can go here

            // Reactivate or handle accordingly
            gameObject.SetActive(false);  // Example: Disable the character after death
        }

        /// <summary>
        /// Draws Gizmos to visualize the character's health.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // Example Gizmo to show current health in editor
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
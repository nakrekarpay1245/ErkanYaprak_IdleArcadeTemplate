using _Game.Scripts._Abstracts;
using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Test
{

    /// <summary>
    /// Represents a damageable object that triggers a shake animation when damaged and a shrink-then-grow animation upon death.
    /// </summary>
    public class TestDamageable : AbstractDamageableBase
    {
        [Header("Model Reference")]
        [Tooltip("The model to apply shake and death animations on.")]
        [SerializeField] private GameObject _model;

        [Header("Shake Animation Settings")]
        [Tooltip("The duration of the shake animation.")]
        [Range(0.1f, 2f)]
        [SerializeField] private float shakeDuration = 0.5f;

        [Tooltip("The intensity of the shake rotation.")]
        [Range(1f, 20f)]
        [SerializeField] private float shakeRotationIntensity = 10f;

        [Tooltip("The scale multiplier applied during the shake.")]
        [Range(0.8f, 1.2f)]
        [SerializeField] private float shakeScaleMultiplier = 0.9f;

        [Header("Death Animation Settings")]
        [Tooltip("The duration of the scale down/up effect upon death.")]
        [Range(0.1f, 3f)]
        [SerializeField] private float deathAnimationDuration = 1f;
        [SerializeField] private float _waitAfterBorn = 5f;

        private Coroutine _shakeCoroutine;
        private Coroutine _deathCoroutine;

        /// <summary>
        /// Applies damage to the object and triggers the shake animation.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to apply.</param>
        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
            StartShakeAnimation();
        }

        /// <summary>
        /// Handles the death animation by scaling the model down and up, then resets health.
        /// </summary>
        public override void Die()
        {
            base.Die();
            StartDeathAnimation();
        }

        /// <summary>
        /// Starts the shake animation on the model. Ensures that multiple coroutines do not run simultaneously.
        /// </summary>
        private void StartShakeAnimation()
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }
            _shakeCoroutine = StartCoroutine(PlayShakeAnimation());
        }

        /// <summary>
        /// Plays the shake animation over a given duration by modifying the model's rotation and scale.
        /// </summary>
        /// <returns>An IEnumerator to be used by the coroutine system.</returns>
        private IEnumerator PlayShakeAnimation()
        {
            Vector3 originalPosition = _model.transform.localPosition;
            Quaternion originalRotation = _model.transform.localRotation;
            Vector3 originalScale = _model.transform.localScale;

            float elapsedTime = 0f;

            while (elapsedTime < shakeDuration)
            {
                float progress = elapsedTime / shakeDuration;

                // Apply rotation shake
                float rotationShake = Mathf.Sin(progress * Mathf.PI * shakeRotationIntensity);
                _model.transform.localRotation = originalRotation * Quaternion.Euler(rotationShake, rotationShake, rotationShake);

                // Apply scale shake
                float scaleShake = Mathf.Lerp(1f, shakeScaleMultiplier, Mathf.PingPong(progress * 2, 1));
                _model.transform.localScale = originalScale * scaleShake;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Reset model to original state
            _model.transform.localPosition = originalPosition;
            _model.transform.localRotation = originalRotation;
            _model.transform.localScale = originalScale;

            _shakeCoroutine = null;
        }

        /// <summary>
        /// Starts the death animation on the model. Ensures that only one death animation runs at a time.
        /// </summary>
        private void StartDeathAnimation()
        {
            if (_deathCoroutine != null)
            {
                StopCoroutine(_deathCoroutine);
            }
            _deathCoroutine = StartCoroutine(PlayDeathAnimation());
        }

        /// <summary>
        /// Plays the death animation by scaling the model down to zero and then scaling it back up.
        /// Calls SetHealth after the animation completes.
        /// </summary>
        /// <returns>An IEnumerator to be used by the coroutine system.</returns>
        private IEnumerator PlayDeathAnimation()
        {
            Vector3 originalScale = transform.localScale;

            // Scale down
            float elapsedTime = 0f;
            while (elapsedTime < deathAnimationDuration / 2)
            {
                transform.localScale = Vector3.Lerp(originalScale,
                    Vector3.zero, elapsedTime / (deathAnimationDuration / 2));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = Vector3.zero;

            yield return new WaitForSeconds(_waitAfterBorn);

            // Reset health or perform other logic
            SetHealth();
            _isDie = false;

            // Scale back up
            elapsedTime = 0f;
            while (elapsedTime < deathAnimationDuration / 2)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, originalScale,
                    elapsedTime / (deathAnimationDuration / 2));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = originalScale;

            _deathCoroutine = null;
        }
    }
}
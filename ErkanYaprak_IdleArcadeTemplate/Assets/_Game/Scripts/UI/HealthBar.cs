using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using _Game.Scripts._Abstracts;

namespace _Game.Scripts.UI
{
    /// <summary>
    /// Manages the display of the health bar UI element based on the health status of a damageable parent.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Header("Health Bar Parameters")]
        [Header("Reference")]
        [SerializeField, Tooltip("Reference to the parent damageable object.")]
        private AbstractDamageableBase _parentDamageable;

        [Header("UI")]
        [SerializeField, Tooltip("The UI Image component that represents the health bar fill.")]
        private Image _healthBarFill;

        [SerializeField, Tooltip("Duration of the smooth fill transition.")]
        private float _fillTransitionDuration = 0.5f;

        private float _currentFillAmount;
        private Coroutine _currentFillCoroutine;

        private void Awake()
        {
            // Ensure the parent damageable reference is assigned
            if (_parentDamageable == null)
            {
                _parentDamageable = GetComponentInParent<AbstractDamageableBase>();
            }

            if (_parentDamageable != null)
            {
                _parentDamageable.OnHealthChanged += UpdateHealthBar;
            }
            else
            {
                Debug.LogError("Parent damageable reference is missing.", this);
            }
        }

        /// <summary>
        /// Smoothly updates the health bar fill amount.
        /// </summary>
        /// <param name="health">Current health value.</param>
        /// <param name="maxHealth">Maximum health value.</param>
        private void UpdateHealthBar(float health, float maxHealth)
        {
            float targetFillAmount = health / maxHealth;

            // Stop any ongoing fill animation before starting a new one
            if (_currentFillCoroutine != null)
            {
                StopCoroutine(_currentFillCoroutine);
            }

            _currentFillCoroutine = StartCoroutine(SmoothFill(targetFillAmount));
        }

        /// <summary>
        /// Smoothly animates the health bar fill amount.
        /// </summary>
        /// <param name="targetFillAmount">The target fill amount to reach.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator SmoothFill(float targetFillAmount)
        {
            float elapsedTime = 0f;
            float startFillAmount = _currentFillAmount;

            while (elapsedTime < _fillTransitionDuration)
            {
                _currentFillAmount = Mathf.Lerp(startFillAmount, targetFillAmount,
                    elapsedTime / _fillTransitionDuration);
                _healthBarFill.fillAmount = _currentFillAmount;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _currentFillAmount = targetFillAmount;
            _healthBarFill.fillAmount = _currentFillAmount;

            // Clear the reference to the running coroutine
            _currentFillCoroutine = null;
        }
    }
}
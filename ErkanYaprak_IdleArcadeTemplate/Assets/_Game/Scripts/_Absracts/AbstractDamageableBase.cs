using _Game.Scripts._Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Scripts._Abstracts
{
    /// <summary>
    /// Abstract base class for objects that have health, can take damage, and can die.
    /// Inherits from IDamageable to implement damage and health management functionalities.
    /// </summary>
    public abstract class AbstractDamageableBase : MonoBehaviour, IDamageable
    {
        [Header("Health Settings")]
        [Tooltip("Current health of the object.")]
        [SerializeField] private float _health = 5f;

        [Tooltip("Maximum health of the object.")]
        [SerializeField] private float _maxHealth = 5f;

        protected bool _isDie = false;

        public UnityAction<float, float> OnHealthChanged;

        public virtual void Start()
        {
            SetHealth();
        }

        protected virtual void SetHealth()
        {
            _health = _maxHealth;
            RaiseHealthChangedEvent(_health, _maxHealth);
        }

        /// <summary>
        /// Applies damage to the object and checks if it should die.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to apply.</param>
        public virtual void TakeDamage(float damageAmount)
        {
            _health -= damageAmount;

            //Debug.Log("Taked Damage: " + damageAmount + "/// my health is: " + _health);

            if (_health <= 0 && !_isDie)
            {
                Die();
            }

            OnHealthChanged?.Invoke(_health, _maxHealth);
        }

        public virtual void Die()
        {
            _isDie = true;
        }

        protected virtual void RaiseHealthChangedEvent(float health, float maxHealth)
        {
            OnHealthChanged?.Invoke(health, maxHealth);
        }


        /// <summary>
        /// Heals the object by a certain amount.
        /// </summary>
        /// <param name="healAmount">The amount of health to restore.</param>
        public void Heal(float healAmount)
        {
            _health += healAmount;
            if (_health > _maxHealth)
            {
                _health = _maxHealth; // Ensure health does not exceed max health
                OnHealthChanged?.Invoke(_health, _maxHealth);
            }
        }
    }
}
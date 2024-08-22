using UnityEngine;
using _Game.Scripts._Interfaces;

namespace _Game.Scripts._Abstracts
{
    public abstract class AbstractDamagerBase : MonoBehaviour, IDamager
    {
        [Header("Damager Settings")]
        [Tooltip("Damage dealt by this damager.")]
        [SerializeField] protected float _damageAmount;

        [Tooltip("Range of the attack.")]
        [SerializeField] protected float _attackRange;

        /// <summary>
        /// Deals damage to a target that implements IDamageable.
        /// </summary>
        /// <param name="target">The target to damage.</param>
        /// <param name="damageAmount">The amount of damage to deal.</param>
        public abstract void DealDamage(IDamageable target, float damageAmount);

        /// <summary>
        /// Executes the attack, applying damage to all applicable targets.
        /// This method will handle the attack process for derived classes.
        /// </summary>
        public abstract void ExecuteAttack();

        /// <summary>
        /// Draws gizmos in the editor to visualize the attack range.
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
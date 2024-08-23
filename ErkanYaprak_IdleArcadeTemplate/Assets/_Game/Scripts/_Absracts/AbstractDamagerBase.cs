using UnityEngine;
using _Game.Scripts._Interfaces;

namespace _Game.Scripts._Abstracts
{
    public abstract class AbstractDamagerBase : MonoBehaviour, IDamager
    {
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
    }
}
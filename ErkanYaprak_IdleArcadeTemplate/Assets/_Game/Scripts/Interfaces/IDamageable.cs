/// <summary>
/// Represents an object that can take damage.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Apply damage to the object.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    void TakeDamage(float amount);
}
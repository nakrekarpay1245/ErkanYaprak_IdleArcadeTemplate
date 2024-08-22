/// <summary>
/// Represents an object that can be collected.
/// </summary>
public interface ICollectable
{
    /// <summary>
    /// Called when the object is collected.
    /// </summary>
    void Collect();
}
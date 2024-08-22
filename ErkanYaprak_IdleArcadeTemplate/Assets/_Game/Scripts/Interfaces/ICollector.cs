/// <summary>
/// Represents an object that can collect items.
/// </summary>
public interface ICollector
{
    /// <summary>
    /// Called when a collection action occurs.
    /// </summary>
    /// <param name="collectable">The collectable object.</param>
    void Collect(ICollectable collectable);
}
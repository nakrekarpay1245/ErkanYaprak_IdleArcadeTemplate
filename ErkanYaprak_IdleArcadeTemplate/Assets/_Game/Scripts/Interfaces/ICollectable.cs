using UnityEngine;

/// <summary>
/// Represents an object that can be collected.
/// </summary>
public interface ICollectable
{
    /// <summary>
    /// Called when the object is collected.
    /// </summary>
    /// <param name="collector">The object collecting this item as a ICollector.</param>
    void OnCollect(ICollector collector);
}
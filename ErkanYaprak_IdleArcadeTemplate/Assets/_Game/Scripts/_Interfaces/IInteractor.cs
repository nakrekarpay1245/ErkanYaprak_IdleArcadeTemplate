namespace _Game.Scripts._Interfaces
{
    /// <summary>
    /// Represents an object that can interact with interactables.
    /// </summary>
    public interface IInteractor
    {
        /// <summary>
        /// Triggers interaction with an interactable object.
        /// </summary>
        /// <param name="interactable">The interactable object.</param>
        void InteractWith(IInteractable interactable);
    }
}
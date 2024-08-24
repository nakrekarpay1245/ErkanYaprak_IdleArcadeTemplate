namespace _Game.Scripts._Interfaces
{
    /// <summary>
    /// Represents an object that can interact with interactables.
    /// </summary>
    public interface IInteractor
    {
        /// <summary>
        /// Starts interaction with the specified interactable object.
        /// </summary>
        /// <param name="interactable">The interactable object.</param>
        void StartInteraction(IInteractable interactable);

        /// <summary>
        /// Completes the interaction with the specified interactable object.
        /// </summary>
        /// <param name="interactable">The interactable object.</param>
        void CompleteInteraction(IInteractable interactable);
    }
}
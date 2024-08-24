namespace _Game.Scripts._Interfaces
{
    /// <summary>
    /// Represents an object that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Starts interaction with the object.
        /// </summary>
        void StartInteraction();

        /// <summary>
        /// Completes interaction with the object.
        /// </summary>
        void CompleteInteraction();

        /// <summary>
        /// Cancels interaction with the object.
        /// </summary>
        void CancelInteraction();
    }
}
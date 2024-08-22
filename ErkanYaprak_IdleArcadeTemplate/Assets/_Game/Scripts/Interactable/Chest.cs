using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using _Game.Scripts._Abstracts;

namespace _Game.Scripts.Interactable
{
    /// <summary>
    /// Represents a chest that can be interacted with. Upon interaction, it performs a 
    /// smooth opening animation and displays a UI progress bar.
    /// </summary>
    public class Chest : AbstractInteractableBase
    {
        [Header("Chest Settings")]
        [Tooltip("The transform of the chest's lid.")]
        [SerializeField]
        private Transform _lidTransform;

        [Tooltip("The duration of the lid opening animation.")]
        [SerializeField, Range(0.1f, 5f)]
        private float _lidAnimationDuration = 1f;

        [Tooltip("The distance the lid moves along the Z-axis.")]
        [SerializeField]
        private float _lidMoveDistanceZ = 1f;

        [Tooltip("The distance the lid moves along the Y-axis.")]
        [SerializeField]
        private float _lidMoveDistanceY = 0.5f;

        [Tooltip("The angle the lid rotates around the X-axis.")]
        [SerializeField, Range(0f, 90f)]
        private float _lidRotationAngleX = 75f;

        [Header("Progress Bar Settings")]
        [Tooltip("The UI Image component representing the fill amount of the progress bar.")]
        [SerializeField]
        private Image _progressBarFill;
        [Tooltip("The UI Image component representing the entire progress bar.")]
        [SerializeField]
        private Image _progressBar;

        private Vector3 _initialLidPosition;
        private Quaternion _initialLidRotation;
        private Coroutine _interactionCoroutine;

        private void Start()
        {
            if (_lidTransform == null)
            {
                Debug.LogError("Lid transform is not assigned.");
                return;
            }

            // Save the initial position and rotation of the lid
            _initialLidPosition = _lidTransform.localPosition;
            _initialLidRotation = _lidTransform.localRotation;

            _progressBar.gameObject.SetActive(false);

            // Initialize the progress bar
            ResetProgressBar();
        }

        /// <summary>
        /// Begins the interaction and starts updating the progress bar.
        /// </summary>
        public override void StartInteraction()
        {
            base.StartInteraction();

            _progressBar.gameObject.SetActive(true);

            // Start progress bar and animation coroutines
            if (_interactionCoroutine == null)
            {
                _interactionCoroutine = StartCoroutine(UpdateProgressBar());
            }
        }

        /// <summary>
        /// Completes the interaction and triggers the chest's lid opening animation.
        /// </summary>
        public override void CompleteInteraction()
        {
            base.CompleteInteraction();

            _progressBar.gameObject.SetActive(false);

            // Start lid animation if active
            if (gameObject.activeInHierarchy && _lidTransform != null)
            {
                StartCoroutine(AnimateLidOpen());
            }

            StopAndResetInteraction();
        }

        /// <summary>
        /// Cancels the interaction and resets the progress bar.
        /// </summary>
        public override void CancelInteraction()
        {
            base.CancelInteraction();

            _progressBar.gameObject.SetActive(false);

            ResetProgressBar();
            StopAndResetInteraction();
        }

        /// <summary>
        /// Coroutine to smoothly update the progress bar as the chest opens.
        /// </summary>
        private IEnumerator UpdateProgressBar()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _interactionDuration)
            {
                float progress = elapsedTime / _interactionDuration;

                if (_progressBarFill != null)
                {
                    _progressBarFill.fillAmount = progress;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the progress bar is fully filled when the interaction completes
            if (_progressBarFill != null)
            {
                _progressBarFill.fillAmount = 1f;
            }

            CompleteInteraction();
        }

        /// <summary>
        /// Coroutine to smoothly animate the chest lid opening.
        /// </summary>
        private IEnumerator AnimateLidOpen()
        {
            float elapsedTime = 0f;
            Vector3 targetPosition = _initialLidPosition + new Vector3(0f, _lidMoveDistanceY, _lidMoveDistanceZ);
            Quaternion targetRotation = Quaternion.Euler(_lidRotationAngleX, 0f, 0f);

            while (elapsedTime < _lidAnimationDuration)
            {
                float t = elapsedTime / _lidAnimationDuration;
                _lidTransform.localPosition = Vector3.Lerp(_initialLidPosition, targetPosition, t);
                _lidTransform.localRotation = Quaternion.Lerp(_initialLidRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure final position and rotation of the lid
            _lidTransform.localPosition = targetPosition;
            _lidTransform.localRotation = targetRotation;
        }

        /// <summary>
        /// Resets the progress bar to zero.
        /// </summary>
        private void ResetProgressBar()
        {
            if (_progressBarFill != null)
            {
                _progressBarFill.fillAmount = 0f;
            }
        }

        /// <summary>
        /// Stops the interaction coroutine and resets it to null.
        /// </summary>
        private void StopAndResetInteraction()
        {
            if (_interactionCoroutine != null)
            {
                StopCoroutine(_interactionCoroutine);
                _interactionCoroutine = null;
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws gizmos in the editor to visualize the chest's interaction range.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            if (TryGetComponent(out SphereCollider sphereCollider))
            {
                Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
            }
        }
#endif
    }
}
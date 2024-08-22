using UnityEngine;
using System.Collections;
using _Game.Scripts._Abstracts;

namespace _Game.Scripts.Interactable
{
    /// <summary>
    /// Represents a chest that can be interacted with. Upon interaction, it performs a 
    /// smooth opening animation.
    /// </summary>
    public class Chest : AbstractInteractableBase
    {
        [Header("Chest Settings")]
        [Tooltip("The lid transform of the chest.")]
        [SerializeField]
        private Transform _lidTransform;

        [Tooltip("The duration for the lid opening animation.")]
        [SerializeField, Range(0.1f, 5f)]
        private float _animationDuration = 1f;

        [Tooltip("The distance the lid will move along the Z-axis.")]
        [SerializeField]
        private float _lidMoveDistanceZ = 1f;

        [Tooltip("The distance the lid will move along the Y-axis.")]
        [SerializeField]
        private float _lidMoveDistanceY = 0.5f;

        [Tooltip("The angle the lid will rotate along the X-axis.")]
        [SerializeField, Range(0f, 90f)]
        private float _lidRotationAngleX = 75f;

        private Vector3 _initialLidPosition;
        private Quaternion _initialLidRotation;

        private void Start()
        {
            if (_lidTransform != null)
            {
                _initialLidPosition = _lidTransform.localPosition;
                _initialLidRotation = _lidTransform.localRotation;
            }
            else
            {
                Debug.LogError("Lid transform is not assigned.");
            }
        }

        public override void CompleteInteraction()
        {
            base.CompleteInteraction();
            // Check if the GameObject is active before starting the coroutine
            if (gameObject.activeInHierarchy && _lidTransform != null)
            {
                StartCoroutine(AnimateLidOpen());
            }
        }

        private IEnumerator AnimateLidOpen()
        {
            float elapsedTime = 0f;
            Vector3 targetPosition = _initialLidPosition + new Vector3(0f, _lidMoveDistanceY, _lidMoveDistanceZ);
            Quaternion targetRotation = Quaternion.Euler(_lidRotationAngleX, 0f, 0f);

            while (elapsedTime < _animationDuration)
            {
                float t = elapsedTime / _animationDuration;
                _lidTransform.localPosition = Vector3.Lerp(_initialLidPosition, targetPosition, t);
                _lidTransform.localRotation = Quaternion.Lerp(_initialLidRotation, targetRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure final values
            _lidTransform.localPosition = targetPosition;
            _lidTransform.localRotation = targetRotation;
        }
    }
}
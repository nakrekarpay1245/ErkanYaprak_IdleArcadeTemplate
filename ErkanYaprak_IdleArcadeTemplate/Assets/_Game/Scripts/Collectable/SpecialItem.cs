using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace _Game.Scripts.Collectable
{
    /// <summary>
    /// Represents a special one-time collectible item with unique behaviors.
    /// </summary>
    public class SpecialCollectible : AbstractBaseCollectible
    {
        [Header("Special Settings")]
        [SerializeField, Tooltip("Special effect triggered when collected.")]
        private UnityEvent _specialEvent;

        [SerializeField]
        private float _waitTopOfThePlayerTime = 0.5f;
        public override void OnCollect(ICollector collector)
        {
            base.OnCollect(collector);

            // Trigger any special effect specific to this collectible
            _specialEvent?.Invoke();
        }

        protected override IEnumerator CollectAnimation(Transform collector)
        {
            // Scale up
            float elapsedTime = 0f;
            Vector3 initialScale = transform.localScale;
            Vector3 targetScale = initialScale * _scaleFactor;

            while (elapsedTime < _animationDuration / 3f)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale,
                    elapsedTime / (_animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = targetScale;

            // Wait
            yield return new WaitForSeconds(_waitTime);

            // Scale normalization and Move to collector.position + Vector3.up
            elapsedTime = 0f;
            Vector3 normalScale = initialScale;

            Vector3 targetPositionUp = collector.position + Vector3.up;
            Vector3 initialPosition = transform.position;

            while (elapsedTime < _animationDuration / 3f)
            {
                transform.localScale = Vector3.Lerp(targetScale, normalScale,
                    elapsedTime / (_animationDuration / 3f));
                transform.position = Vector3.Lerp(initialPosition, targetPositionUp,
                    elapsedTime / (_animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPositionUp;

            // Wait
            yield return new WaitForSeconds(_waitTopOfThePlayerTime);

            // Move to collector's position
            elapsedTime = 0f;
            Vector3 downScale = Vector3.zero;
            Vector3 downPosition = collector.position;

            while (elapsedTime < _animationDuration / 3f)
            {
                transform.localScale = Vector3.Lerp(targetScale, downScale,
                    elapsedTime / (_animationDuration / 3f));
                transform.position = Vector3.Lerp(targetPositionUp, downPosition,
                    elapsedTime / (_animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localScale = downScale;
            transform.position = downPosition;

            // Deactivate the collectible object after animation
            gameObject.SetActive(false);
        }
    }
}
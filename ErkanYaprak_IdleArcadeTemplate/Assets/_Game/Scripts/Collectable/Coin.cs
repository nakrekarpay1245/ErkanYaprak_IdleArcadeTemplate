using UnityEngine;
using System.Collections;
using _Game.Scripts._Abstracts;

namespace _Game.Scripts.Collectable
{
    /// <summary>
    /// Represents a collectible coin item in the game.
    /// Inherits from BaseCollectible.
    /// </summary>
    public class Coin : AbstractCollectibleBase
    {
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

            // Move up
            elapsedTime = 0f;
            Vector3 initialPosition = transform.position;

            while (elapsedTime < _animationDuration / 3f)
            {
                transform.position = Vector3.Lerp(initialPosition, _targetPosition,
                    elapsedTime / (_animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = _targetPosition;

            // Wait
            yield return new WaitForSeconds(_waitTime);

            // Scale down and move towards the collector's position
            elapsedTime = 0f;
            Vector3 downScale = initialScale;
            Vector3 downPosition = collector.position;

            while (elapsedTime < _animationDuration / 3f)
            {
                transform.localScale = Vector3.Lerp(targetScale, downScale,
                    elapsedTime / (_animationDuration / 3f));
                transform.position = Vector3.Lerp(_targetPosition, downPosition,
                    elapsedTime / (_animationDuration / 3f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = downScale;
            transform.position = downPosition;

            // Deactivate the coin object after animation
            gameObject.SetActive(false);
        }
    }
}
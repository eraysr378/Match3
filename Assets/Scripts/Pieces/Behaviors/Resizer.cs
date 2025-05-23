using System;
using System.Collections;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class Resizer : MonoBehaviour
    {
        [SerializeField] private float defaultShrinkDuration = 0.2f;
        public void ShrinkToZero(Action onComplete = null)
        {
            Resize(Vector3.zero, defaultShrinkDuration, onComplete);
        }
        public void Resize(Vector3 targetScale, float duration, Action onComplete = null)
        {
            StartCoroutine(ResizeCoroutine(transform, targetScale, duration, onComplete));
        }

        private IEnumerator ResizeCoroutine(Transform target, Vector3 targetScale, float duration,
            Action onComplete = null)
        {
            Vector3 originalScale = target.localScale;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                target.localScale = Vector3.Lerp(originalScale, targetScale, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            target.localScale = targetScale;
            onComplete?.Invoke();
        }

        public void ResetScale()
        {
            transform.localScale = Vector3.one;
        }
    }
}
using System;
using System.Collections;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class Resizer : MonoBehaviour
    {
        public void Resize(Transform target, Vector3 targetScale, float duration, Action onComplete = null)
        {
            StartCoroutine(ResizeCoroutine(target, targetScale, duration, onComplete));
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

        public void ResetScale(Transform target)
        {
            target.localScale = Vector3.one;
        }
    }
}
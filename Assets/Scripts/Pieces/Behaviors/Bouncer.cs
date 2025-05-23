using System;
using System.Collections;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class Bouncer : MonoBehaviour
    {
        private float _bounceHeight = 0.1f;
        private float _bounceDuration = 0.2f;
        private Vector3 _originalLocalPos;

        private Coroutine _bounceRoutine;

        private void Start()
        {
            _originalLocalPos = transform.localPosition;
        }

        public void CancelBounce()
        {
            if (_bounceRoutine != null)
                StopCoroutine(_bounceRoutine);
        }

        public void Bounce(Action onComplete = null)
        {
            if (_bounceRoutine != null)
                StopCoroutine(_bounceRoutine);

            _bounceRoutine = StartCoroutine(BounceRoutine(onComplete));
        }

        private IEnumerator BounceRoutine(Action onComplete)
        {
            float halfDuration = _bounceDuration / 2f;
            float elapsed = 0f;
            Vector3 targetUp = _originalLocalPos + Vector3.up * _bounceHeight;

            // Move up
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                transform.localPosition = Vector3.Lerp(_originalLocalPos, targetUp, t);
                yield return null;
            }

            elapsed = 0f;

            // Move back down
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                transform.localPosition = Vector3.Lerp(targetUp, _originalLocalPos, t);
                yield return null;
            }

            transform.localPosition = _originalLocalPos;
            _bounceRoutine = null;
            onComplete?.Invoke();
        }
    }
}
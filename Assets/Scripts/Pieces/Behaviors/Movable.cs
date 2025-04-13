using System;
using System.Collections;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class Movable : MonoBehaviour
    {
        private Coroutine _moveCoroutine;

        public void StartMovingWithDuration(Vector3 targetPosition, float duration, Action onComplete = null)
        {
            CancelMove();
            _moveCoroutine = StartCoroutine(MoveToPositionDurationBasedIE(targetPosition, duration, onComplete));
        }

        public void StartMovingWithSpeed(Vector3 targetPosition, float speed, Action onComplete = null)
        {
            CancelMove();
            _moveCoroutine = StartCoroutine(MoveToPositionSpeedBasedIE(targetPosition, speed, onComplete));
        }

        private void CancelMove()
        {
            if (_moveCoroutine == null) return;

            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }

        private IEnumerator MoveToPositionDurationBasedIE(Vector3 targetPosition, float duration, Action onComplete)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            _moveCoroutine = null;
            onComplete?.Invoke();
        }

        private IEnumerator MoveToPositionSpeedBasedIE(Vector3 targetPosition, float speed, Action onComplete)
        {
            Vector3 startPosition = transform.position;
            float distance = Vector2.Distance(startPosition, targetPosition);
            float duration = distance / speed;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            _moveCoroutine = null;
            onComplete?.Invoke();
        }
    }
}
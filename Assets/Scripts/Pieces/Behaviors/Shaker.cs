using DG.Tweening;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class Shaker : MonoBehaviour
    {
        [SerializeField] private float shakeStrength = 0.075f;
        [SerializeField] private int vibrato = 12;

        private Tween _shakeTween;
        private Vector3 _initialPosition;

        public void Shake()
        {
            if (_shakeTween != null && _shakeTween.IsPlaying())
                return;
            _initialPosition = transform.position;
            _shakeTween = transform.DOShakePosition(
                    duration: 1,
                    strength: shakeStrength,
                    vibrato: vibrato,
                    randomness: 90,
                    snapping: false,
                    fadeOut: false
                )
                .SetLoops(-1, LoopType.Restart) // infinite loop
                .SetEase(Ease.Linear);
        }

        public void Stop()
        {
            _shakeTween?.Kill();
            transform.position = _initialPosition;
        }

        private void OnDisable()
        {
            _shakeTween?.Kill();
        }
    }
}
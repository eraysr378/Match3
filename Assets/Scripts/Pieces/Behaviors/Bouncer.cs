using System;
using DG.Tweening;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class Bouncer : MonoBehaviour
    {
         private float bounceHeight = 0.1f;
         private float bounceDuration = 0.4f;
         private float squashFactor = 0.1f;

        private Vector3 _originalLocalPos;
        private Vector3 _originalScale;
        private Tween _bounceTween;
        private Tween _scaleTween;

        private void Start()
        {
            DOTween.SetTweensCapacity(tweenersCapacity: 500, sequencesCapacity: 100);
            _originalLocalPos = transform.localPosition;
            _originalScale = transform.localScale;
        }
        private void OnDisable()
        {
            CancelBounce();
        }
        public void CancelBounce()
        {
            _bounceTween?.Kill();
            _scaleTween?.Kill();
            transform.localPosition = _originalLocalPos;
            transform.localScale = _originalScale;
        }

        public void Bounce(Action onComplete = null)
        {
            _bounceTween?.Kill();
            _scaleTween?.Kill();

            float fourth = bounceDuration / 4f;
            Vector3 downPos = _originalLocalPos - Vector3.up * bounceHeight;
            Vector3 upPos = _originalLocalPos + Vector3.up * (bounceHeight / 2f);

            Vector3 squashedScale = new Vector3(
                _originalScale.x + squashFactor,
                _originalScale.y - squashFactor,
                _originalScale.z + squashFactor
            );

            Vector3 stretchedScale = new Vector3(
                _originalScale.x - squashFactor,
                _originalScale.y + squashFactor,
                _originalScale.z - squashFactor
            );

            // Squash down
            _scaleTween = transform.DOScale(squashedScale, fourth).SetEase(Ease.OutQuad);
            _bounceTween = transform.DOLocalMove(downPos, fourth).SetEase(Ease.InQuad).OnComplete(() =>
            {
                // Stretch up
                _scaleTween = transform.DOScale(stretchedScale, 2 * fourth).SetEase(Ease.OutQuad);
                _bounceTween = transform.DOLocalMove(upPos, 2 * fourth).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // Return to normal
                    _scaleTween = transform.DOScale(_originalScale, fourth).SetEase(Ease.InOutQuad);
                    _bounceTween = transform.DOLocalMove(_originalLocalPos, fourth).SetEase(Ease.InOutQuad).OnComplete(() =>
                    {
                        _bounceTween = null;
                        _scaleTween = null;
                        onComplete?.Invoke();
                    });
                });
            });
        }
    }
}

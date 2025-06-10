using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Projectiles
{
    public class WoodProjectile : MonoBehaviour
    {
        [SerializeField] private float arcHeightMax = 1.5f;
        [SerializeField] private float arcHeightMin = 1f;
        
        [SerializeField] private float forwardDistance = 1.25f;
        
        [SerializeField] private float minFallDistance = 5f;
        [SerializeField] private float maxFallDistance = 10;
        
        [SerializeField] private float maxUpDuration = 0.35f;
        [SerializeField] private float minUpDuration = 0.25f;
        [SerializeField] private float maxDownDuration = 0.75f;
        [SerializeField] private float minDownDuration = 0.5f;
        
        [SerializeField] private float maxSpinSpeed = 720f;
        [SerializeField] private float minSpinSpeed = 1080f; 
        [SerializeField] private Vector3 scaleUpTarget = new Vector3(1.2f, 1.2f, 1.2f); // desired scale
        [SerializeField] private float scaleUpDuration = 0.3f;
        private float _fallDistance;
        private float _arcHeight;
        private float _upDuration;
        private float _downDuration;
        private float _spinSpeed;
        private void Start()
        {
            // Choose move direction randomly right or left
            forwardDistance = Random.Range(0,2) == 0 ? forwardDistance : -forwardDistance;
            
            _arcHeight = Random.Range(arcHeightMin, arcHeightMax);
            _fallDistance = Random.Range(minFallDistance, maxFallDistance);
            _upDuration = Random.Range(minUpDuration, maxUpDuration);
            _downDuration = Random.Range(minDownDuration, maxDownDuration);
            _spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
        }

        public void SplitOff()
        {
            transform.SetParent(null); // The parent may be destroyed earlier
            Vector3 startPos = transform.position;
            Vector3 peakPos = startPos + new Vector3(forwardDistance * 0.5f, _arcHeight, 0f);

            Sequence seq = DOTween.Sequence();

            // Move up and forward to peak
            seq.Append(transform.DOMove(peakPos, _upDuration).SetEase(Ease.OutQuad));

            // Fall down and continue moving forward (relative to current peak)
            seq.Append(transform.DOMoveY(-_fallDistance, _downDuration).SetRelative(true).SetEase(Ease.InQuad));
            seq.Join(transform.DOMoveX(forwardDistance * 0.5f, _downDuration).SetRelative(true));

            // Spin
            float totalDuration = _upDuration + _downDuration;
            transform.DORotate(new Vector3(0, 0, _spinSpeed * totalDuration), totalDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);

            transform.DOScale(scaleUpTarget, scaleUpDuration).SetEase(Ease.OutBack);

            seq.OnComplete(() => Destroy(gameObject));
        }
    }
}
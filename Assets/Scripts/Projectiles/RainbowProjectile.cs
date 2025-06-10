using System;
using DG.Tweening;
using Pieces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Projectiles
{
    public class RainbowProjectile : MonoBehaviour
    {
        public event Action<RainbowProjectile, Piece> OnReachedTarget;

        [SerializeField] private float speed = 6f;
        [SerializeField] private float amplitude = 1.25f;
        [SerializeField] private float frequency = 1;
        [SerializeField] private Ease ease = Ease.OutSine;
        [SerializeField] private ParticleSystem impactEffect; 
        [SerializeField] private ParticleSystem trailEffect; 
        private Piece _targetPiece;
        private Tween _moveTween;

        public void Initialize(Piece targetPiece)
        {
            
            _targetPiece = targetPiece;
            if (targetPiece == null)
            {
                Debug.LogError("Target piece should not be null!");
            }
            
            Vector3 startPos = transform.position;
            Vector3 endPos = _targetPiece.transform.position;
            Vector3 direction = (endPos - startPos).normalized;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward); // For XY plane zigzag

            float distance = Vector3.Distance(startPos, endPos);
            float duration = distance / speed;
            
            trailEffect.gameObject.SetActive(true);
            
            _moveTween = DOVirtual.Float(0f, 1f, duration, t =>
            {
                Vector3 linearPos = Vector3.Lerp(startPos, endPos, t);
                
                // Sine wave offset
                float wave = Mathf.Sin(t * frequency * 2 * Mathf.PI) * amplitude;
                transform.position = linearPos + perpendicular * wave;
            })
            .SetEase(ease)
            .OnComplete(() =>
            {
                OnReachedTarget?.Invoke(this, _targetPiece);
                trailEffect.gameObject.SetActive(false);
                impactEffect.gameObject.SetActive(true);
                Destroy(gameObject,impactEffect.main.duration);
            });
        }

        private void OnDisable()
        {
            _moveTween?.Kill();
        }
    }
}

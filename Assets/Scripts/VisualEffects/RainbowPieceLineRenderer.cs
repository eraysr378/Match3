using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace VisualEffects
{
    public class RainbowPieceLineRenderer :BaseVisualEffect,IRainbowLineEffect
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        private LineRenderer _lineRenderer;
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField] private float pulseMinIntensity = 2f;
        [SerializeField] private float pulseMaxIntensity = 3.5f;
        private Color _color;
        private Coroutine _pulseCoroutine;
        private MaterialPropertyBlock _propertyBlock;
        
       private void Awake()
       {
           _lineRenderer = GetComponent<LineRenderer>();
           _propertyBlock = new MaterialPropertyBlock();
       }

       public void SetUpAtEdge(Transform from, Transform to, float radius, float widthMultiplier)
        {
            var direction = (to.position - from.position).normalized;
            var edgePosition = from.position + direction * radius;
    
            SetUp(edgePosition, to.position, widthMultiplier);
        }

        public void SetColor(Color color)
        {
            _lineRenderer.GetPropertyBlock(_propertyBlock);
            _color = color;
            _propertyBlock.SetColor(EmissionColor, _color);
            _lineRenderer.SetPropertyBlock(_propertyBlock);
        }
        public override void Play()
        {
            _lineRenderer.enabled = true;
            StartEmissionPulse(_color);
        }

        public override void Finish()
        {
            StopEmissionPulse();
            StartCoroutine(FadeAndDestroy());
        }
        private void SetUp(Vector3 startPosition, Vector3 endPosition,float widthMultiplier)
        {
            _lineRenderer.widthMultiplier = widthMultiplier;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, startPosition);
            _lineRenderer.SetPosition(1, endPosition);
            _lineRenderer.enabled = false;

        }
        private void StartEmissionPulse(Color baseColor)
        {
            if (_pulseCoroutine != null)
                StopCoroutine(_pulseCoroutine);

            _pulseCoroutine = StartCoroutine(PulseEmissionColor(baseColor));
        }
        private void StopEmissionPulse()
        {
            if (_pulseCoroutine != null)
            {
                StopCoroutine(_pulseCoroutine);
                _pulseCoroutine = null;
            }
        }
        private IEnumerator PulseEmissionColor(Color baseColor)
        {
            Color normalizedColor = NormalizeBrightness(baseColor);

            while (true)
            {
                float t = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f; // oscillates between 0 and 1
                float intensity = Mathf.Lerp(pulseMinIntensity, pulseMaxIntensity, t);
                Color emissionColor = normalizedColor * intensity;

                _lineRenderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor(EmissionColor, emissionColor);
                _lineRenderer.SetPropertyBlock(_propertyBlock);

                yield return null;
            }
        }

        private Color NormalizeBrightness(Color color)
        {
            float maxComponent = Mathf.Max(color.r, color.g, color.b);
            return maxComponent > 0f ? color / maxComponent : color;
        }
        private IEnumerator FadeAndDestroy()
        {
            float duration = 0.25f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Vector3 newPosition = Vector3.Lerp(_lineRenderer.GetPosition(0), _lineRenderer.GetPosition(1), t);

                _lineRenderer.SetPosition(0, newPosition);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _lineRenderer.SetPosition(0, _lineRenderer.GetPosition(1));
            OnReturnToPool();
        }

  
    }
}
using System;
using System.Collections;
using Interfaces;
using UnityEngine;

namespace VisualEffects
{
    public class PieceLineRenderer :BaseVisualEffect
    {
       private LineRenderer _lineRenderer;

       private void Awake()
       {
           _lineRenderer = GetComponent<LineRenderer>();
       }

       public void SetUpAtEdge(Transform from, Transform to, float radius, float widthMultiplier)
        {
            var direction = (to.position - from.position).normalized;
            var edgePosition = from.position + direction * radius;
    
            SetUp(edgePosition, to.position, widthMultiplier);
        }
    
        public override void Play()
        {
            _lineRenderer.enabled = true;
        }

        public override void Finish()
        {
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
        private IEnumerator FadeAndDestroy()
        {
            float duration = 0.2f;
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
            Destroy(gameObject);
        }

  
    }
}
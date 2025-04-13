using System;
using System.Collections;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class VisualController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer visual;
        [SerializeField] private float shrinkDuration = 0.2f;
        private Resizer _resizer;

        private void Awake()
        {
            _resizer = GetComponent<Resizer>();
        }

        public void Shrink(Action onComplete = null)
        {
            _resizer.Resize(visual.transform,Vector3.zero, shrinkDuration, onComplete);
        }

        
        public void ResetScale()
        {
            visual.transform.localScale = Vector3.one;
        }

        public void PlayBounce()
        {
        }

    }
}
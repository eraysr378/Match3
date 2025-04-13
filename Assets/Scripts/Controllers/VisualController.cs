using System;
using Pieces.Behaviors;
using UnityEngine;

namespace Controllers
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

        public void DisableVisual()
        {
            visual.enabled = false;
        }
        public void EnableVisual()
        {
            visual.enabled = true;
        }
        public Color GetColor()
        {
            return visual.color;
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
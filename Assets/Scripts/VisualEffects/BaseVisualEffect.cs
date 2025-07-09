using Interfaces;
using Managers;
using UnityEngine;

namespace VisualEffects
{
    public enum VisualEffectType
    {
        RainbowPieceLineRenderer,
        
    }
    public abstract class BaseVisualEffect : MonoBehaviour,IPoolableObject 
    {
        [SerializeField] private VisualEffectType effectType;
        public abstract void Play();
        public abstract void Finish();
        public void OnSpawn()
        {
        }

        public void OnReturnToPool()
        {
            EventManager.ReturnVisualEffectToPool?.Invoke(this);
        }
        public VisualEffectType GetEffectType() => effectType;
    }
}
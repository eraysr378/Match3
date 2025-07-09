using Factories.GeneralFactories;
using Managers;
using UnityEngine;
using VisualEffects;

namespace Spawners
{
    public class VisualEffectSpawner : MonoBehaviour
    {
        [SerializeField] private GeneralVisualEffectFactory visualEffectFactory;
        public void OnEnable()
        {
            EventManager.RequestVisualEffectSpawn += SpawnEffect;
        }

        private void OnDisable()
        {
            EventManager.RequestVisualEffectSpawn -= SpawnEffect;
        }

 
        private BaseVisualEffect SpawnEffect(VisualEffectType effectType)
        {
            BaseVisualEffect effect = visualEffectFactory.GetEffectBasedOnType(effectType);
            if (effect == null) return null;
            
            return effect;
        }

    }
}
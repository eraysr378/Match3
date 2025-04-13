using System.Collections.Generic;
using Factories.BaseFactories;
using Managers;
using UnityEngine;
using VisualEffects;

namespace Factories.GeneralFactories
{
    public class GeneralVisualEffectFactory : MonoBehaviour
    {
        [SerializeField] private List<BaseVisualEffectFactory> visualEffectFactoryList;

        private void OnEnable()
        {
            EventManager.OnVisualEffectReturnToPool += ReturnToPool;
        }

        private void OnDisable()
        {
            EventManager.OnVisualEffectReturnToPool -= ReturnToPool;
        }

        public BaseVisualEffect GetEffectBasedOnType(VisualEffectType effectType)
        {
            foreach (var factory in visualEffectFactoryList)
            {
                if (factory.CanCreateEffect(effectType))
                {
                    return factory.Get();
                }
            }

            return null;
        }

        private void ReturnToPool(BaseVisualEffect effect)
        {
            foreach (var factory in visualEffectFactoryList)
            {
                if (factory.CanCreateEffect(effect.GetEffectType()))
                {
                    factory.ReturnToPool(effect);
                    return;
                }
            }
        }
    }
}
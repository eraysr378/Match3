using System;
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
        [SerializeField] private Transform visualEffectParent;

        private void OnEnable()
        {
            EventManager.ReturnVisualEffectToPool += ReturnToPool;
        }

        private void OnDisable()
        {
            EventManager.ReturnVisualEffectToPool -= ReturnToPool;
            foreach (var factory in visualEffectFactoryList)
            {
                factory.ResetPool();
            }
        }

        private void Awake()
        {
            foreach (var factory in visualEffectFactoryList)
            {
                factory.Initialize(visualEffectParent);
            }
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
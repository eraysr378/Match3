using System;
using System.Collections.Generic;
using Combinations;
using Factories.BaseFactories;
using Managers;
using Misc;
using UnityEngine;

namespace Factories.GeneralFactories
{
    public class GeneralCombinationFactory : MonoBehaviour
    {

        [SerializeField] private List<BaseCombinationFactory> combinationFactoryList;
        [SerializeField] private Transform combinationParent;
        private void OnEnable()
        {
            EventManager.ReturnCombinationToPool += ReturnToPool;
        }
        private void OnDisable()
        {
            EventManager.ReturnCombinationToPool -= ReturnToPool;
            foreach (var factory in combinationFactoryList)
            {
                factory.ResetPool();
            }
        }

        private void Awake()
        {
            foreach (var factory in combinationFactoryList)
            {
                factory.Initialize(combinationParent);
            }
        }

        public BaseCombination GetCombinationBasedOnType(CombinationType combinationType)
        {
            foreach (var factory in combinationFactoryList)
            {
                if (factory.CanCreateCombination(combinationType))
                {
                    return factory.Get();
                }
            }
            return null;
        }

        private void ReturnToPool(BaseCombination combination)
        {
            foreach (var factory in combinationFactoryList)
            {
                if (factory.CanCreateCombination(combination.GetCombinationType()))
                {
                    factory.ReturnToPool(combination);
                    return;
                }
            }
        }
    }
}
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
        
        private void OnEnable()
        {
            EventManager.OnCombinationReturnToPool += ReturnToPool;
        }
        private void OnDisable()
        {
            EventManager.OnCombinationReturnToPool -= ReturnToPool;
            foreach (var factory in combinationFactoryList)
            {
                factory.ResetPool();
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
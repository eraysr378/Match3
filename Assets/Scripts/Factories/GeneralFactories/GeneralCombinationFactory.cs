using System.Collections.Generic;
using Combinations;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Factories.CombinationFactories
{
    public class GeneralCombinationFactory : MonoBehaviour
    {
        [SerializeField] private List<CombinationType> combinationTypeKeys;
        [SerializeField] private List<BaseCombinationFactory> combinationFactoryValues;
        private Dictionary<CombinationType, BaseCombinationFactory> _combinationFactoriesByType;
        

        public Combination CreateCombinationBasedOnType(CombinationType combinationType)
        {
            if (_combinationFactoriesByType.TryGetValue(combinationType, out var factory))
            {
                return factory.Get();
            }

            return null;
        }
    }
}
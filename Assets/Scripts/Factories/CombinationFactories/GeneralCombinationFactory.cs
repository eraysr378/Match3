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
        [SerializeField] private List<CombinationFactory> combinationFactoryValues;
        private Dictionary<CombinationType, CombinationFactory> _combinationFactoriesByType;

        private void Awake()
        {
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            _combinationFactoriesByType = new Dictionary<CombinationType, CombinationFactory>();
            for (int i = 0; i < Mathf.Min(combinationTypeKeys.Count, combinationFactoryValues.Count); i++)
            {
                if (!_combinationFactoriesByType.TryAdd(combinationTypeKeys[i], combinationFactoryValues[i]))
                {
                    Debug.LogError("Duplicate key");
                }
            }
        }

        public Combination CreateCombinationBasedOnType(CombinationType combinationType)
        {
            if (_combinationFactoriesByType.TryGetValue(combinationType, out var factory))
            {
                return factory.CreateCombination(combinationType);
            }

            return null;
        }
    }
}
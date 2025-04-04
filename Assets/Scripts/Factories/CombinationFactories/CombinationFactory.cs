using Combinations;
using Misc;
using UnityEngine;

namespace Factories.CombinationFactories
{
    public abstract class CombinationFactory: ScriptableObject
    {
        public abstract Combination CreateCombination(CombinationType combinationType);

    }
}
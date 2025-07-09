using Combinations;
using Misc;
using UnityEngine;

namespace Factories.BaseFactories
{
    public abstract class BaseCombinationFactory: BasePoolableObjectFactory<BaseCombination>
    {
        
        public abstract bool CanCreateCombination(CombinationType combinationType);

    }
}
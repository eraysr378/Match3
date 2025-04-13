using Combinations;
using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.CombinationFactories
{
    public abstract class BaseCombinationFactory: BasePoolableObjectFactory<Combination>
    {
        public abstract bool CanCreateCombination(CombinationType combinationType);

    }
}
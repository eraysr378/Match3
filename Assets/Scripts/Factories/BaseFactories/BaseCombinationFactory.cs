using Combinations;
using Misc;

namespace Factories.BaseFactories
{
    public abstract class BaseCombinationFactory: BasePoolableObjectFactory<BaseCombination>
    {
        public abstract bool CanCreateCombination(CombinationType combinationType);

    }
}
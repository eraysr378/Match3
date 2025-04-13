using Combinations;
using Factories.BaseFactories;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RocketRocketCombinationFactory",
        menuName = "Factories/Combination/RocketRocketCombinationFactory")]
    public class RocketRocketCombinationFactory : BaseCombinationFactory
    {
        public override bool CanCreateCombination(CombinationType combinationType)
        {
            return combinationType == CombinationType.RocketRocketCombination;
        }
    }
}
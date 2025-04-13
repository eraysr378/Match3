using Combinations;
using Factories.BaseFactories;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RainbowRocketCombinationFactory",
        menuName = "Factories/Combination/RainbowRocketCombinationFactory")]
    public class RainbowRocketCombinationFactory : BaseCombinationFactory
    {
        public override bool CanCreateCombination(CombinationType combinationType)
        {
            return combinationType == CombinationType.RainbowRocketCombination;
        }
    }
}
using Combinations;
using Factories.BaseFactories;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RainbowRainbowCombinationFactory",
        menuName = "Factories/Combination/RainbowRainbowCombinationFactory")]
    public class RainbowRainbowCombinationFactory : BaseCombinationFactory
    {
        public override bool CanCreateCombination(CombinationType combinationType)
        {
            return combinationType == CombinationType.RainbowRainbowCombination;
        }
    }
}
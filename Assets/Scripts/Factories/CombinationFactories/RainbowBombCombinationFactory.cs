using Combinations;
using Factories.BaseFactories;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RainbowBombCombinationFactory",
        menuName = "Factories/Combination/RainbowBombCombinationFactory")]
    public class RainbowBombCombinationFactory : BaseCombinationFactory
    {
        public override bool CanCreateCombination(CombinationType combinationType)
        {
            return combinationType == CombinationType.RainbowBombCombination;
        }
    }
}
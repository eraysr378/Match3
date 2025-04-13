using Combinations;
using Factories.BaseFactories;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RocketBombCombinationFactory", menuName = "Factories/Combination/RocketBombCombinationFactory")]
    public class RocketBombCombinationFactory : BaseCombinationFactory
    {
        public override bool CanCreateCombination(CombinationType combinationType)
        {
            return combinationType == CombinationType.RocketBombCombination;
        }
    }
}
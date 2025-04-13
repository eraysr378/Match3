using Combinations;
using Factories.BaseFactories;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "BombBombCombinationFactory", menuName = "Factories/Combination/BombBombCombinationFactory")]
    public class BombBombCombinationFactory : BaseCombinationFactory
    {
        public override bool CanCreateCombination(CombinationType combinationType)
        {
            return combinationType == CombinationType.BombBombCombination;
        }
    }
}
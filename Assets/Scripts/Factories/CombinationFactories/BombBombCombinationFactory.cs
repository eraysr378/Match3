using Combinations;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "BombBombCombinationFactory", menuName = "Factories/BombBombCombinationFactory")]
    public class BombBombCombinationFactory : CombinationFactory
    {
        [SerializeField] private BombBombCombination prefab;

        public override Combination CreateCombination(CombinationType combinationType)
        {
            BombBombCombination bombCombination = Instantiate(prefab);
            bombCombination.SetCombinationType(combinationType);
            return bombCombination;
        }
    }
}
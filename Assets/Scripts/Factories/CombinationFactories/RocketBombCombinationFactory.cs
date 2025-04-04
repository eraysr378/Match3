using Combinations;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RocketBombCombinationFactory", menuName = "Factories/RocketBombCombinationFactory")]
    public class RocketBombCombinationFactory : CombinationFactory
    {
        [SerializeField] private RocketBombCombination prefab;
        
        public override Combination CreateCombination(CombinationType combinationType)
        {
            RocketBombCombination rocketBombCombination = Instantiate(prefab);
            rocketBombCombination.SetCombinationType(combinationType);
            return rocketBombCombination;

        }
    }
}
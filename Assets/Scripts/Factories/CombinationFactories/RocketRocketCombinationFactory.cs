using Combinations;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RocketRocketCombinationFactory",
        menuName = "Factories/RocketRocketCombinationFactory")]
    public class RocketRocketCombinationFactory : CombinationFactory
    {
        [SerializeField] private RocketRocketCombination prefab;

        public override Combination CreateCombination(CombinationType combinationType)
        {
            RocketRocketCombination rocketRocketCombination = Instantiate(prefab);
            rocketRocketCombination.SetCombinationType(combinationType);
            return rocketRocketCombination;
        }
    }
}
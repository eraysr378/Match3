using Combinations;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RainbowRocketCombinationFactory",
        menuName = "Factories/RainbowRocketCombinationFactory")]
    public class RainbowRocketCombinationFactory : CombinationFactory
    {
        [SerializeField] private RainbowRocketCombination prefab;


        public override Combination CreateCombination(CombinationType combinationType)
        {
            RainbowRocketCombination rainbowRocketCombination = Instantiate(prefab);
            rainbowRocketCombination.SetCombinationType(combinationType);
            return rainbowRocketCombination;
        }
    }
}
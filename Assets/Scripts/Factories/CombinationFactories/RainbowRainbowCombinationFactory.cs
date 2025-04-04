using Combinations;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RainbowRainbowCombinationFactory",
        menuName = "Factories/RainbowRainbowCombinationFactory")]
    public class RainbowRainbowCombinationFactory : CombinationFactory
    {
        [SerializeField] private RainbowRainbowCombination prefab;


        public override Combination CreateCombination(CombinationType combinationType)
        {
            RainbowRainbowCombination rainbowRainbowCombination = Instantiate(prefab);
            rainbowRainbowCombination.SetCombinationType(combinationType);
            return rainbowRainbowCombination;
        }
    }
}
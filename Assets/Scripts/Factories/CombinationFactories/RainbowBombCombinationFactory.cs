using Combinations;
using Factories.PieceFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.CombinationFactories
{
    [CreateAssetMenu(fileName = "RainbowBombCombinationFactory",
        menuName = "Factories/RainbowBombCombinationFactory")]
    public class RainbowBombCombinationFactory : CombinationFactory
    {
        [SerializeField] private RainbowBombCombination prefab;


        public override Combination CreateCombination(CombinationType combinationType)
        {
            RainbowBombCombination rainbowBombCombination = Instantiate(prefab);
            rainbowBombCombination.SetCombinationType(combinationType);
            return rainbowBombCombination;
        }
    }
}
using Misc;
using Pieces;
using Pieces.CombinationPieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "RocketRocketCombinationFactory",
        menuName = "Factories/RocketRocketCombinationFactory")]
    public class RocketRocketCombinationFactory : PieceFactory
    {
        [SerializeField] private RocketRocketCombination prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            RocketRocketCombination rocketRocketCombination = Instantiate(prefab);
            rocketRocketCombination.SetPieceType(pieceType);
            return rocketRocketCombination;
        }
    }
}
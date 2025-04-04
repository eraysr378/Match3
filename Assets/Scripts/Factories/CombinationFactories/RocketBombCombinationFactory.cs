using Misc;
using Pieces;
using Pieces.CombinationPieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "RocketBombCombinationFactory", menuName = "Factories/RocketBombCombinationFactory")]
    public class RocketBombCombinationFactory : PieceFactory
    {
        [SerializeField] private RocketBombCombination prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            RocketBombCombination rocketBombCombination = Instantiate(prefab);
            rocketBombCombination.SetPieceType(pieceType);
            return rocketBombCombination;
        }
    }
}
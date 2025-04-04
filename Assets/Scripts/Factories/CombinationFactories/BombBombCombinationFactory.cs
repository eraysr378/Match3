using Misc;
using Pieces;
using Pieces.CombinationPieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "BombBombCombinationFactory", menuName = "Factories/BombBombCombinationFactory")]
    public class BombBombCombinationFactory : PieceFactory
    {
        [SerializeField] private BombBombCombination prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            BombBombCombination bombCombination = Instantiate(prefab);
            bombCombination.SetPieceType(pieceType);
            return bombCombination;
        }
    }
}
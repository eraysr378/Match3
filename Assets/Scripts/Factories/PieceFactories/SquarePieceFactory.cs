using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "SquarePieceFactory", menuName = "Factories/Piece/SquarePieceFactory")]

    public class SquarePieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.SquareNormalPiece;
        }
    }
}
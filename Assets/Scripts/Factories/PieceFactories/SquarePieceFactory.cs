using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "SquarePieceFactory", menuName = "Factories/SquarePieceFactory")]

    public class SquarePieceFactory : NormalPieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.SquareNormalPiece;
        }
    }
}
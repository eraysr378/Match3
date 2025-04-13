using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "BombPieceFactory", menuName = "Factories/Piece/BombPieceFactory")]

    public class BombPieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.BombPiece;
        }
    }
}
using Factories.BaseFactories;
using Misc;
using Pieces;
using Pieces.SpecialPieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "RainbowPieceFactory", menuName = "Factories/Piece/RainbowPieceFactory")]
    public class RainbowPieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.RainbowPiece;
        }
    }
}
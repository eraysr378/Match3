using Misc;
using Pieces;
using Pieces.SpecialPieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "RainbowPieceFactory", menuName = "Factories/RainbowPieceFactory")]
    public class RainbowBasePieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.RainbowPiece;
        }
    }
}
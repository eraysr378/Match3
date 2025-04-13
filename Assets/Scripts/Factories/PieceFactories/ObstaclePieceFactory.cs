using Factories.BaseFactories;
using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "ObstaclePieceFactory", menuName = "Factories/Piece/ObstaclePieceFactory")]
    public class ObstaclePieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.ObstaclePiece;
        }


    }

}
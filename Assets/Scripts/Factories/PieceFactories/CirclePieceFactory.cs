using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "CirclePieceFactory", menuName = "Factories/Piece/CirclePieceFactory")]

    public class CirclePieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.CircleNormalPiece;
        }
    }
}
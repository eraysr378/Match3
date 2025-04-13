using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "TrianglePieceFactory", menuName = "Factories/Piece/TrianglePieceFactory")]

    public class TrianglePieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.TriangleNormalPiece;
        }
    }
}
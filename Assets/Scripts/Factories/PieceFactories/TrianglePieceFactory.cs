using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "TrianglePieceFactory", menuName = "Factories/TrianglePieceFactory")]

    public class TriangleBasePieceFactory : NormalBasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.TriangleNormalPiece;
        }
    }
}
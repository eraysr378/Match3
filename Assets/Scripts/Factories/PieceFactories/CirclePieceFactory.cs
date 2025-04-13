using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "CirclePieceFactory", menuName = "Factories/CirclePieceFactory")]

    public class CircleBasePieceFactory : NormalBasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.CircleNormalPiece;
        }
    }
}
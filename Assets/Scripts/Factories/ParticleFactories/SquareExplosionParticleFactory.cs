using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "BombPieceFactory", menuName = "Factories/SquareExplodeParticleFactory")]

    public class SquareExplodeParticleFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.BombPiece;
        }
    }
}
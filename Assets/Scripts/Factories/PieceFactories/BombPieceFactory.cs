using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "BombPieceFactory", menuName = "Factories/BombPieceFactory")]

    public class BombBasePieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.BombPiece;
        }
    }
}
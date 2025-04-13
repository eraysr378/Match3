using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "ObstaclePieceFactory", menuName = "Factories/ObstaclePieceFactory")]
    public class ObstacleBasePieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.ObstaclePiece;
        }


    }

}
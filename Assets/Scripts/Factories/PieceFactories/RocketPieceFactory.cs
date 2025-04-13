using System;
using Factories.BaseFactories;
using Misc;
using Pieces;
using Pieces.SpecialPieces;
using UnityEngine;
using UnityEngine.Pool;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "RocketPieceFactory", menuName = "Factories/Piece/RocketPieceFactory")]

    public class RocketPieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.RocketPiece;
        }
    }
}
using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "HexagonPieceFactory", menuName = "Factories/Piece/HexagonPieceFactory")]

    public class HexagonPieceFactory : BasePieceFactory
    {
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.HexagonNormalPiece;
        }
    }
}
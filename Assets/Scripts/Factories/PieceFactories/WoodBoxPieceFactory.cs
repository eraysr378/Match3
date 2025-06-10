using Factories.BaseFactories;
using Misc;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "WoodBoxPieceFactory", menuName = "Factories/Piece/WoodBoxPieceFactory")]

    public class WoodBoxPieceFactory : BasePieceFactory
    {
        
        public override bool CanCreatePiece(PieceType pieceType)
        {
            return pieceType == PieceType.WoodBox;
            
        }
    }
}
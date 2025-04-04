using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "RainbowPieceFactory", menuName = "Factories/RainbowPieceFactory")]
    public class RainbowPieceFactory : PieceFactory

    {
        [SerializeField] private RainbowPiece prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            RainbowPiece rainbowPiece = Instantiate(prefab);
            rainbowPiece.SetPieceType(pieceType);
            return rainbowPiece;
        }
        
    }
}
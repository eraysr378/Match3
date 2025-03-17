using Pieces;
using Misc;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "ObstaclePieceFactory", menuName = "Factories/ObstaclePieceFactory")]
    public class ObstaclePieceFactory : PieceFactory
    {
        [SerializeField] private ObstaclePiece prefab;
        
        public override Piece CreateCell(PieceType pieceType)
        {
            ObstaclePiece obstaclePiece = Instantiate(prefab);
            obstaclePiece.SetPieceType(pieceType);
            return obstaclePiece;
        }
    }

}
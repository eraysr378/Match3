using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "ObstaclePieceFactory", menuName = "Factories/ObstaclePieceFactory")]
    public class ObstaclePieceFactory : PieceFactory
    {
        [SerializeField] private ObstaclePiece prefab;
        
        public override Piece CreatePiece(PieceType pieceType)
        {
            ObstaclePiece obstaclePiece = Instantiate(prefab);
            obstaclePiece.SetPieceType(pieceType);
            return obstaclePiece;
        }
    }

}
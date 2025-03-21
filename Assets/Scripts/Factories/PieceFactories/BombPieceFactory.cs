using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "BombPieceFactory", menuName = "Factories/BombPieceFactory")]

    public class BombPieceFactory : PieceFactory
    {
        [SerializeField] private BombPiece prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            BombPiece bombPiece = Instantiate(prefab);
            bombPiece.SetPieceType(pieceType);
            return bombPiece;
        }
    }
}
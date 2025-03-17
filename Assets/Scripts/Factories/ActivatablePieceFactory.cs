using Pieces;
using Misc;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "ActivatablePieceFactory", menuName = "Factories/ActivatablePieceFactory")]

    public class ActivatablePieceFactory : PieceFactory
    {
        [SerializeField] private ActivatablePiece prefab;

        public override Piece CreateCell(PieceType pieceType)
        {
            ActivatablePiece activatablePiece = Instantiate(prefab);
            activatablePiece.SetPieceType(pieceType);
            return activatablePiece;
        }
    }
}
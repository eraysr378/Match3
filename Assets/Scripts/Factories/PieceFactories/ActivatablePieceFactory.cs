using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "ActivatablePieceFactory", menuName = "Factories/ActivatablePieceFactory")]

    public class ActivatablePieceFactory : PieceFactory
    {
        [SerializeField] private ActivatablePiece prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            ActivatablePiece activatablePiece = Instantiate(prefab);
            activatablePiece.SetPieceType(pieceType);
            return activatablePiece;
        }
    }
}
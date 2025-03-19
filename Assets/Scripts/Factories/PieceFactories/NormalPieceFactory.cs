using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "NormalPieceFactory", menuName = "Factories/NormalPieceFactory")]
    public class NormalPieceFactory : PieceFactory
    {
        [SerializeField] private NormalPiece prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            NormalPiece normalPiece = Instantiate(prefab);
            normalPiece.SetPieceType(pieceType);
            return normalPiece;
        }
    }

}

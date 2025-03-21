using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    [CreateAssetMenu(fileName = "RocketPieceFactory", menuName = "Factories/RocketPieceFactory")]

    public class RocketPieceFactory : PieceFactory
    {
        [SerializeField] private RocketPiece prefab;

        public override Piece CreatePiece(PieceType pieceType)
        {
            RocketPiece rocketPiece = Instantiate(prefab);
            rocketPiece.SetPieceType(pieceType);
            return rocketPiece;
        }
    }
}
using System;
using Pieces;
using Misc;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "NormalPieceFactory", menuName = "Factories/NormalPieceFactory")]
    public class NormalPieceFactory : PieceFactory
    {
        [SerializeField] private NormalPiece prefab;

        public override Piece CreateCell(PieceType pieceType)
        {
            NormalPiece normalPiece = Instantiate(prefab);
            normalPiece.SetPieceType(pieceType);
            return normalPiece;
        }
    }

}

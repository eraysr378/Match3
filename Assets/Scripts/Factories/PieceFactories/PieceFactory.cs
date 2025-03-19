using Misc;
using Pieces;
using UnityEngine;

namespace Factories.PieceFactories
{
    public abstract class PieceFactory : ScriptableObject
    {
        public abstract Piece CreatePiece(PieceType pieceType);
    }

}

    
    
    

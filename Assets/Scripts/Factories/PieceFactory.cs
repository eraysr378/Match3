using Pieces;
using Misc;
using UnityEngine;

namespace Factories
{
    public abstract class PieceFactory : ScriptableObject
    {
        public abstract Piece CreateCell(PieceType pieceType);
    }

}

    
    
    

using Misc;
using Pieces;
using UnityEngine;

namespace Factories.BaseFactories
{
    public abstract class BasePieceFactory : BasePoolableObjectFactory<Piece>
    {
        public abstract bool CanCreatePiece(PieceType pieceType);
        
    }
}
using Misc;
using Pieces;
using UnityEngine;
using UnityEngine.Pool;

namespace Factories.PieceFactories
{
    public abstract class BasePieceFactory : BasePoolFactory<Piece>
    {
        private ObjectPool<Piece> _pool;
        public abstract bool CanCreatePiece(PieceType pieceType);
        
    }
}
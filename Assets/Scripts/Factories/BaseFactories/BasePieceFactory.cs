using Misc;
using Pieces;

namespace Factories.BaseFactories
{
    public abstract class BasePieceFactory : BasePoolableObjectFactory<Piece>
    {
        public abstract bool CanCreatePiece(PieceType pieceType);
        
    }
}
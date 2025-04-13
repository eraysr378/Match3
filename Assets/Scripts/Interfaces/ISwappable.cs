using Pieces;
using UnityEngine.EventSystems;

namespace Interfaces
{
    public interface ISwappable 
    {
        public void OnSwap(Piece otherPiece);
         public void OnPostSwap(Piece otherPiece);
    }
}

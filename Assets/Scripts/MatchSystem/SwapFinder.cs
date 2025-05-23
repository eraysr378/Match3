using Interfaces;
using Managers;
using Pieces;
using Grid = GridRelated.Grid;

namespace MatchSystem
{
    public class SwapFinder
    {
        private readonly Grid _grid;
        private MatchFinder _matchFinder;

        public SwapFinder(Grid grid,MatchFinder matchFinder)
        {
            _grid = grid;
            _matchFinder = matchFinder;
        }

        // public bool TryFindValidSwapForMatch(out (Piece, Piece)? pieces )
        // {
        //     pieces = null;
        //     for (int row = 0; row < _grid.Height; row++)
        //     {
        //         for (int col = 0; col < _grid.Width; col++)
        //         {
        //             Piece currentPiece = _grid.GetCellAt(row, col).CurrentPiece;
        //             if (currentPiece is not ISwappable) continue;
        //
        //             if (col + 1 < _grid.Width)
        //             {
        //                 Piece rightPiece = _grid.GetCellAt(row, col + 1).CurrentPiece;
        //                 if (rightPiece is ISwappable && _matchFinder.WouldSwapCauseMatch(currentPiece, rightPiece) )
        //                 {
        //                     pieces = (currentPiece, rightPiece);
        //                     return true;
        //                 }
        //             }
        //
        //             if (row + 1 < _grid.Height)
        //             {
        //                 Piece belowPiece = _grid.GetCellAt(row + 1, col).CurrentPiece;
        //                 if (belowPiece is ISwappable && _matchFinder.WouldSwapCauseMatch(currentPiece, belowPiece))
        //                 {
        //                     pieces = (currentPiece, belowPiece);
        //                     return true;
        //                 }
        //             }
        //         }
        //     }
        //     return false;
        // }
    }
}
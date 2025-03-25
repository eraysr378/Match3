using System;
using Interfaces;
using Managers;
using MatchSystem;
using Pieces;
using Grid = GridRelated.Grid;

namespace SwapSystem
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

        public (Piece, Piece)? FindSwappablePieces()
        {
            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Piece currentPiece = _grid.GetCell(row, col).CurrentPiece;
                    if (currentPiece is not ISwappable) continue;

                    if (col + 1 < _grid.Width)
                    {
                        Piece rightPiece = _grid.GetCell(row, col + 1).CurrentPiece;
                        if (rightPiece is ISwappable && WillFormMatch(currentPiece, rightPiece))
                            return (currentPiece, rightPiece);
                    }

                    if (row + 1 < _grid.Height)
                    {
                        Piece belowPiece = _grid.GetCell(row + 1, col).CurrentPiece;
                        if (belowPiece is ISwappable && WillFormMatch(currentPiece, belowPiece))
                            return (currentPiece, belowPiece);
                    }
                }
            }
            return null;
        }

        private bool WillFormMatch(Piece pieceA, Piece pieceB)
        {
            if (pieceA is IActivatable || pieceB is IActivatable)
                return true;

            // Swap the pieces temporarily
            EventManager.OnInstantSwapRequested?.Invoke(pieceA, pieceB);

            // Check if swapping creates a match
            bool matchFound = CanCreateMatch(pieceA) || CanCreateMatch(pieceB);

            // Swap back to original position
            EventManager.OnInstantSwapRequested?.Invoke(pieceA, pieceB);

            return matchFound;
        }

        private bool CanCreateMatch(Piece piece)
        {
            return _matchFinder.TryGetMatch(piece, out _, out _);
        }
    }
}
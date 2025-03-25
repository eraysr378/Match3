using System.Collections.Generic;
using GridRelated;
using Interfaces;
using Managers;
using Misc;
using Pieces;

namespace MatchSystem
{
    public class MatchFinder
    {
        private readonly Grid _grid;
        private HashSet<Piece> _matchedPieces = new HashSet<Piece>();

        public MatchFinder(Grid grid)
        {
            _grid = grid;
        }

        public List<(List<Piece>, Piece)> FindMatches(IEnumerable<Piece> pieces)
        {
            _matchedPieces.Clear();
            List<(List<Piece>, Piece)> allMatches = new List<(List<Piece>, Piece)>();

            foreach (Piece piece in pieces)
            {
                if (piece is not IMatchable || _matchedPieces.Contains(piece))
                    continue;

                if (TryGetMatch(piece, out var matchList, out var spawnPiece))
                {
                    _matchedPieces.UnionWith(matchList);
                    allMatches.Add((matchList, spawnPiece));
                }
            }

            return allMatches;
        }

        public bool TryGetMatch(Piece piece, out List<Piece> matchList, out Piece spawnPiece)
        {
            matchList = null;
            spawnPiece = piece;

            if (piece is not IMatchable || _matchedPieces.Contains(piece))
                return false;

            List<Piece> horizontalPieces = GetHorizontalMatch(piece);
            if (horizontalPieces.Count >= 2)
            {
                horizontalPieces.Add(piece);
                foreach (var horizontalPiece in horizontalPieces)
                {
                    var verticalExtension = GetVerticalMatch(horizontalPiece);
                    if (verticalExtension.Count >= 2)
                    {
                        horizontalPieces.AddRange(verticalExtension);
                        spawnPiece = horizontalPiece; // Prefer the intersection piece for spawn
                        break;
                    }
                }
            }

            if (horizontalPieces.Count >= 3)
            {
                matchList = horizontalPieces;
                return true;
            }

            List<Piece> verticalPieces = GetVerticalMatch(piece);
            if (verticalPieces.Count >= 2)
            {
                verticalPieces.Add(piece);
                foreach (var verticalPiece in verticalPieces)
                {
                    var horizontalExtension = GetHorizontalMatch(verticalPiece);
                    if (horizontalExtension.Count >= 2)
                    {
                        verticalPieces.AddRange(horizontalExtension);
                        spawnPiece = verticalPiece; // Prefer the intersection piece for spawn
                        break;
                    }
                }
            }

            if (verticalPieces.Count >= 3)
            {
                matchList = verticalPieces;
                return true;
            }

            return false;
        }

        private List<Piece> GetVerticalMatch(Piece piece)
        {
            if (piece is not IMatchable) return new List<Piece>();

            List<Piece> verticalPieces = new List<Piece>();
            PieceType pieceType = piece.GetPieceType();
            for (int row = piece.Row - 1; row >= 0; row--)
            {
                Piece pieceBelow = _grid.GetCell(row, piece.Col).CurrentPiece;
                if (pieceBelow is not IMatchable || pieceBelow.GetPieceType() != pieceType ||
                    _matchedPieces.Contains(pieceBelow))
                    break;

                verticalPieces.Add(pieceBelow);
            }

            for (int row = piece.Row + 1; row < _grid.Height; row++)
            {
                Piece pieceAbove = _grid.GetCell(row, piece.Col).CurrentPiece;
                if (pieceAbove is not IMatchable || pieceAbove.GetPieceType() != pieceType ||
                    _matchedPieces.Contains(pieceAbove))
                    break;

                verticalPieces.Add(pieceAbove);
            }

            return verticalPieces;
        }

        private List<Piece> GetHorizontalMatch(Piece piece)
        {
            if (piece is not IMatchable) return new List<Piece>();

            List<Piece> horizontalPieces = new List<Piece>();
            PieceType pieceType = piece.GetPieceType();
            for (int col = piece.Col - 1; col >= 0; col--)
            {
                Piece pieceLeft = _grid.GetCell(piece.Row, col).CurrentPiece;
                if (pieceLeft is not IMatchable || pieceLeft.GetPieceType() != pieceType ||
                    _matchedPieces.Contains(pieceLeft))
                    break;
                horizontalPieces.Add(pieceLeft);
            }

            for (int col = piece.Col + 1; col < _grid.Width; col++)
            {
                Piece pieceRight = _grid.GetCell(piece.Row, col).CurrentPiece;
                if (pieceRight is not IMatchable || pieceRight.GetPieceType() != pieceType ||
                    _matchedPieces.Contains(pieceRight))
                    break;
                horizontalPieces.Add(pieceRight);
            }

            return horizontalPieces;
        }
        public (Piece, Piece)? FindSwappablePieces()
        {
            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Piece currentPiece = _grid.GetCell(row, col).CurrentPiece;
                    if (currentPiece is not ISwappable) continue;

                    // Check right swap
                    if (col + 1 < _grid.Width)
                    {
                        Piece rightPiece = _grid.GetCell(row, col + 1).CurrentPiece;
                        if (rightPiece is ISwappable && WillFormMatch(currentPiece, rightPiece))
                            return (currentPiece, rightPiece);
                    }

                    // Check below swap
                    if (row + 1 < _grid.Height)
                    {
                        Piece belowPiece = _grid.GetCell(row + 1, col).CurrentPiece;
                        if (belowPiece is ISwappable && WillFormMatch(currentPiece, belowPiece))
                            return (currentPiece, belowPiece);
                    }
                }
            }
            return null; // No swappable pieces found
        }

        private bool WillFormMatch(Piece pieceA, Piece pieceB)
        {
            if(pieceA is IActivatable || pieceB is IActivatable)
                return true;
            // Swap the pieces temporarily
            EventManager.OnInstantSwapRequested?.Invoke(pieceA, pieceB);
    
            // Check if swapping creates a match
            bool matchFound = TryGetMatch(pieceA, out _, out _) || TryGetMatch(pieceB, out _, out _);
    
            // Swap back to original position
            EventManager.OnInstantSwapRequested?.Invoke(pieceA, pieceB);
    
            return matchFound;
        }
    }
}
using System.Collections.Generic;
using Cells;
using Interfaces;
using Misc;
using Pieces;
using Grid = GridRelated.Grid;

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

        public bool TryFindMatch(Piece piece, out List<Piece> matchList, out Piece spawnPiece)
        {
            _matchedPieces.Clear();
            return TryGetMatch(piece, out matchList, out spawnPiece);
        }

        public List<(List<Piece>, Piece)> FindMatches(params Piece[] pieces)
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

        private bool TryGetMatch(Piece piece, out List<Piece> matchList, out Piece spawnPiece)
        {
            matchList = null;
            spawnPiece = piece;

            if (piece is not IMatchable || _matchedPieces.Contains(piece))
            {
                return false;
            }

            List<Piece> horizontalPieces = GetHorizontalMatch(piece.CurrentCell, piece.GetPieceType());
            if (horizontalPieces.Count >= 2)
            {
                horizontalPieces.Add(piece);
                foreach (var horizontalPiece in horizontalPieces)
                {
                    var verticalExtension =
                        GetVerticalMatch(horizontalPiece.CurrentCell, horizontalPiece.GetPieceType());
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

            List<Piece> verticalPieces = GetVerticalMatch(piece.CurrentCell, piece.GetPieceType());
            if (verticalPieces.Count >= 2)
            {
                verticalPieces.Add(piece);
                foreach (var verticalPiece in verticalPieces)
                {
                    var horizontalExtension =
                        GetHorizontalMatch(verticalPiece.CurrentCell, verticalPiece.GetPieceType());
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

        private List<Piece> GetVerticalMatch(Cell origin, PieceType pieceType, Cell ignoreCell = null)
        {
            List<Piece> verticalPieces = new List<Piece>();
            for (int row = origin.Row - 1; row >= 0; row--)
            {
                Cell cell = _grid.GetCellAt(row, origin.Col);
                if (cell == ignoreCell)
                {
                    break;
                }

                Piece pieceBelow = cell.CurrentPiece;
                if (pieceBelow is not IMatchable || pieceBelow.GetPieceType() != pieceType || pieceBelow.IsBusy() ||
                    _matchedPieces.Contains(pieceBelow))
                    break;

                verticalPieces.Add(pieceBelow);
            }

            for (int row = origin.Row + 1; row < _grid.Height; row++)
            {
                Cell cell = _grid.GetCellAt(row, origin.Col);
                if (cell == ignoreCell)
                    break;
                Piece pieceAbove = cell.CurrentPiece;
                if (pieceAbove is not IMatchable || pieceAbove.GetPieceType() != pieceType || pieceAbove.IsBusy() ||
                    _matchedPieces.Contains(pieceAbove))
                    break;

                verticalPieces.Add(pieceAbove);
            }

            return verticalPieces;
        }

        private List<Piece> GetHorizontalMatch(Cell origin, PieceType pieceType, Cell ignoreCell = null)
        {
            List<Piece> horizontalPieces = new List<Piece>();
            for (int col = origin.Col - 1; col >= 0; col--)
            {
                var cell = _grid.GetCellAt(origin.Row, col);
                if (cell == ignoreCell)
                    break;
                var pieceLeft = cell.CurrentPiece;
                if (pieceLeft is not IMatchable || pieceLeft.GetPieceType() != pieceType || pieceLeft.IsBusy() ||
                    _matchedPieces.Contains(pieceLeft))
                    break;
                horizontalPieces.Add(pieceLeft);
            }

            for (int col = origin.Col + 1; col < _grid.Width; col++)
            {
                var cell = _grid.GetCellAt(origin.Row, col);
                if (cell == ignoreCell)
                    break;
                var pieceRight = cell.CurrentPiece;
                if (pieceRight is not IMatchable || pieceRight.GetPieceType() != pieceType || pieceRight.IsBusy() ||
                    _matchedPieces.Contains(pieceRight))
                    break;
                horizontalPieces.Add(pieceRight);
            }

            return horizontalPieces;
        }

        // public (Piece, Piece)? FindSwappablePieces()
        // {
        //     for (int row = 0; row < _grid.Height; row++)
        //     {
        //         for (int col = 0; col < _grid.Width; col++)
        //         {
        //             Piece currentPiece = _grid.GetCellAt(row, col).CurrentPiece;
        //             if (currentPiece is not ISwappable) continue;
        //
        //             // Check right swap
        //             if (col + 1 < _grid.Width)
        //             {
        //                 Piece rightPiece = _grid.GetCellAt(row, col + 1).CurrentPiece;
        //                 if (rightPiece is ISwappable && WouldSwapCauseMatch(currentPiece, rightPiece))
        //                     return (currentPiece, rightPiece);
        //             }
        //
        //             // Check below swap
        //             if (row + 1 < _grid.Height)
        //             {
        //                 Piece belowPiece = _grid.GetCellAt(row + 1, col).CurrentPiece;
        //                 if (belowPiece is ISwappable && WouldSwapCauseMatch(currentPiece, belowPiece))
        //                     return (currentPiece, belowPiece);
        //             }
        //         }
        //     }
        //
        //     return null; // No swappable pieces found
        // }

        public bool WouldSwapCauseMatch(Piece pieceA, Piece pieceB)
        {
            var cellA = pieceA.CurrentCell;
            var cellB = pieceB.CurrentCell;

            var typeA = pieceA.GetPieceType();
            var typeB = pieceB.GetPieceType();

            int horizMatchA = GetHorizontalMatch(cellB, typeA, cellA).Count + 1;
            int vertMatchA = GetVerticalMatch(cellB, typeA, cellA).Count + 1;

            int horizMatchB = GetHorizontalMatch(cellA, typeB, cellB).Count + 1;
            int vertMatchB = GetVerticalMatch(cellA, typeB, cellB).Count + 1;

            return horizMatchA >= 3 || vertMatchA >= 3 || horizMatchB >= 3 || vertMatchB >= 3;
        }
    }
}
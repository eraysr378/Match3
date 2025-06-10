using System.Collections.Generic;
using System.IO;
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
        
        public List<Match> FindMatches(params Piece[] pieces)
        {
            _matchedPieces.Clear();
            List<Match> allMatches = new List<Match>();

            foreach (Piece piece in pieces)
            {
                if (piece is not IMatchable || _matchedPieces.Contains(piece))
                    continue;

                if (TryGetMatch(piece, out var matchList, out var originCell,out var matchShape))
                {
                    _matchedPieces.UnionWith(matchList);
                    var match = new Match(matchList, matchShape,originCell );
                    allMatches.Add(match);
                }
            }

            return allMatches;
        }

        private bool TryGetMatch(Piece piece, out List<Piece> matchList, out BaseCell originCell,out MatchShape matchShape)
        {
            matchList = null;
            originCell = piece.CurrentCell;
            matchShape = MatchShape.Line;
            if (piece is not IMatchable || _matchedPieces.Contains(piece))
            {
                return false;
            }

            if (TryFindMatchStartingHorizontally(piece, out matchList, out originCell, out var isBidirectional,
                    out var isExtended))
            {
                matchShape = DetectShape(isBidirectional, isExtended);
                return true;

            }

            if (TryFindMatchStartingVertically(piece, out matchList, out originCell, out  isBidirectional,
                    out  isExtended))
            {
                matchShape = DetectShape(isBidirectional, isExtended);
                return true;
            }
            return false;
        }

        private bool TryFindMatchStartingHorizontally(Piece piece, out List<Piece> matchList, out BaseCell originCell,out bool isBidirectional,out bool isExtended)
        {
            matchList = null;
            originCell = piece.CurrentCell;
            isBidirectional = false;
            isExtended = false;
            var horizontalMatch = GetHorizontalMatch(piece.CurrentCell, piece.GetPieceType(),out _);
            
            if (horizontalMatch.Count >= 2)
            {
                horizontalMatch.Add(piece);

                foreach (var p in horizontalMatch)
                {
                    var verticalExtension = GetVerticalMatch(p.CurrentCell, p.GetPieceType(),out var currentIsBidirectional);
                    if (verticalExtension.Count >= 2)
                    {
                        isExtended = true;
                        isBidirectional = currentIsBidirectional;
                        horizontalMatch.AddRange(verticalExtension);
                        originCell = p.CurrentCell; // Intersection is preferred
                        break;
                    }
                }

                if (horizontalMatch.Count >= 3)
                {
                    matchList = horizontalMatch;
                    return true;
                }
            }

            return false;
        }

        private bool TryFindMatchStartingVertically(Piece piece, out List<Piece> matchList, out BaseCell originCell,out bool isBidirectional,out bool isExtended)
        {
            matchList = null;
            originCell = piece.CurrentCell;
            isBidirectional = false;
            isExtended = false;
            var verticalMatch = GetVerticalMatch(piece.CurrentCell, piece.GetPieceType(),out _);

            if (verticalMatch.Count >= 2)
            {
                verticalMatch.Add(piece);

                foreach (var p in verticalMatch)
                {
                    var horizontalExtension = GetHorizontalMatch(p.CurrentCell, p.GetPieceType(),out var currentIsBidirectional);
                    if (horizontalExtension.Count >= 2)
                    {
                        isExtended = true;
                        isBidirectional = currentIsBidirectional;
                        verticalMatch.AddRange(horizontalExtension);
                        originCell = p.CurrentCell; // Intersection is preferred
                        break;
                    }
                }

                if (verticalMatch.Count >= 3)
                {
                    matchList = verticalMatch;
                    return true;
                }
            }

            return false;
        }

        private MatchShape DetectShape(bool isBidirectional,bool isExtended)
        {
            if (isBidirectional)
                return MatchShape.T;
            if (isExtended)
                return MatchShape.L;
            
            return MatchShape.Line;
            
        }
        private List<Piece> GetVerticalMatch(BaseCell origin, PieceType pieceType, out bool isBidirectional,
            BaseCell ignoreCell = null)
        {
            
            var matchesDown = GetMatchingPiecesDown(origin, pieceType, ignoreCell);
            var matchesUp = GetMatchingPiecesUp(origin, pieceType, ignoreCell);

            isBidirectional = matchesDown.Count > 0 && matchesUp.Count > 0;

            var verticalMatch = new List<Piece>();
            verticalMatch.AddRange(matchesDown);
            verticalMatch.AddRange(matchesUp);
    
            return verticalMatch;
        }

        private  List<Piece> GetMatchingPiecesUp(BaseCell origin, PieceType pieceType, BaseCell ignoreCell )
        {
            List<Piece> matchList = new List<Piece>();

            for (int row = origin.Row + 1; row < _grid.Height; row++)
            {
                BaseCell cell = _grid.GetCellAt(row, origin.Col);
                if (cell == null || cell == ignoreCell)
                    break;
                Piece pieceUp = cell.CurrentPiece;
                if (pieceUp is not IMatchable || pieceUp.GetPieceType() != pieceType || pieceUp.IsBusy() ||
                    _matchedPieces.Contains(pieceUp))
                    break;

                matchList.Add(pieceUp);
            }
            return matchList;
        }

        private List<Piece> GetMatchingPiecesDown(BaseCell origin, PieceType pieceType, BaseCell ignoreCell)
        {
            List<Piece> matchList = new List<Piece>();
            for (int row = origin.Row - 1; row >= 0; row--)
            {
                BaseCell cell = _grid.GetCellAt(row, origin.Col);
                if (cell == null || cell == ignoreCell)
                    break;

                Piece pieceDown = cell.CurrentPiece;
                if (pieceDown is not IMatchable || pieceDown.GetPieceType() != pieceType || pieceDown.IsBusy() ||
                    _matchedPieces.Contains(pieceDown))
                    break;
                matchList.Add(pieceDown);
            }

            return matchList;
        }

        private List<Piece> GetHorizontalMatch(BaseCell origin, PieceType pieceType, out bool isBidirectional,
            BaseCell ignoreCell = null)
        {
            var matchesLeft = GetMatchingPiecesLeft(origin, pieceType, ignoreCell);
            var matchesRight =  GetMatchingPiecesRight(origin, pieceType, ignoreCell);

            isBidirectional = matchesLeft.Count > 0 && matchesRight.Count > 0;

            List<Piece> horizontalPieces = new List<Piece>();
            horizontalPieces.AddRange(matchesLeft);
            horizontalPieces.AddRange(matchesRight);
            return horizontalPieces;
        }

        private List<Piece>  GetMatchingPiecesRight(BaseCell origin, PieceType pieceType, BaseCell ignoreCell)
        {
            List<Piece> leftPieces = new List<Piece>();
            for (int col = origin.Col + 1; col < _grid.Width; col++)
            {
                var cell = _grid.GetCellAt(origin.Row, col);
                if (cell == null || cell == ignoreCell)
                    break;
                var pieceRight = cell.CurrentPiece;
                if (pieceRight is not IMatchable || pieceRight.GetPieceType() != pieceType || pieceRight.IsBusy() ||
                    _matchedPieces.Contains(pieceRight))
                    break;
                leftPieces.Add(pieceRight);
            }

            return leftPieces;
        }

        private List<Piece> GetMatchingPiecesLeft(BaseCell origin, PieceType pieceType, BaseCell ignoreCell)
        {
            List<Piece> rightPieces = new List<Piece>();
            for (int col = origin.Col - 1; col >= 0; col--)
            {
                var cell = _grid.GetCellAt(origin.Row, col);
                if (cell == null || cell == ignoreCell)
                    break;
                var pieceLeft = cell.CurrentPiece;
                if (pieceLeft is not IMatchable || pieceLeft.GetPieceType() != pieceType || pieceLeft.IsBusy() ||
                    _matchedPieces.Contains(pieceLeft))
                    break;
                rightPieces.Add(pieceLeft);
            }

            return rightPieces;
        }


        public bool WouldSwapCauseMatch(Piece pieceA, Piece pieceB)
        {
            var cellA = pieceA.CurrentCell;
            var cellB = pieceB.CurrentCell;

            var typeA = pieceA.GetPieceType();
            var typeB = pieceB.GetPieceType();

            int horizMatchA = GetHorizontalMatch(cellB, typeA,out _, cellA).Count + 1;
            int vertMatchA = GetVerticalMatch(cellB, typeA,out _, cellA).Count + 1;

            int horizMatchB = GetHorizontalMatch(cellA, typeB,out _, cellB).Count + 1;
            int vertMatchB = GetVerticalMatch(cellA, typeB,out _, cellB).Count + 1;

            return horizMatchA >= 3 || vertMatchA >= 3 || horizMatchB >= 3 || vertMatchB >= 3;
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
    }
}
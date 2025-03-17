using System.Collections.Generic;
using Pieces;
using Interfaces;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class MatchManager : MonoBehaviour
    {
        private Grid _grid;

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }

        public void OnEnable()
        {
            EventManager.OnFillCompleted += OnFillCompleted;
        }

        public void OnDisable()
        {
            EventManager.OnFillCompleted -= OnFillCompleted;
        }

        private void OnFillCompleted()
        {
            if (ClearAllValidMatches())
            {
                EventManager.OnValidMatchCleared?.Invoke();
            }
        }

        public bool ClearAllValidMatches()
        {
            HashSet<Piece> matchedPieces = new HashSet<Piece>();

            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Piece piece = _grid.GetCell(row, col).CurrentPiece;
                    if (piece is not IMatchable || matchedPieces.Contains(piece)) continue;

                    var matchList = GetMatch(piece);
                    if (matchList == null) continue;

                    foreach (var match in matchList)
                    {
                        match.DestroyPiece();
                        matchedPieces.Add(match);
                    }
                }
            }

            return matchedPieces.Count > 0;
        }

        public List<Piece> GetMatch(Piece piece)
        {
            if (piece is not IMatchable) return null;

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
                        break;
                    }
                }
            }

            if (horizontalPieces.Count >= 3)
                return horizontalPieces;


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
                        break;
                    }
                }
            }

            return verticalPieces.Count >= 3 ? verticalPieces : null;
        }

        private List<Piece> GetVerticalMatch(Piece piece)
        {
            if (piece is not IMatchable) return new List<Piece>();

            List<Piece> verticalPieces = new List<Piece>();
            PieceType pieceType = piece.GetPieceType();
            for (int row = piece.Row - 1; row >= 0; row--)
            {
                Piece pieceBelow = _grid.GetCell(row, piece.Col).CurrentPiece;
                if (pieceBelow is not IMatchable || pieceBelow.GetPieceType() != pieceType)
                    break;

                verticalPieces.Add(pieceBelow);
            }

            for (int row = piece.Row + 1; row < _grid.Height; row++)
            {
                Piece pieceAbove = _grid.GetCell(row, piece.Col).CurrentPiece;
                if (pieceAbove is not IMatchable || pieceAbove.GetPieceType() != pieceType)
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
                if (pieceLeft is not IMatchable || pieceLeft.GetPieceType() != pieceType)
                    break;
                horizontalPieces.Add(pieceLeft);
            }

            for (int col = piece.Col + 1; col < _grid.Width; col++)
            {
                Piece pieceRight = _grid.GetCell(piece.Row, col).CurrentPiece;
                if (pieceRight is not IMatchable || pieceRight.GetPieceType() != pieceType)
                    break;
                horizontalPieces.Add(pieceRight);
            }

            return horizontalPieces;
        }
    }
}
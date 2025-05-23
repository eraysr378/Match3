using System.Collections.Generic;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;

namespace MatchSystem
{
    public class FillCompletionMatchChecker
    {
        private MatchFinder _matchFinder;
        private MatchHandler _matchHandler;

        public FillCompletionMatchChecker(MatchFinder matchFinder, MatchHandler matchHandler)
        {
            _matchFinder = matchFinder;
            _matchHandler = matchHandler;

            // FillHandler.OnAnyFillCompleted += OnAnyPieceFillCompleted;
        }

        public void Dispose()
        {
            // FillHandler.OnAnyFillCompleted -= OnAnyPieceFillCompleted;
        }

        private void OnAnyPieceFillCompleted(FillHandler fillHandler)
        {
            Piece piece = fillHandler.GetComponent<Piece>();
            if (piece == null || piece.CurrentCell == null) return;

            if (_matchFinder.TryFindMatch(piece, out var matchList, out var spawnPiece))
            {
                _matchHandler.HandleMatches(new List<(List<Piece>, Piece)>
                {
                    (matchList, spawnPiece)
                });
            }
        }
    }
}
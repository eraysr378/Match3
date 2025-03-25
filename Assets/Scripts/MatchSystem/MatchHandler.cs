using System;
using System.Collections.Generic;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces;
using UnityEngine;

namespace MatchSystem
{
    public class MatchHandler
    {
                
        public static event Action OnMatchHandlingStarted;
        public static event Action OnMatchHandlingCompleted;
        private int _activeMatchLists = 0;

        public void HandleMatches(List<(List<Piece>, Piece)> matchLists)
        {
            OnMatchHandlingStarted?.Invoke();
            // Debug.Log("Match Handling Started");
            _activeMatchLists = matchLists.Count;

            foreach (var (matchList, spawnPiece) in matchLists)
            {
                HandleSingleMatch(matchList, spawnPiece);
            }
        }

        private void HandleSingleMatch(List<Piece> matchList, Piece spawnPiece)
        {
            int remainingPieces = matchList.Count;
            Cell spawnCell = spawnPiece.CurrentCell;

            foreach (var piece in matchList)
            {
                if (piece is IMatchable matchable)
                {
                    matchable.OnMatchHandled += OnMatchHandled;

                    if (matchList.Count == 3)
                        matchable.OnNormalMatch();
                    else if (matchList.Count > 3)
                        matchable.OnSpecialMatch(spawnCell);
                    else
                        Debug.LogError($"Matchlist has less than 3 pieces");
                }
                else
                {
                    Debug.LogError($"Piece {piece.name} does not implement IMatchable!");
                }
            }

            return;

            void OnMatchHandled()
            {
                remainingPieces--;

                if (remainingPieces == 0)
                {
                    switch (matchList.Count)
                    {
                        case > 4:
                            RequestSpecialPieceSpawn(spawnCell, PieceType.RocketPiece);
                            break;
                        case > 3:
                            RequestSpecialPieceSpawn(spawnCell, PieceType.BombPiece);
                            break;
                    }

                    _activeMatchLists--;
                    // Debug.Log($"remaining match lists: {_activeMatchLists}");
                    if (_activeMatchLists == 0)
                    {
                        OnMatchHandlingCompleted?.Invoke();
                        // Debug.Log("Match Handling end");

                    }
                }
            }
        }

        private Piece RequestSpecialPieceSpawn(Cell cell,PieceType pieceType)
        {
            // Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, cell.Row, cell.Col);
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(pieceType, cell.Row, cell.Col);
            piece.SetCell(cell);
            return piece;
        }
    }
}

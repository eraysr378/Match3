using System;
using System.Collections.Generic;
using Cells;
using Interfaces;
using Managers;
using Misc;
using Pieces;
using UnityEngine;
using Utils;

namespace MatchSystem
{
    public class MatchProcess
    {
        public event Action OnAllMatchablesNotified;
        public event Action<CellDirtyTracker> OnMatchProcessCompleted;
        private int _remainingPieces;
        private readonly CellDirtyTracker _cellDirtyTracker;
        private readonly Match _match;
        public MatchProcess(Match match,CellDirtyTracker cellDirtyTracker)
        {
            _match = match;
            _remainingPieces = match.PieceList.Count;
            _cellDirtyTracker = cellDirtyTracker;
        }

        public void Begin()
        {
            foreach (var piece in _match.PieceList)
            {
                if (piece is not IMatchable matchable)
                {
                    Debug.LogError($"Piece {piece.name} does not implement IMatchable!");
                    continue;
                }
                if (piece.CurrentCell == null)
                {
                    Debug.LogError($"{piece.name} does not have a cell!");
                }

                _cellDirtyTracker.Mark(piece.CurrentCell);

                if (_match.Length == 3)
                {
                    if (!matchable.TryHandleNormalMatch(OnPieceMatchHandled))
                    {
                        Debug.LogError("TryHandleNormalMatch failed");
                    }
                }
                else
                {
                    if (!matchable.TryHandleSpecialMatch(_match.OriginCell, OnPieceMatchHandled))
                    {
                        Debug.LogError("TryHandleSpecialMatch failed");
                    }
                }
            }
            NotifyMatchCells();
            NotifyAdjacentCells();
            OnAllMatchablesNotified?.Invoke();
        }

        private void OnPieceMatchHandled()
        {
            _remainingPieces--;
            if (_remainingPieces != 0) return;
            if(_match.Length > 3)
                SpawnSpecialPiece();
            OnMatchProcessCompleted?.Invoke(_cellDirtyTracker);
        }

        private void RequestSpecialPieceSpawn(BaseCell cell, PieceType pieceType)
        {
            Piece piece = EventManager.RequestPieceSpawn?.Invoke(pieceType, cell.Row, cell.Col);
            piece?.SetCell(cell);
        }

        private void SpawnSpecialPiece()
        {
            PieceType pieceType = MatchRewardDecider.DecideReward(_match);
            RequestSpecialPieceSpawn(_match.OriginCell, pieceType);

        }
        private void NotifyMatchCells()
        {
            var matchedCells = _cellDirtyTracker.GetDirtyCells();
            foreach (var cell in matchedCells)
            {
                cell.OnPieceMatched();
            }
        }
        private void NotifyAdjacentCells()
        {
            var matchedCells = _cellDirtyTracker.GetDirtyCells();
            foreach (var cell in matchedCells)
            {
                var adjacentCells = GridManager.Instance.GetAdjacentCells(cell.Row, cell.Col);
                foreach (var adjacentCell in adjacentCells)
                {
                    if (!matchedCells.Contains(adjacentCell))
                    {
                        adjacentCell?.TriggerAdjacentMatch();
                    }
                }
            }
        }
    }
}
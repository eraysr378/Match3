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
        private readonly Cell _spawnCell;
        private readonly List<Cell> _dirtyCells = new();
        private readonly List<Piece> _matchList;
        private readonly CellDirtyTracker _cellDirtyTracker;
        public MatchProcess(List<Piece> matchList, Piece spawnPiece,CellDirtyTracker cellDirtyTracker)
        {
            _matchList = matchList;
            _spawnCell = spawnPiece.CurrentCell;
            _remainingPieces = matchList.Count;
            _cellDirtyTracker = cellDirtyTracker;
        }

        public void Begin()
        {
            foreach (var piece in _matchList)
            {
                if (piece is not IMatchable matchable)
                {
                    Debug.LogError($"Piece {piece.name} does not implement IMatchable!");
                    continue;
                }

                if (piece.CurrentCell == null)
                {
                    Debug.LogWarning($"{piece.name} does not have a cell!");
                }

                _cellDirtyTracker.Mark(piece.CurrentCell);

                if (_matchList.Count == 3)
                {
                    if (!matchable.TryHandleNormalMatch(OnPieceMatchHandled))
                    {
                        Debug.LogError("TryHandleNormalMatch failed");
                    }
                }
                else
                {
                    if (!matchable.TryHandleSpecialMatch(_spawnCell, OnPieceMatchHandled))
                    {
                        Debug.LogError("TryHandleSpecialMatch failed");
                    }
                }   
            }
            OnAllMatchablesNotified?.Invoke();
        }

        private void OnPieceMatchHandled()
        {
            _remainingPieces--;
            if (_remainingPieces != 0) return;
            SpawnCorrespondingPiece(_matchList.Count, _spawnCell);
            OnMatchProcessCompleted?.Invoke(_cellDirtyTracker);
        }

        private void RequestSpecialPieceSpawn(Cell cell, PieceType pieceType)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(pieceType, cell.Row, cell.Col);
            piece?.SetCell(cell);
        }

        private void SpawnCorrespondingPiece(int count, Cell spawnCell)
        {
            switch (count)
            {
                case > 5:
                    RequestSpecialPieceSpawn(spawnCell, PieceType.RainbowPiece);
                    break;
                case > 4:
                    RequestSpecialPieceSpawn(spawnCell, PieceType.BombPiece);
                    break;
                case > 3:
                    RequestSpecialPieceSpawn(spawnCell, PieceType.RocketPiece);
                    break;
            }
        }
    }
}
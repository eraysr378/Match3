using System;
using System.Collections.Generic;
using Cells;
using Pieces;
using UnityEngine;
using Utils;

namespace MatchSystem
{
    public class MatchHandler
    {
                
        public static event Action OnMatchHandlingStarted;
        public static event Action OnMatchHandlingCompleted;
        public bool IsHandlerActive {get; private set;} = false;

        private int _activeProcessCount = 0;
        private int _startedProcessCount = 0;
        private int _totalProcessCount = 0;
        private readonly List<MatchProcess> _processList = new();
        private readonly List<CellDirtyTracker> _cellDirtyTrackerList = new ();
        public void HandleMatches(List<(List<Piece>, Piece)> matchLists)
        {
            if (!IsHandlerActive)
            {
                OnMatchHandlingStarted?.Invoke();
                IsHandlerActive = true;
            }
            
            foreach (var (matchList, spawnPiece) in matchLists)
            {
                CreateMatchProcess(matchList, spawnPiece);
            }
            foreach (var process in _processList)
            {
                process.Begin();
            }
            _processList.Clear();
        }
        
        private void CreateMatchProcess(List<Piece> matchList, Piece spawnPiece)
        {
            var cellDirtyTracker = new CellDirtyTracker();
            var process = new MatchProcess(matchList, spawnPiece,cellDirtyTracker);
            process.OnAllMatchablesNotified += OnAllMatchablesNotified;
            process.OnMatchProcessCompleted += OnMatchProcessCompleted;
            _processList.Add(process);
            _totalProcessCount++;
            _activeProcessCount++;
        }

        private void OnAllMatchablesNotified()
        {
            _startedProcessCount++;
        }

        private void OnMatchProcessCompleted(CellDirtyTracker cellDirtyTracker)
        {
            _activeProcessCount--;
            _cellDirtyTrackerList.Add(cellDirtyTracker);

            if (_startedProcessCount == _totalProcessCount)
            {
                foreach (var tracker in _cellDirtyTrackerList)
                {
                    tracker.ClearAll();
                }
                _cellDirtyTrackerList.Clear();
            }

            if (_activeProcessCount == 0)
            {
                OnAllProcessesCompleted();
            }
        }

        private void OnAllProcessesCompleted()
        {
            // Debug.Log("Match Handling completed");
            _startedProcessCount = 0;
            _totalProcessCount = 0;
            _processList.Clear();
            IsHandlerActive = false;
            OnMatchHandlingCompleted?.Invoke();
        }
    }
}

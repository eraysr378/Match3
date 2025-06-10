using System;
using System.Collections.Generic;
using BuildSystem;
using TileRelated;
using UnityEngine;

namespace Managers
{
    public class GridStabilizationChecker : MonoBehaviour
    {
        public event Action<int> OnRowStabilized;
        public event Action<int> OnColumnStabilized;
        public event Action OnGridStabilized;
        [SerializeField] private FillManager fillManager;
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private FallManager fallManager;

        private GridRelated.Grid _grid;
        private HashSet<int> _stabilizedRowSet;
        private bool[] _previousRowStable;
        private bool[] _previousColumnStable;


        private bool _isSetupCompleted;

        private void OnEnable()
        {
            GridBuilder.OnCellsCreated += Initialize;
        }

        private void OnDisable()
        {
            GridBuilder.OnCellsCreated -= Initialize;
        }

        private void Initialize(GridRelated.Grid grid)
        {
            _grid = grid;
            _previousRowStable = new bool[grid.Height];
            _previousColumnStable = new bool[grid.Width];
            _stabilizedRowSet = new HashSet<int>();
            _isSetupCompleted = true;
        }

        public bool IsGridStabilized()
        {
            return !fillManager.IsBusy()
                   && !matchManager.IsBusy()
                   && !fallManager.IsBusy()
                   && _stabilizedRowSet.Count == _grid.Height;
        }

        private void Update()
        {
            if (_isSetupCompleted == false) return;
            CheckAllRows();
            CheckAllColumns();
        }

        private void CheckAllRows()
        {
            for (int row = 0; row < _grid.Height; row++)
            {
                bool isStable = IsRowStable(row);
                if (isStable)
                {
                    _stabilizedRowSet.Add(row);
                }
                else
                {
                    _stabilizedRowSet.Remove(row);
                }

                if (!_previousRowStable[row] && isStable)
                {
                    OnRowStabilized?.Invoke(row);
                }

                _previousRowStable[row] = isStable;
            }

            if (IsGridStabilized())
            {
                OnGridStabilized?.Invoke();
            }
        }

        private void CheckAllColumns()
        {
            for (int col = 0; col < _grid.Width; col++)
            {
                bool isStable = IsColumnStable(col);
                if (!_previousColumnStable[col] && isStable)
                {
                    OnColumnStabilized?.Invoke(col);
                }

                _previousColumnStable[col] = isStable;
            }
        }

        private bool IsRowStable(int row)
        {
            for (int col = 0; col < _grid.Width; col++)
            {
                var cell = _grid.GetCellAt(row, col);

                if ((cell !=null && cell.CurrentPiece == null) ||
                    (cell != null && cell.CurrentPiece != null && cell.CurrentPiece.IsBusy()))
                    return false;
            }

            return true;
        }

        private bool IsColumnStable(int col)
        {
            for (int row = 0; row < _grid.Height; row++)
            {
                var cell = _grid.GetCellAt(row, col);
                if ((cell !=null && cell.CurrentPiece == null) ||
                    (cell != null && cell.CurrentPiece != null && cell.CurrentPiece.IsBusy()))
                    return false;
            }

            return true;
        }
    }
}
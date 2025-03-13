using System.Collections.Generic;
using Cells;
using Interfaces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        private SwapManager SwapManager { get; set; }
        private MatchManager MatchManager { get; set; }
        private ActivationManager ActivationManager { get; set; }

        private Cell _firstSelectedCell;
        private Cell _secondSelectedCell;
        private Grid _grid;

        private void Awake()
        {
            EventManager.OnPointerDownCell += OnPointerDownCellEvent;
            EventManager.OnPointerUpCell += OnPointerUpCellEvent;
            EventManager.OnPointerEnterCell += OnPointerEnterCellEvent;
            EventManager.OnPointerClickedCell += OnPointerClickedCellEvent;
            EventManager.OnGridInitialized += OnGridInitialized;

            SwapManager = GetComponent<SwapManager>();
            MatchManager = GetComponent<MatchManager>();
            ActivationManager = GetComponent<ActivationManager>();
        }
        
        private void OnGridInitialized(Grid grid)
        {
            _grid = grid;
            SwapManager.Initialize(_grid);
            SwapManager.OnSwapCompleted += SwapManager_OnSwapCompleted;
            MatchManager.Initialize(_grid);
        }
        
        private void SwapManager_OnSwapCompleted(Cell swappedFirstCell, Cell swappedSecondCell)
        {
            bool anyMatchFound = false;
            bool isAnyActivated = false;

            anyMatchFound |= MatchManager.CheckAndHandleMatch(swappedFirstCell);
            anyMatchFound |= MatchManager.CheckAndHandleMatch(swappedSecondCell);


            isAnyActivated |= ActivationManager.ActivateCell(swappedFirstCell);
            isAnyActivated |= ActivationManager.ActivateCell(swappedSecondCell);

            if (!anyMatchFound && !isAnyActivated)
            {
                SwapManager.RevertSwap();
            }

            _firstSelectedCell = null;
            _secondSelectedCell = null;
        }

        private void OnPointerEnterCellEvent(Cell cell)
        {
            if (!SwapManager.CanSwap()) return;
            if (!_firstSelectedCell || _firstSelectedCell == cell) return;
            if (cell is not ISwappable || !AreCellsAdjacent(_firstSelectedCell, cell)) return;

            _secondSelectedCell = cell;
            SwapManager.Swap(_firstSelectedCell, _secondSelectedCell);
        }

        private void OnPointerClickedCellEvent(Cell cell)
        {
            ActivationManager.ActivateCell(cell);
        }
        private void OnPointerDownCellEvent(Cell cell)
        {
            if (cell is ISwappable)
            {
                _firstSelectedCell = cell;
            }
        }
        
        private void OnPointerUpCellEvent(Cell cell)
        {
            _firstSelectedCell = null;
            _secondSelectedCell = null;
        }

        private bool AreCellsAdjacent(Cell cell1, Cell cell2)
        {
            int dx = Mathf.Abs(cell1.Row - cell2.Row);
            int dy = Mathf.Abs(cell1.Col - cell2.Col);
            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1); // Ensure adjacent swap only
        }

        public List<Cell> GetAdjacentElements(int row, int col)
        {
            List<Cell> adjacentElementList = new List<Cell>();
            // Check below
            if (row > 0)
            {
                if (_grid.GetCell(row - 1, col) != null)
                {
                    adjacentElementList.Add(_grid.GetCell(row - 1, col));
                }
            }

            // Check above
            if (row < _grid.Width - 1)
            {
                if (_grid.GetCell(row + 1, col) != null)
                {
                    adjacentElementList.Add(_grid.GetCell(row + 1, col));
                }
            }

            // Check left
            if (col > 0)
            {
                if (_grid.GetCell(row, col - 1) != null)
                {
                    adjacentElementList.Add(_grid.GetCell(row, col - 1));
                }
            }

            // Check right
            if (col < _grid.Height - 1)
            {
                if (_grid.GetCell(row, col + 1) != null)
                {
                    adjacentElementList.Add(_grid.GetCell(row, col + 1));
                }
            }

            return adjacentElementList;
        }
    }
}
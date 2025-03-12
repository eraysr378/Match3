using System.Collections.Generic;
using Cells;
using Interfaces;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace GridRelated
{
    public class GridManager : MonoBehaviour
    {
        
        public static GridManager Instance { get; private set; }
        [SerializeField] private GridPropertiesSo propertiesSo;
        [SerializeField] private int minSortingOrder;
        private Cell _firstCell;
        private Cell _secondCell;
        private Grid _grid;
        private List<Cell> _matchList = new List<Cell>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _grid = propertiesSo.grid;
            EventManager.OnPointerDownCell += OnPointerDownCellEvent;
            EventManager.OnPointerUpCell += OnPointerUpCellEvent;
            EventManager.OnPointerEnterCell += OnPointerEnterCellEvent;
        }

        private void OnPointerEnterCellEvent(Cell cell)
        {
            if (_firstCell == null || _firstCell == cell) return;
            if (cell is not ISwappable || !AreCellsAdjacent(_firstCell, cell)) return;

            _secondCell = cell;

            // 🔹 Swap visually & in grid
            _grid.SwapCells(_firstCell, _secondCell);
            (_firstCell as ISwappable)?.Swap(_secondCell);

            // 🔹 Check for matches
            bool matchFound = HandleMatches(); // NEW: Check for & remove matches
            bool shouldActivate = _firstCell is IActivatable || _secondCell is IActivatable;

            if (!matchFound && !shouldActivate)
            {
                // 🔹 No match & no activation → Swap back
                _grid.SwapCells(_firstCell, _secondCell);
                (_firstCell as ISwappable)?.Swap(_secondCell);
            }
            else if (shouldActivate)
            {
                // 🔹 Activate special cells (e.g., bomb explosions)
                (_firstCell as IActivatable)?.Activate();
                (_secondCell as IActivatable)?.Activate();
            }

            _firstCell = null;
            _secondCell = null;
        }

        private bool CheckIsMatch(int row, int col)
        {
            int count = 1;
            CellType matchingCellType = _grid.GetCell(row, col).GetCellType();
            for (int i = col+1; i < _grid.Width; i++)
            {
                if (_grid.GetCell(row, i).GetCellType() != matchingCellType)
                {
                    break;
                }
                count++;
            }
            for (int i = col-1; i >= 0; i--)
            {
                if (_grid.GetCell(row, i).GetCellType() != matchingCellType)
                {
                    break;
                }
                count++;

            }
            for (int i = row-1; i >= 0; i--)
            {
                if (_grid.GetCell(i, col).GetCellType() != matchingCellType)
                {
                    break;
                }

                count++;
            }
            for (int i = row+1; i < _grid.Height; i++)
            {
                if (_grid.GetCell(i, col).GetCellType() != matchingCellType)
                {
                    break;
                }  
                count++;
            }

            return count >= 3;
        }
        private void CheckMatchRecursively(int row, int col,CellType matchingCellType)
        {
            Cell cell = _grid.GetCell(row, col);
            if (cell == null)
            {
                return;
            }
            // Do not check the element if it is checked already
            if (cell.IsChecked())
            {
                return;
            }

            cell.Check();
            // If the match chain continues with cube, enlarge the chain
            if (cell.TryGetComponent<IMatchable>(out var matchableCell) && cell.GetCellType() == matchingCellType)
            {
                _matchList.Add(cell);
                // Check below
                if (row > 0)
                {
                    CheckMatchRecursively(row - 1, col, matchingCellType);
                }
                // Check above
                if (row < propertiesSo.width - 1)
                {
                    CheckMatchRecursively(row + 1, col, matchingCellType);
                }
                // Check left
                if (col > 0)
                {
                    CheckMatchRecursively(row, col - 1, matchingCellType);
                }
                // Check right
                if (col < propertiesSo.width - 1)
                {
                    CheckMatchRecursively(row, col + 1, matchingCellType);
                }
            }
        }
        private void GetMatchingCells(int row, int col)
        {
            CellType type = _grid.GetCell(row, col).GetCellType();

            _matchList.Clear();
            _grid.UncheckAll();
            
            CheckMatchRecursively(row, col,type);
        }
        private bool HandleMatches()
        {
            bool rval = false;
            if (CheckIsMatch(_firstCell.Row, _firstCell.Col))
            {
                GetMatchingCells(_firstCell.Row, _firstCell.Col);
                foreach (var matchCell in _matchList)
                {
                    Destroy(matchCell.gameObject);
                }

                rval = true;
            }
            if (CheckIsMatch(_secondCell.Row, _secondCell.Col))
            {
                GetMatchingCells(_secondCell.Row, _secondCell.Col);
                foreach (var matchCell in _matchList)
                {
                    Destroy(matchCell.gameObject);
                }

                rval = true;
            }

            return rval;

        }
        private void OnPointerDownCellEvent(Cell cell)
        {
            if (cell is ISwappable swappable)
            {
                _firstCell = cell;
            }
        }
        private void OnPointerUpCellEvent(Cell cell)
        {
            _firstCell = null;
            _secondCell = null;
        }
        private void SwapCellsInGrid(Cell cell1, Cell cell2)
        {
            _grid.SwapCells(cell1, cell2);
            Debug.Log($"Swapped {cell1} with {cell2}");
        }
        private bool AreCellsAdjacent(Cell cell1, Cell cell2)
        {
            int dx = Mathf.Abs(cell1.Row - cell2.Row);
            int dy = Mathf.Abs(cell1.Col - cell2.Col);
            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1); // Ensure adjacent swap only
        }
      

        private void OnDisable()
        {
        }

        // Return the adjacent elements to the element which is in the given row and column of the Grid
        public List<Cell> GetAdjacentElements(int row, int col)
        {
            List<Cell> adjacentElementList = new List<Cell>();
            // Check below
            if (row > 0)
            {
                if (propertiesSo.grid.GetCell(row - 1, col) != null)
                {
                    adjacentElementList.Add(propertiesSo.grid.GetCell(row - 1, col));
                }
            }

            // Check above
            if (row < propertiesSo.width - 1)
            {
                if (propertiesSo.grid.GetCell(row + 1, col) != null)
                {
                    adjacentElementList.Add(propertiesSo.grid.GetCell(row + 1, col));
                }
            }

            // Check left
            if (col > 0)
            {
                if (propertiesSo.grid.GetCell(row, col - 1) != null)
                {
                    adjacentElementList.Add(propertiesSo.grid.GetCell(row, col - 1));
                }
            }

            // Check right
            if (col < propertiesSo.height - 1)
            {
                if (propertiesSo.grid.GetCell(row, col + 1) != null)
                {
                    adjacentElementList.Add(propertiesSo.grid.GetCell(row, col + 1));
                }
            }

            return adjacentElementList;
        }
    }
}
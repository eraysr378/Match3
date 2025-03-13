using System.Collections.Generic;
using Cells;
using Interfaces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class MatchManager : MonoBehaviour
    {
        private List<Cell> _matchList = new List<Cell>();

        private Grid _grid;

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }

        public bool CheckAndHandleMatch(Cell cell)
        {
            if (cell is not IMatchable) return false;
            bool rval = false;

            if (CheckIsMatch(cell.Row, cell.Col))
            {
                _matchList = GetMatchingCells(cell.Row, cell.Col);
                foreach (var matchCell in _matchList)
                {
                    Destroy(matchCell.gameObject);
                }

                rval = true;
            }
            

            return rval;
        }

        public bool HandleMatch(Cell cell)
        {
            return false;
        }

        public bool CheckIsMatch(int row, int col)
        {
            int count = 1;

            CellType matchingCellType = _grid.GetCell(row, col).GetCellType();
            for (int i = col + 1; i < _grid.Width; i++)
            {
                if (_grid.GetCell(row, i).GetCellType() != matchingCellType)
                {
                    break;
                }
                count++;
            }

            for (int i = col - 1; i >= 0; i--)
            {
                if (_grid.GetCell(row, i).GetCellType() != matchingCellType)
                {
                    break;
                }
                count++;
            }

            for (int i = row - 1; i >= 0; i--)
            {
                if (_grid.GetCell(i, col).GetCellType() != matchingCellType)
                {
                    break;
                }
                count++;
            }

            for (int i = row + 1; i < _grid.Height; i++)
            {
                if (_grid.GetCell(i, col).GetCellType() != matchingCellType)
                {
                    break;
                }
                count++;
            }

            return count >= 3;
        }

        private void CheckMatchRecursively(int row, int col, CellType matchingCellType)
        {
            Cell cell = _grid.GetCell(row, col);
            if (cell == null)
            {
                return;
            }
            if (cell.IsChecked())
            {
                return;
            }

            cell.Check();

            if (cell.TryGetComponent<IMatchable>(out var matchableCell) && cell.GetCellType() == matchingCellType)
            {
                _matchList.Add(cell);
                // Check below
                if (row > 0)
                {
                    CheckMatchRecursively(row - 1, col, matchingCellType);
                }

                // Check above
                if (row < _grid.Width - 1)
                {
                    CheckMatchRecursively(row + 1, col, matchingCellType);
                }

                // Check left
                if (col > 0)
                {
                    CheckMatchRecursively(row, col - 1, matchingCellType);
                }

                // Check right
                if (col < _grid.Height - 1)
                {
                    CheckMatchRecursively(row, col + 1, matchingCellType);
                }
            }
        }

        public List<Cell> GetMatchingCells(int row, int col)
        {
            CellType type = _grid.GetCell(row, col).GetCellType();

            _matchList.Clear();
            _grid.UncheckAll();

            CheckMatchRecursively(row, col, type);
            return _matchList;
        }
    }
}
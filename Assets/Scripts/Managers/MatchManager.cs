using System.Collections.Generic;
using Cells;
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
            bool anyMatchFound = false;
            for (int row = 0; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Cell cell = _grid.GetCell(row, col);
                    if (cell is IMatchable)
                    {
                        List<Cell> matchList = GetMatch(cell);
                        if (matchList != null)
                        {
                            for (int i = 0; i < matchList.Count; i++)
                            {
                                matchList[i].DestroyCell();
                                anyMatchFound = true;
                            }
                        }
                    }
                }
            }

            return anyMatchFound;
        }

        public List<Cell> GetMatch(Cell cell)
        {
            if (cell is not IMatchable) return null;
            List<Cell> matchingCells = new List<Cell>();
            List<Cell> verticalCells;

            List<Cell> horizontalCells = GetHorizontalMatch(cell);
            horizontalCells.Add(cell);

            if (horizontalCells.Count >= 3)
            {
                matchingCells.AddRange(horizontalCells);
                foreach (var horizontalCell in horizontalCells)
                {
                    verticalCells = GetVerticalMatch(horizontalCell);
                    if (verticalCells.Count < 2)
                    {
                        verticalCells.Clear();
                    }
                    else
                    {
                        matchingCells.AddRange(verticalCells);
                        break;
                    }
                }
            }
            
            if (matchingCells.Count >= 3)
            {
                return matchingCells;
            }
            horizontalCells.Clear();
            verticalCells = GetVerticalMatch(cell );
            verticalCells.Add(cell);

            if (verticalCells.Count >= 3)
            {
                matchingCells.AddRange(verticalCells);
                foreach (var verticalCell in verticalCells)
                {
                    horizontalCells = GetHorizontalMatch(verticalCell);
                    if (horizontalCells.Count < 2)
                    {
                        horizontalCells.Clear();
                    }
                    else
                    {
                        matchingCells.AddRange(horizontalCells);
                        break;
                    }
                }
            }
            if (matchingCells.Count >= 3)
            {
                return matchingCells;
            }

            return null;
        }

        private List<Cell> GetVerticalMatch(Cell cell)
        {
            List<Cell> verticalCells = new List<Cell>();
            CellType cellType = cell.GetCellType();
            for (int row = cell.Row - 1; row >= 0; row--)
            {
                Cell cellBelow = _grid.GetCell(row, cell.Col);
                if (cellBelow is not IMatchable || cellBelow.GetCellType() != cellType)
                    break;
                
                verticalCells.Add(cellBelow);
            }

            for (int row = cell.Row + 1; row < _grid.Height; row++)
            {
                Cell cellAbove = _grid.GetCell(row, cell.Col);
                if (cellAbove is not IMatchable || cellAbove.GetCellType() != cellType)
                    break;
                
                verticalCells.Add(cellAbove);
            }
            return verticalCells;
        }

        private List<Cell> GetHorizontalMatch(Cell cell)
        {
            List<Cell> horizontalCells = new List<Cell>();
            CellType cellType = cell.GetCellType();
            for (int col = cell.Col - 1; col >= 0; col--)
            {
                Cell cellLeft = _grid.GetCell(cell.Row, col);
                if (cellLeft is not IMatchable || cellLeft.GetCellType() != cellType)
                    break;
                horizontalCells.Add(cellLeft);
            }
            for (int col = cell.Col +1; col < _grid.Width; col++)
            {
                Cell cellRight = _grid.GetCell(cell.Row, col);
                if (cellRight is not IMatchable || cellRight.GetCellType() != cellType)
                    break;
                horizontalCells.Add(cellRight);
            }
            return horizontalCells;
        }
    }
}
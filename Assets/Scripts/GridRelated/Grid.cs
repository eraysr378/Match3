using System;
using Cells;
using UnityEngine;

namespace GridRelated
{
    [Serializable]
    public class Grid
    {
        public int Height => height;
        public int Width => width;
        [SerializeField] private int width;
        [SerializeField] private int height;
        private Cell[,] _gridArray;
        public Grid(int rows, int columns)
        {
            height = rows;
            width = columns;
            _gridArray = new Cell[rows, columns];
        }

        public void SwapCells(Cell cell1, Cell cell2)
        {
            _gridArray[cell1.Row, cell1.Col] = cell2;
            _gridArray[cell2.Row, cell2.Col] = cell1;
        }
        public void SetCell(int row, int col, Cell cell)
        {
            if (row >= 0 && row < height && col >= 0 && col < width)
            {
                _gridArray[row, col] = cell;
            }
        }
        public Cell GetCell(int row, int col)
        {
            if (row >= 0 && row < height && col >= 0 && col < width)
            {
                return _gridArray[row, col];
            }
            return null;
        }
        public void RemoveCell(int row, int col)
        {
            if (row >= 0 && row < height && col >= 0 && col < width)
            {
                _gridArray[row, col] = null;
            }
        }
        public void UncheckAll()
        {
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Cell cell = _gridArray[row, col];
                    if (cell != null)
                    {
                        cell.Uncheck();
                    }
                }
            }
        }



    }
}
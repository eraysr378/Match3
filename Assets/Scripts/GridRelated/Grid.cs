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

        public void SetCellAt(int row, int col, Cell cell)
        {
            if (row >= 0 && row < height && col >= 0 && col < width)
            {
                _gridArray[row, col] = cell;
            }
        }

        public Cell GetCellAt(int row, int col)
        {
            if (row >= 0 && row < height && col >= 0 && col < width)
            {
                return _gridArray[row, col];
            }

            return null;
        }

        public void RemoveCellAt(int row, int col)
        {
            if (row >= 0 && row < height && col >= 0 && col < width)
            {
                _gridArray[row, col] = null;
            }
        }
    }
}
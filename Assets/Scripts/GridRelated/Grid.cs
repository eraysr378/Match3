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
        private BaseCell[,] _gridArray;

        public Grid(int rows, int columns)
        {
            height = rows;
            width = columns;
            _gridArray = new BaseCell[rows, columns];
        }

        public void SetCellAt(int row, int col, BaseCell baseCell)
        {
            if (row >= 0 && row < height && col >= 0 && col < width)
            {
                _gridArray[row, col] = baseCell;
            }
        }

        public BaseCell GetCellAt(int row, int col)
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
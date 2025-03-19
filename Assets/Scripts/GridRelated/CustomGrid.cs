using System;
using Misc;
using UnityEngine;

namespace GridRelated
{
    [Serializable]
    public class CustomGrid
    {
        public int height;
        public int width;

        [SerializeReference] 
        public CustomGridCell[] cellData;

        public CustomGrid(int height, int width)
        {
            this.height = height;
            this.width = width;
            cellData = new CustomGridCell[height * width];

            // Ensure each cell is properly instantiated
            for (int i = 0; i < cellData.Length; i++)
            {
                cellData[i] = new CustomGridCell(); 
            }
        }

        public CustomGridCell GetElement(int row, int col)
        {
            return cellData[row * width + col];
        }
    }

    [Serializable]
    public class CustomGridCell
    {
        public CellType cellType;
        public PieceType pieceType;
    }
}
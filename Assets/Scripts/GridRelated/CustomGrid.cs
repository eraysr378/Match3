using System;

namespace GridRelated
{
    [Serializable]
    public class CustomGrid
    {
        public int height;
        public int width;
        public CellType[] data; // Flatten the array to store 2D grid data because it is easy to show in inspector

        public CustomGrid(int height, int width)
        {
            this.height = height;
            this.width = width;
            data = new CellType[height * width];
        }
        public CellType GetElement(int row, int col)
        {
            // Return the data as if the array is 2D
            return data[row * width + col];
        }
    }
}
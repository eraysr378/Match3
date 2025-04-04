using Pieces;
using ScriptableObjects;
using UnityEngine;

namespace GridRelated
{
    public static class GridUtility
    {
        public static GridPropertiesSo PropertiesSo;
        // Return the grid position that corresponds to the given world position
        public static Vector2 WorldPositionToGridPosition(Vector3 worldPos)
        {
            float col = -(PropertiesSo.gridPlaygroundCenter.x + PropertiesSo.gridOffset.x - worldPos.x) / PropertiesSo.cellSize;
            float row = -(PropertiesSo.gridPlaygroundCenter.y + PropertiesSo.gridOffset.y - worldPos.y) / PropertiesSo.cellSize;

            col = Mathf.Round(col);
            row = Mathf.Round(row);
            return new Vector2(row,col);
        }

        public static Vector3 GridPositionToWorldPosition(int row, int col)
        {
            float spacingRatio = PropertiesSo.spacingRatio;

            // Compute the additional spacing offset
            float spacingX = col * spacingRatio;
            float spacingY = row * spacingRatio;

            // Calculate the new position with spacing
            float x = PropertiesSo.gridPlaygroundCenter.x + PropertiesSo.gridOffset.x + col * PropertiesSo.cellSize + spacingX;
            float y = PropertiesSo.gridPlaygroundCenter.y + PropertiesSo.gridOffset.y + row * PropertiesSo.cellSize + spacingY;

            return new Vector3(x, y, 0);
        }

    }
}

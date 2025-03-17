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
            float col = -(PropertiesSo.gridPlaygroundCenter.x + PropertiesSo.gridOffset.x - worldPos.x) / PropertiesSo.elementSize;
            float row = -(PropertiesSo.gridPlaygroundCenter.y + PropertiesSo.gridOffset.y - worldPos.y) / PropertiesSo.elementSize;

            col = Mathf.Round(col);
            row = Mathf.Round(row);
            return new Vector2(row,col);
        }
        // Return the world position on the grid for given row,col and element
        public static Vector3 GridPositionToWorldPosition(int row, int col)
        {
            // Set the initial position for adjacent placement, adjusted for the grid offset
            float x = PropertiesSo.gridPlaygroundCenter.x + PropertiesSo.gridOffset.x + col * PropertiesSo.elementSize;
            float y = PropertiesSo.gridPlaygroundCenter.y + PropertiesSo.gridOffset.y + row * PropertiesSo.elementSize;

            Vector3 gridCellCenter = new Vector3(x, y, 0);

            // // Adjust the position using the collider's center
            // Collider2D collider = piece.Collider;
            // if (collider)
            // {
            //     // Make sure bounds are calculated correctly
            //     collider.enabled = false;
            //     collider.enabled = true;
            //
            //     Vector3 colliderOffset = collider.bounds.center - piece.transform.position;
            //     return gridCellCenter - colliderOffset;
            // }
            // If no collider is found, fallback to direct grid alignment
            return gridCellCenter;
        }
    }
}

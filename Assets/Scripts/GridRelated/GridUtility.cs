using Cells;
using UnityEngine;

namespace GridRelated
{
    public class GridUtility
    {
        // Return the grid position that corresponds to the given world position
        public static Vector2 WorldPositionToGridPosition(Vector3 worldPos,Vector3 gridPlaygroundCenter,Vector2 gridOffset,float elementSize)
        {
            float col = -(gridPlaygroundCenter.x + gridOffset.x - worldPos.x) / elementSize;
            float row = -(gridPlaygroundCenter.y + gridOffset.y - worldPos.y) / elementSize;

            col = Mathf.Round(col);
            row = Mathf.Round(row);
            return new Vector2(row,col);
        }
        // Return the world position on the grid for given row,col and element
        public static Vector3 GridPositionToWorldPosition(int row, int col, Cell element,Vector3 gridPlaygroundCenter,Vector2 gridOffset,float elementSize)
        {
            // Set the initial position for adjacent placement, adjusted for the grid offset
            float x = gridPlaygroundCenter.x + gridOffset.x + col * elementSize;
            float y = gridPlaygroundCenter.y + gridOffset.y + row * elementSize;

            Vector3 gridCellCenter = new Vector3(x, y, 0);

            // Adjust the position using the collider's center
            Collider2D collider = element.GetComponent<Collider2D>();
            if (collider != null)
            {
                // Make sure bounds are calculated correctly
                collider.enabled = false;
                collider.enabled = true;

                Vector3 colliderOffset = collider.bounds.center - element.transform.position;
                return gridCellCenter - colliderOffset;
            }
            else
            {
                // If no collider is found, fallback to direct grid alignment
                return gridCellCenter;
            }
        }
    }
}

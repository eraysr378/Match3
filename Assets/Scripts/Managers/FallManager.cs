using Cells;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class FallManager : MonoBehaviour
    {
        private Grid _grid;

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }

        private void OnEnable()
        {
            // EventManager.OnFillCompleted += ProcessFalling;
        }

        private void OnDisable()
        {
            // EventManager.OnFillCompleted -= ProcessFalling;
        }

        private void ProcessFalling()
        {
            for (int col = 0; col < _grid.Width; col++)
            {
                HandleFallingForColumn(col);
            }
            
            EventManager.OnFallCompleted?.Invoke();
        }

        private void HandleFallingForColumn(int col)
        {
            for (int row = _grid.Height - 1; row >= 0; row--)
            {
                if (_grid.GetCell(row, col) != null) break;

                Cell newCell = EventManager.OnRequestRandomCellSpawn(_grid.Height, col);

                _grid.SetCell(row, col, newCell);
                newCell.SetPosition(row, col);
                // newCell.UpdatePosition(row, col);
            }
        }
    }
}
using CellOverlays;
using Managers;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace BuildSystem
{
    public class CellOverlayBuilder
    {
        public void Build(GridDataSo gridData, Grid grid)
        {
            if (gridData.cellOverlayDataArray == null)
            {
                Debug.Log("No cell overlay data found");
                return;
            }
            foreach (var overlayData in gridData.cellOverlayDataArray)
            {
                var cell = grid.GetCellAt(overlayData.row, overlayData.column);
                if (cell == null)
                {
                    UnityEngine.Debug.LogWarning($"No cell found at ({overlayData.row}, {overlayData.column}) to add glass overlay.");
                    continue;
                }

                var glass = EventManager.OnCellOverlaySpawnRequested(overlayData.overlayType,cell.transform.position);
                cell.SetOverlay(glass);
                glass.SetCell(cell);
            }
        }
    }
}
using Cells;
using Interfaces;
using Managers;
using Misc;
using UnityEngine;

namespace BuildSystem
{
    public class CellBuilder : IGridBuildProcess
    {

        public void Build(GridDataSo gridData, GridRelated.Grid grid)
        {
            foreach (var cellData in gridData.cellDataArray)
            {
                CellType cellType = cellData.cellType;
                Vector3 position = new Vector3(cellData.column + 0.5f, cellData.row + 0.5f, 0);
                
                BaseCell createdCell = EventManager.RequestCellSpawn?.Invoke(cellType, position);
                createdCell?.SetPosition(cellData.row, cellData.column);

                grid.SetCellAt(cellData.row, cellData.column, createdCell);
            }

        }
    }
}
using Cells;
using Factories.CellFactories;
using GridRelated;
using Managers;
using Misc;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spawners
{
    public class CellSpawner : MonoBehaviour
    {
        [SerializeField] private GeneralCellFactory cellFactory;
        [SerializeField] private Transform cellParent;
        public void OnEnable()
        {
            EventManager.OnCellSpawnRequested += SpawnCell;
        }

        private void OnDisable()
        {
            EventManager.OnCellSpawnRequested -= SpawnCell;
        }

 
        private Cell SpawnCell(CellType cellType, int row, int col)
        {
            Cell cell = cellFactory.CreateCellBasedOnType(cellType);
            if (cell == null) return null;
            
            Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col);
            cell.transform.position = pos;
            cell.transform.localScale = Vector3.one * GridUtility.PropertiesSo.cellSize;

            cell.SetPosition(row,col);

            return cell;
        }
        
    }
}
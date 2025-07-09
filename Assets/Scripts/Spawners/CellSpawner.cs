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
            EventManager.RequestCellSpawn += SpawnCell;
        }

        private void OnDisable()
        {
            EventManager.RequestCellSpawn -= SpawnCell;
        }

 
        private BaseCell SpawnCell(CellType cellType, Vector3 position)
        {
            BaseCell baseCell = cellFactory.CreateCellBasedOnType(cellType);
            if (baseCell == null) return null;
            
            // Vector2 pos = GridUtility.GridPositionToWorldPosition(row, col);
            baseCell.transform.position = position;
            // cell.transform.localScale = Vector3.one * GridUtility.PropertiesSo.cellSize;
            baseCell.transform.localScale = Vector3.one;

            // cell.SetPosition(row,col);

            return baseCell;
        }
        
        
    }
}
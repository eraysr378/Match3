using Cells;
using Misc;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "ObstacleCellFactory", menuName = "Factories/ObstacleCellFactory")]
    public class ObstacleCellFactory : CellFactory
    {
        [SerializeField] private ObstacleCell prefab;
        
        public override Cell CreateCell(CellType cellType)
        {
            ObstacleCell obstacleCell = Instantiate(prefab);
            obstacleCell.SetCellType(cellType);
            return obstacleCell;
        }
    }

}
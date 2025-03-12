using System;
using Cells;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "GeneralCellFactory", menuName = "Factories/GeneralCellFactory")]

    public class GeneralCellFactory : CellFactory
    {
        [SerializeField] private NormalCellFactory normalCellFactory;
        [SerializeField] private ObstacleCellFactory obstacleCellFactory;
        
        public override Cell CreateCell(CellType cellType)
        {
            Cell cell = null;
            switch (cellType)
            {
                case CellType.SquareNormalCell or CellType.CircleNormalCell or CellType.TriangleNormalCell:
                    cell = normalCellFactory.CreateCell(cellType);
                    break;
                case CellType.ObstacleCell:
                    cell = obstacleCellFactory.CreateCell(cellType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null);
            }
            return cell;
        }


    }
}

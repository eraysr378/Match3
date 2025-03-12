using System;
using Cells;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "NormalCellFactory", menuName = "Factories/NormalCellFactory")]
    public class NormalCellFactory : CellFactory
    {
        [SerializeField] private NormalCell prefab;
        public override Cell CreateCell(CellType cellType)
        {
            NormalCell normalCell = Instantiate(prefab);
            normalCell.SetCellType(cellType);
            return normalCell;
        }
    }
}

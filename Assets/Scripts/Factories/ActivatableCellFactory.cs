using Cells;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "ActivatableCellFactory", menuName = "Factories/ActivatableCellFactory")]

    public class ActivatableCellFactory : CellFactory
    {
        [SerializeField] private ActivatableCell prefab;

        public override Cell CreateCell(CellType cellType)
        {
            ActivatableCell activatableCell = Instantiate(prefab);
            activatableCell.SetCellType(cellType);
            return activatableCell;
        }
    }
}
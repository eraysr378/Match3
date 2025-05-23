using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    [CreateAssetMenu(fileName = "DisabledCellFactory", menuName = "Factories/Cell/DisabledCellFactory")]
    public class DisabledCellFactory : BaseCellFactory
    {
        [SerializeField] private DisabledCell prefab;

        public override Cell CreateCell(CellType cellType)
        {
            DisabledCell disabledCell = Instantiate(prefab);
            return disabledCell;
        }
    }
}
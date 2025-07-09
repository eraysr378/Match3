using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    [CreateAssetMenu(fileName = "DisabledCellFactory", menuName = "Factories/Cell/DisabledCellFactory")]
    public class DisabledCellFactory : BaseCellFactory
    {
        [SerializeField] private DisabledCell prefab;

        public override BaseCell CreateCell(CellType cellType,Transform parent = null)
        {
            DisabledCell disabledCell = Instantiate(prefab,parent);
            return disabledCell;
        }
    }
}
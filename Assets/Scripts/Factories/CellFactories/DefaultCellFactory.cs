using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    [CreateAssetMenu(fileName = "DefaultCellFactory", menuName = "Factories/Cell/DefaultCellFactory")]
    public class DefaultCellFactory : BaseCellFactory
    {
        [SerializeField] private DefaultCell prefab;

        public override BaseCell CreateCell(CellType cellType,Transform parent = null)
        {
            DefaultCell defaultCell = Instantiate(prefab,parent);
            return defaultCell;
        }
    }
}
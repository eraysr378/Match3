using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    [CreateAssetMenu(fileName = "DefaultCellFactory", menuName = "Factories/Cell/DefaultCellFactory")]
    public class DefaultCellFactory : BaseCellFactory
    {
        [SerializeField] private DefaultCell prefab;

        public override Cell CreateCell(CellType cellType)
        {
            DefaultCell defaultCell = Instantiate(prefab);
            return defaultCell;
        }
    }
}
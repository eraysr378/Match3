using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    [CreateAssetMenu(fileName = "DefaultCellFactory", menuName = "Factories/DefaultCellFactory")]
    public class DefaultCellFactory : CellFactory
    {
        [SerializeField] private DefaultCell prefab;

        public override Cell CreateCell(CellType cellType)
        {
            DefaultCell defaultCell = Instantiate(prefab);
            return defaultCell;
        }
    }
}
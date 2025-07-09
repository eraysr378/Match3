using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    [CreateAssetMenu(fileName = "RedCellFactory", menuName = "Factories/Cell/RedCellFactory")]
    public class RedCellFactory : BaseCellFactory
    {
        [SerializeField] private RedCell prefab;

        public override BaseCell CreateCell(CellType cellType,Transform parent = null)
        {
            RedCell redCell = Instantiate(prefab,parent);
            return redCell;
        }
    }
}
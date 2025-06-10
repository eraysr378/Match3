using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    public abstract class BaseCellFactory: ScriptableObject
    {
        public abstract BaseCell CreateCell(CellType cellType);

    }
}
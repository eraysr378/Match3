using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    public abstract class BaseCellFactory: ScriptableObject
    {
        public abstract Cell CreateCell(CellType cellType);

    }
}
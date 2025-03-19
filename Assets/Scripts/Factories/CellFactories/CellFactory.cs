using Cells;
using Misc;
using UnityEngine;

namespace Factories.CellFactories
{
    public abstract class CellFactory: ScriptableObject
    {
        public abstract Cell CreateCell(CellType cellType);

    }
}
using Cells;
using UnityEngine;

namespace Factories
{
    public abstract class CellFactory : ScriptableObject
    {
        public abstract Cell CreateCell(CellType cellType);

    }
}

    
    
    

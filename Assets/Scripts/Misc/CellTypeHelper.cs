using System;
using System.Collections.Generic;
using System.Linq;

namespace Misc
{
    public class CellTypeHelper
    {
        private static readonly HashSet<CellType> NormalCells;

        static CellTypeHelper()
        {
            NormalCells = Enum.GetValues(typeof(NormalCellType))
                .Cast<NormalCellType>()
                .Select(type => (CellType)type)
                .ToHashSet();
        }
        public static bool  IsNormalCell(CellType cellType) => NormalCells.Contains(cellType);
    }
}
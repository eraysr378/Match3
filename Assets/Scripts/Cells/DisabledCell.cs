using System;
using Misc;

namespace Cells
{
    public class DisabledCell : Cell
    {
        private void Awake()
        {
            cellType = CellType.Disabled;
        }
    }
}
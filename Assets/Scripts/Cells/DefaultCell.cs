using System;
using Misc;

namespace Cells
{
    public class DefaultCell : Cell
    {
        private void Awake()
        {
            cellType = CellType.Default;
        }
    }
}
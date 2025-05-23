using System;
using Misc;

namespace Cells
{
    public class RedCell : Cell
    {
        private void Awake()
        {
            cellType = CellType.Red;
        }
    }
}
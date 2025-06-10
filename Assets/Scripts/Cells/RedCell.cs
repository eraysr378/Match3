using System;
using Misc;

namespace Cells
{
    public class RedCell : BaseCell
    {
        private void Awake()
        {
            cellType = CellType.Red;
        }
    }
}
using System;
using Misc;

namespace Cells
{
    public class DefaultCell : BaseCell
    {
        private void Awake()
        {
            cellType = CellType.Default;
        }
    }
}
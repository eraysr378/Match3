using System;
using Misc;

namespace Cells
{
    public class DisabledCell : BaseCell
    {
        private void Awake()
        {
            cellType = CellType.None;
        }
    }
}
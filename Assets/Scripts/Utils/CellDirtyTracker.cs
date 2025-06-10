using System.Collections.Generic;
using Cells;
using UnityEngine;

namespace Utils
{
    public class CellDirtyTracker
    {
        private readonly HashSet<BaseCell> _dirtyCells = new();

        public void Mark(IEnumerable<BaseCell> cells)
        {
            foreach (var cell in cells)
            {
                if (cell == null || _dirtyCells.Contains(cell)) continue;
                _dirtyCells.Add(cell);
                cell.MarkDirty();
            }
        }

        public HashSet<BaseCell> GetDirtyCells()
        {
            return _dirtyCells;
        }
        public void Mark(BaseCell cell)
        {
            if (cell == null || _dirtyCells.Contains(cell)) return;
            cell.MarkDirty();
            _dirtyCells.Add(cell);
        }

        public void Clear(BaseCell cell)
        {
            if (!_dirtyCells.Contains(cell)) return;
            cell.ClearDirty();
            _dirtyCells.Remove(cell);
        }

        public void ClearAll()
        {
            foreach (var cell in _dirtyCells)
            {
                cell.ClearDirty();
            }

            _dirtyCells.Clear();
        }
    }
}
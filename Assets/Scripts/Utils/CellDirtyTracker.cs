using System.Collections.Generic;
using Cells;

public class CellDirtyTracker
{
    private readonly HashSet<Cell> _dirtyCells = new();

    public void Mark(IEnumerable<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (_dirtyCells.Contains(cell)) continue;
            _dirtyCells.Add(cell);
            cell.MarkDirty();
        }
    }

    public void Mark(Cell cell)
    {
        if (_dirtyCells.Contains(cell)) return;
        cell.MarkDirty();
        _dirtyCells.Add(cell);
    }

    public void Clear(Cell cell)
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
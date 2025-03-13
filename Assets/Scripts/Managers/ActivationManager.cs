using System;
using UnityEngine;
using Cells;
using Interfaces;

namespace Managers
{
    public class ActivationManager : MonoBehaviour
    {
        public event Action<Cell> OnCellActivated;

        public bool ActivateCell(Cell cell)
        {
            if (cell is IActivatable activatable)
            {
                activatable.Activate();
                OnCellActivated?.Invoke(cell);
                return true;
            }
            return false;
        }

        public void ActivateCells(params Cell[] cells)
        {
            foreach (var cell in cells)
            {
                ActivateCell(cell);
            }
        }
    }
}
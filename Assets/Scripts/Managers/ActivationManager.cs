using System;
using UnityEngine;
using Pieces;
using Interfaces;

namespace Managers
{
    public class ActivationManager : MonoBehaviour
    {
        public event Action<Piece> OnCellActivated;

        public bool ActivateCell(Piece piece)
        {
            if (piece is IActivatable activatable)
            {
                activatable.Activate();
                OnCellActivated?.Invoke(piece);
                return true;
            }
            return false;
        }

        public void ActivateCells(params Piece[] cells)
        {
            foreach (var cell in cells)
            {
                ActivateCell(cell);
            }
        }
    }
}
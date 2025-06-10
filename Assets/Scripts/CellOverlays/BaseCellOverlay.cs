using System;
using Cells;
using UnityEngine;

namespace CellOverlays
{
    public class BaseCellOverlay : MonoBehaviour
    {
        public event Action<BaseCellOverlay> OnDestroyed;
        protected BaseCell cell;

        public void SetCell(BaseCell cell)
        {
            this.cell = cell;
        }

        protected virtual void DestroySelf()
        {
            OnDestroyed?.Invoke(this);
            Destroy(gameObject);
        }
    }

  
}
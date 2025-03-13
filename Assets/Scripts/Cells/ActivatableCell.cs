using Interfaces;
using UnityEngine;

namespace Cells
{
    public class ActivatableCell : Cell, IActivatable,ISwappable
    {
        public bool IsActivated { get; private set; }

        public void Activate()
        {
            if (IsActivated)
            {
                return;
            }
            IsActivated = true;
            Debug.Log("Activated");
        }


        public void Swap(Cell other)
        {
            throw new System.NotImplementedException();
        }
    }
}
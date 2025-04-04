using Interfaces;
using Managers;
using Pieces;
using UnityEngine;

namespace Handlers
{
    public abstract class InputHandler : MonoBehaviour
    {
        protected void ProcessInput(Piece piece1, Piece piece2)
        {
            if (piece1 is ICombinable && piece2 is ICombinable)
            {
                Debug.Log("Combination requested");
                EventManager.OnCombinationRequested?.Invoke(piece1, piece2);
                return;
            }

            EventManager.OnSwapRequested?.Invoke(piece1, piece2);
        }

        public abstract void Enable();
        public abstract void Disable();
    }
}
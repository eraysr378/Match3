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

            if (piece1 is ISwappable && piece2 is ISwappable)
            {
                Debug.Log($"SWAP requested {piece1.name} {piece2.name}");
                EventManager.OnSwapRequested?.Invoke(piece1, piece2);
                return;
            }
        }

        public abstract void Enable();
        public abstract void Disable();
    }
}
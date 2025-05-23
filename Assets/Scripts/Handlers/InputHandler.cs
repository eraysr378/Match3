using Cells;
using Interfaces;
using Managers;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;

namespace Handlers
{
    public abstract class InputHandler : MonoBehaviour
    {
        [SerializeField] protected MoveManager moveManager;
        [SerializeField] protected SwapManager swapManager;
        protected void ProcessInput(Cell cell1, Cell cell2)
        {
            var piece1 = cell1.CurrentPiece;
            var piece2 = cell2.CurrentPiece;
            if (piece1 == null || piece2 == null || piece1.IsBusy()|| piece2.IsBusy())
            {
                return;
            }
            if (!moveManager.CanMakeMove())
            {
                Debug.Log("No moves left.");
                return;
            }
            if (piece1 is ICombinable && piece2 is ICombinable)
            {
                Debug.Log("Combination requested");
                EventManager.OnCombinationRequested?.Invoke(piece1, piece2);
                moveManager.MakeMove();
                return;
            }

            if (piece1.TryGetComponent<SwapHandler>(out var swapHandler1) &&
                piece2.TryGetComponent<SwapHandler>(out var swapHandler2))
            {
                if (swapManager.TryHandleSwap(swapHandler1, piece1, swapHandler2, piece2))
                {
                    moveManager.MakeMove();
                }
                return;
            }
        }

        public abstract void Enable();
        public abstract void Disable();
    }
}
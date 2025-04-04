using Interfaces;
using Pieces;
using UnityEngine;

namespace Managers
{
    public class SwapSequenceManager : MonoBehaviour
    {
        [SerializeField] private MatchManager matchManager;
        [SerializeField] private ActivationManager activationManager;
        [SerializeField] private SwapManager swapManager;

        private void OnEnable()
        {
            SwapManager.OnSwapCompleted += HandleSwapSequentially;
        }

        private void OnDisable()
        {
            SwapManager.OnSwapCompleted -= HandleSwapSequentially;
        }

        private void HandleSwapSequentially(Piece first, Piece second)
        {
            bool anyMatchFound = matchManager.TryHandleMatches(first, second);
            bool isAnyActivated = first is IActivatable || second is IActivatable;
            
            if (!anyMatchFound && !isAnyActivated)
            {
                swapManager.RevertSwap();
                return;
            }
            
            ((ISwappable)first).OnSwap(second);
            ((ISwappable)second).OnSwap(first);
        }
    }
}
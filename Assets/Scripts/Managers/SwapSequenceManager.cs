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
            // MatchManager processes matches first
            bool anyMatchFound = matchManager.TryHandleMatches(first, second);

            // ActivationManager processes activations next
            bool isAnyActivated = activationManager.TryActivatePiece(first);
            isAnyActivated |= activationManager.TryActivatePiece(second);

            // If nothing happened, tell SwapManager to revert
            if (!anyMatchFound && !isAnyActivated)
            {
                swapManager.RevertSwap();
            }
        }
    }
}
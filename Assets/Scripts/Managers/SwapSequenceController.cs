using System;
using Pieces;
using UnityEngine;

namespace Managers
{
    public class SwapSequenceManager : MonoBehaviour
    {
        public static event Action<Piece, Piece> OnSwapHandled;

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
            // Step 1: MatchManager processes matches first
            bool anyMatchFound = matchManager.TryHandleMatches(first, second);

            // Step 2: ActivationManager processes activations next
            bool isAnyActivated = activationManager.TryActivatePiece(first) || activationManager.TryActivatePiece(second);

            // Step 3: Fire a final event for anything else (e.g., animations, score updates)
            OnSwapHandled?.Invoke(first, second);

            // Step 4: If nothing happened, tell SwapManager to revert
            if (!anyMatchFound && !isAnyActivated)
            {
                swapManager.RevertSwap();
            }
        }
    }
}
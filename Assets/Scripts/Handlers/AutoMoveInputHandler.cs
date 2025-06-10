using System.Collections;
using Managers;
using UnityEngine;

namespace Handlers
{
    public class AutoMoveInputHandler : InputHandler
    {
        [SerializeField] private MatchManager matchManager;
        private float _autoMoveInterval = 1.0f;

        // public void Enable()
        // {
        //     enabled = true;
        //     StartCoroutine(AutoMoveRoutine());
        // }
        //
        // public void Disable()
        // {
        //     enabled = false;
        // }

        private IEnumerator AutoMoveRoutine()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(_autoMoveInterval);
                // AutoSwap();
            }
        }

        // private void AutoSwap()
        // {
        //     if (matchManager.TryGetMatchFormingSwap(out var swapPieces))
        //     {
        //         ProcessInput(swapPieces.Value.Item1, swapPieces.Value.Item2);
        //     }
        //
        //     else
        //     {
        //         Debug.LogWarning("Can't swap");
        //     }
        // }
    }
}
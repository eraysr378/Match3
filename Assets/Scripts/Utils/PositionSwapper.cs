using System;
using Pieces.Behaviors;
using UnityEngine;

namespace Utils
{
    public class PositionSwapper
    {
        private readonly float _swapDuration;
        private readonly Action<GameObject, GameObject> _onPositionsSwapped;
        private int _completedCount;

        public PositionSwapper(float swapDuration, Action<GameObject, GameObject> onPositionsSwapped)
        {
            _swapDuration = swapDuration;
            _onPositionsSwapped = onPositionsSwapped;
        }

        public void SwapPositions(GameObject first, GameObject second)
        {
            var movableFirst = first.GetComponent<Movable>();
            var movableSecond = second.GetComponent<Movable>();

            if (movableFirst == null || movableSecond == null)
                throw new Exception("Movable component missing on one of the objects!");

            _completedCount = 0;

            movableFirst.StartMoving(second.transform.position, _swapDuration,
                onComplete: () => OnMoveFinished(first, second));
            movableSecond.StartMoving(first.transform.position, _swapDuration,
                onComplete: () => OnMoveFinished(first, second));
        }

        private void OnMoveFinished(GameObject first, GameObject second)
        {
            _completedCount++;
            if (_completedCount < 2) return;

            _onPositionsSwapped?.Invoke(first, second);
        }
    }
}
using System;
using Pieces.Behaviors;
using UnityEngine;

namespace Utils
{
    public class PositionSwapper
    {
        private readonly float _swapDuration;
        private readonly Action<GameObject, GameObject> _onPositionsSwapped;

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

            int completedCount = 0;

            void OnMoveFinished()
            {
                completedCount++;
                if (completedCount < 2) return;

                movableFirst.OnTargetReached -= OnMoveFinished;
                movableSecond.OnTargetReached -= OnMoveFinished;

                _onPositionsSwapped?.Invoke(first, second);
            }

            movableFirst.OnTargetReached += OnMoveFinished;
            movableSecond.OnTargetReached += OnMoveFinished;

            movableFirst.StartMoving(second.transform.position, _swapDuration);
            movableSecond.StartMoving(first.transform.position, _swapDuration);
        }
    }
}
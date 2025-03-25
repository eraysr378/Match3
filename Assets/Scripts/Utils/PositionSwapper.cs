using System;
using Pieces;

namespace Utils
{
    public class PieceSwapper
    {
        private readonly float _swapDuration;
        private readonly Action<Piece, Piece, bool> _onMoveCompleted;

        public PieceSwapper(float swapDuration, Action<Piece, Piece, bool> onMoveCompleted)
        {
            _swapDuration = swapDuration;
            _onMoveCompleted = onMoveCompleted;
        }

        public void MovePieces(Piece firstPiece, Piece secondPiece, bool isReverting = false)
        {
            var movableFirst = firstPiece.GetComponent<Movable>();
            var movableSecond = secondPiece.GetComponent<Movable>();

            if (movableFirst == null || movableSecond == null)
                throw new Exception("Movable component missing on one of the pieces!");

            int completedCount = 0;

            void OnMoveFinished()
            {
                completedCount++;
                if (completedCount < 2) return;

                movableFirst.OnTargetReached -= OnMoveFinished;
                movableSecond.OnTargetReached -= OnMoveFinished;

                _onMoveCompleted?.Invoke(firstPiece, secondPiece, isReverting);
            }

            movableFirst.OnTargetReached += OnMoveFinished;
            movableSecond.OnTargetReached += OnMoveFinished;

            movableFirst.StartMoving(secondPiece.CurrentCell.transform.position, _swapDuration);
            movableSecond.StartMoving(firstPiece.CurrentCell.transform.position, _swapDuration);
        }
    }
}
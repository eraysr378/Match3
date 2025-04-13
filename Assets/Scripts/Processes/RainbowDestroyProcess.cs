using System;
using Cells;
using Interfaces;
using UnityEngine;

namespace Pieces.SpecialPieces
{
    public class RainbowDestroyProcess
    {
        private readonly Piece _targetPiece;
        private readonly Action _onCompleted;
        private readonly IRainbowVisualEffect _visualEffect;

        private Cell _targetPieceCell;

        public RainbowDestroyProcess(Piece targetPiece, IRainbowVisualEffect visualEffect , Action onCompleted)
        {
            _targetPiece = targetPiece;
            _visualEffect = visualEffect;
            _onCompleted = onCompleted;
        }

        public void Execute()
        {
            if (_targetPiece != null &&
                _targetPiece.TryGetComponent<IRainbowHittable>(out var hittable))
            {
                _targetPieceCell = _targetPiece.CurrentCell;
                if (!hittable.TryHandleRainbowHit(OnRainbowHitHandled))
                    return;

                _targetPieceCell.MarkDirty();
                _visualEffect.Play();
            }

            _onCompleted?.Invoke();
        }

        private void OnRainbowHitHandled()
        {
            _targetPieceCell.ClearDirty();
            _visualEffect.Finish();
        }

   
    }
}

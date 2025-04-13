using Interfaces;
using Pieces;
using Utils;
using VisualEffects;

namespace Processes
{
    public class RainbowDestroyProcess
    {
        private readonly Piece _targetPiece;
        private readonly BaseVisualEffect _visualEffect;

        private readonly CellDirtyTracker _cellDirtyTracker;

        public RainbowDestroyProcess(Piece targetPiece, BaseVisualEffect visualEffect)
        {
            _targetPiece = targetPiece;
            _visualEffect = visualEffect;
            _cellDirtyTracker = new();
        }

        public void Execute()
        {
            if (_targetPiece != null &&
                _targetPiece.TryGetComponent<IRainbowHittable>(out var hittable))
            {
                var targetPieceCell = _targetPiece.CurrentCell;
                if (!hittable.TryHandleRainbowHit(OnRainbowHitHandled))
                    return;

                _cellDirtyTracker.Mark(targetPieceCell);
                _visualEffect.Play();
            }
        }

        private void OnRainbowHitHandled()
        {
            _cellDirtyTracker.ClearAll();
            _visualEffect.Finish();
        }
    }
}
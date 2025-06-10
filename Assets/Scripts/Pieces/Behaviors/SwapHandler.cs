using System;
using Cells;
using Misc;
using UnityEngine;

namespace Pieces.Behaviors
{
    [RequireComponent(typeof(Movable))]
    public class SwapHandler : MonoBehaviour
    {
        public event Action OnSwapStarted;
        public event Action<Piece> OnSwapCompleted;
        [SerializeField]private SwapReactionPriority swapReactionPriority;
        private Movable _movable;
        private Vector3 _startPosition;
        private float _swapDuration;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
        }
        
        public void OnValidSwap(BaseCell baseCell,float swapDuration,Action onComplete)
        {
            _swapDuration = swapDuration;
            OnSwapStarted?.Invoke();
            _movable.StartMovingWithDuration(baseCell.transform.position,_swapDuration,onComplete);
        }

        public void OnInvalidSwap(BaseCell baseCell,float swapDuration, Action onComplete)
        {
            _swapDuration = swapDuration;
            OnSwapStarted?.Invoke();
            _startPosition = transform.position;
            _movable.StartMovingWithDuration(baseCell.transform.position, _swapDuration/2, 
                () => GoBackToStart(onComplete));
        }

        private void GoBackToStart(Action onComplete)
        {
            _movable.StartMovingWithDuration(_startPosition, _swapDuration/2, onComplete);
        }
        

        public void OnValidSwapCompleted(Piece otherPiece)
        {
            OnSwapCompleted?.Invoke(otherPiece);

        }

        public void OnInvalidSwapCompleted()
        {
            OnSwapCompleted?.Invoke(null);

        }
        
        public SwapReactionPriority GetReactionPriority()
        {
            return swapReactionPriority;
        }
        
    }
}
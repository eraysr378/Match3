using System;
using Cells;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class FallHandler : MonoBehaviour
    {
        public static event Action<FallHandler> OnAnyFallCompleted;
        public static event Action<FallHandler> OnAnyFallStarted;
        public event Action OnFallStarted;
        public event Action OnFallCompleted;
        
        private readonly float _fallSpeed = 2f; //14
        private Movable _movable;
        private bool _isFalling;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
        }

        private void OnEnable()
        {
            _isFalling = false;
        }

        private void OnDisable()
        {
            if (!_isFalling) return;
            OnAnyFallCompleted?.Invoke(this);
        }

        public void FallTo(BaseCell baseCell)
        {
            _isFalling = true;
            OnAnyFallStarted?.Invoke(this);
            OnFallStarted?.Invoke();
            _movable.StartMovingWithSpeed(
                baseCell.transform.position,
                _fallSpeed,
                OnComplete
            );
        }

        private void OnComplete()
        {
            _isFalling = false;
            OnAnyFallCompleted?.Invoke(this);
            OnFallCompleted?.Invoke();
        }
    }
}
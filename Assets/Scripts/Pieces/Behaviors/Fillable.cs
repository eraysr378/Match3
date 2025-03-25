using System;
using Cells;
using UnityEngine;

namespace Pieces.Behaviors
{
    [RequireComponent(typeof(Movable))]
    public class Fillable : MonoBehaviour
    {
        public event Action<Fillable> OnFilled;
        private Movable _movable;
        private Piece _piece;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
            _piece = GetComponent<Piece>();

        }

        public void Fill(Cell targetCell,float duration)
        {
            if (_movable == null)
            {
                Debug.LogWarning("Trying to move a Fillable cell without Movable!");
                return;
            }
            _piece.SetCell(targetCell);
            _movable.StartMoving(targetCell.transform.position,duration);
            _movable.OnTargetReached += OnTargetReached;
        
        }


        private void OnTargetReached()
        {
            _movable.OnTargetReached -= OnTargetReached;
            OnFilled?.Invoke(this);
        }
    }
}
using System;
using Cells;
using Managers;
using UnityEngine;

namespace Pieces.Behaviors
{
    [RequireComponent(typeof(Movable))]
    public class Fillable : MonoBehaviour
    {
        public static event Action<Fillable> OnFillCompleted;
        public static event Action<Fillable> OnFillStarted;
        private readonly float _baseSpeed = 5;
        private float _currentSpeed;
        private readonly float _speedMultiplier = 1.2f;

        private Movable _movable;
        private Piece _piece;
        private bool _isFilling;
        private bool _isFalling;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
            _piece = GetComponent<Piece>();
        }

        private void OnEnable()
        {
            _currentSpeed = _baseSpeed;
        }

        private void OnDisable()
        {
            if (!_isFilling) return;
            
            // Disabled when filling
            _currentSpeed = _baseSpeed;
            _isFilling = false;
            OnFillCompleted?.Invoke(this);
        }

        public void Fall()
        {
            _isFalling = true;
            _movable.StartMovingWithSpeed(
                _piece.CurrentCell.transform.position,
                _baseSpeed,
                OnFallCompleted
            );
        }

        private void OnFallCompleted()
        {
            _isFalling = false;
            StartFill();
        }
        public void StartFill()
        {
            if(_isFilling || _isFalling) return;
            _isFilling = true;
            OnFillStarted?.Invoke(this);
            TryFill();
        }
        private bool TryFill()
        {
            if (_piece.CurrentCell == null || _piece.Row == 0 || _piece.CurrentCell.IsDirty())
            {
                _currentSpeed = _baseSpeed;
                _isFilling = false;
                OnFillCompleted?.Invoke(this);
                return false;
            }

            int row = _piece.CurrentCell.Row;
            int col = _piece.CurrentCell.Col;

            // Try vertical first
            Cell below = GridManager.Instance.GetCellAt(row - 1, col);
            
            if (TryMoveTo(below) || TryMoveDiagonally(row, col)) return true;
            
            _currentSpeed = _baseSpeed;
            _isFilling = false;
            OnFillCompleted?.Invoke(this);
            return false;
        }

        private bool TryMoveTo(Cell targetCell)
        {
            if (targetCell == null || targetCell.CurrentPiece != null || targetCell.IsDirty()) return false;
            Fill(targetCell);
            return true;
        }
        private bool TryMoveDiagonally(int row, int col)
        {

            for (int offset = -1; offset <= 1; offset += 2)
            {
                int diagCol = col + offset;
                if (diagCol < 0 || diagCol >= GridManager.Instance.Width) continue;

                Cell targetCell = GridManager.Instance.GetCellAt(row - 1, diagCol);
                if (targetCell.CurrentPiece != null || targetCell.IsDirty()) continue;

                if (IsPathBlockedAbove(row, diagCol))
                {
                    Fill(targetCell);
                    return true;
                }
            }

            return false;
        }
        private bool IsPathBlockedAbove(int row, int col)
        {

            for (int aboveRow = row; aboveRow < GridManager.Instance.Height; aboveRow++)
            {
                Piece pieceAbove = GridManager.Instance.GetCellAt(aboveRow, col).CurrentPiece;
                if (pieceAbove != null && !pieceAbove.TryGetComponent<Fillable>(out _))
                    return true;
            }

            return false;
        }

        private void Fill(Cell targetCell)
        {
            Cell prev = _piece.CurrentCell;
            _piece.SetCell(targetCell);
            
            prev.FillIfClean();
            
            _movable.StartMovingWithSpeed(
                _piece.CurrentCell.transform.position,
                _currentSpeed,
                OnTargetCellReached
            );
        }

        private void OnTargetCellReached()
        {
            _currentSpeed *= _speedMultiplier;

            TryFill();
        }
        
    }
}

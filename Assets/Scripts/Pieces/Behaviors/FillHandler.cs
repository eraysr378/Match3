using System;
using Cells;
using Managers;
using UnityEngine;

namespace Pieces.Behaviors
{
    [RequireComponent(typeof(Movable))]
    public class FillHandler : MonoBehaviour
    {
        public static event Action<FillHandler> OnAnyFillCompleted;
        public static event Action<FillHandler> OnAnyFillStarted;

        public event Action OnFillStarted;
        public event Action OnFillCompleted;
        private readonly float _baseSpeed = 3; // 15
        private float _currentSpeed;
        private readonly float _speedMultiplier = 1.2f;

        private Movable _movable;
        private Piece _piece;

        private bool _isFilling;

        private void Awake()
        {
            _movable = GetComponent<Movable>();
            _piece = GetComponent<Piece>();
        }

        private void OnEnable()
        {
            _isFilling = false;
            _currentSpeed = _baseSpeed;
        }

        private void OnDisable()
        {
            if (!_isFilling) return;
            OnAnyFillCompleted?.Invoke(this);
            // // Disabled when filling
            // EndFill();
        }


        public bool TryStartFill()
        {
            if (_piece.IsBusy() || _isFilling || !TryFill())
            {
                return false;
            }

            _isFilling = true;
            OnFillStarted?.Invoke();
            OnAnyFillStarted?.Invoke(this);
            return true;
        }

        private void EndFill()
        {
            _currentSpeed = _baseSpeed;
            _isFilling = false;
            OnFillCompleted?.Invoke();
            OnAnyFillCompleted?.Invoke(this);
        }

        private bool TryFill()
        {
            if (_piece.CurrentCell == null || _piece.Row == 0 || _piece.CurrentCell.IsDirty())
            {
                return false;
            }

            int row = _piece.CurrentCell.Row;
            int col = _piece.CurrentCell.Col;

            // Try vertical first
            BaseCell below = GridManager.Instance.GetCellAt(row - 1, col);

            if (TryMoveTo(below) || TryMoveDiagonally(row, col)) return true;

            return false;
        }

        private bool TryMoveTo(BaseCell targetBaseCell)
        {
            if (targetBaseCell == null || targetBaseCell.CurrentPiece != null || targetBaseCell.IsDirty()) return false;
            Fill(targetBaseCell);
            return true;
        }

        private bool TryMoveDiagonally(int row, int col)
        {
            for (int offset = -1; offset <= 1; offset += 2)
            {
                int diagCol = col + offset;
                if (diagCol < 0 || diagCol >= GridManager.Instance.Width) continue;

                BaseCell targetBaseCell = GridManager.Instance.GetCellAt(row - 1, diagCol);
                if (targetBaseCell == null || targetBaseCell.CurrentPiece != null || targetBaseCell.IsDirty()) continue;

                if (IsPathBlockedAbove(row, diagCol))
                {
                    Fill(targetBaseCell);
                    return true;
                }
            }

            return false;
        }

        private bool IsPathBlockedAbove(int row, int col)
        {
            for (int aboveRow = row; aboveRow < GridManager.Instance.Height; aboveRow++)
            {
                BaseCell baseCellAbove = GridManager.Instance.GetCellAt(aboveRow, col);
                if (baseCellAbove == null || (baseCellAbove.CurrentPiece != null && !baseCellAbove.CurrentPiece.TryGetComponent<FillHandler>(out _)))
                    return true;
            }

            return false;
        }

        private void Fill(BaseCell targetBaseCell)
        {
            BaseCell prev = _piece.CurrentCell;
            _piece.SetCell(targetBaseCell);

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

            if (!TryFill())
            {
                EndFill();
            }
        }
    }
}
using Cells;
using Interfaces;
using Managers;
using OperationBlockTrackers;
using Pieces;
using UnityEngine;

namespace Handlers
{
    public class UserInputHandler : InputHandler
    {
        private Cell _selectedCell;
        private UserInputBlockTracker _userInputBlockTracker;

        private void Awake()
        {
            _userInputBlockTracker = GetComponent<UserInputBlockTracker>();
        }

        private void OnEnable()
        {
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }

        private void OnAnyPointerEnterCellEvent(Cell cell)
        {
            if (_userInputBlockTracker.HasActiveOperations())
            {
                _selectedCell = null;
                return;
            }

            if (!_selectedCell || _selectedCell == cell)
                return;

            if (!_selectedCell.gameObject.activeSelf ||
                !AreCellsAdjacent(_selectedCell, cell))
            {
                _selectedCell = null;
                return;
            }
            ProcessInput(_selectedCell, cell);
            _selectedCell = null;
        }
    

        private void OnAnyPointerUpCellEvent(Cell cell)
        {
            if (_userInputBlockTracker.HasActiveOperations())
            {
                _selectedCell = null;
                return;
            }

            if (cell == _selectedCell && cell.CurrentPiece is IActivatable activatable)
            {
                if (moveManager.CanMakeMove() && activatable.TryActivate())
                {
                    moveManager.MakeMove();
                }
            }

            _selectedCell = null;
        }

        private void OnAnyPointerDownCellEvent(Cell cell)
        {
            if (_userInputBlockTracker.HasActiveOperations())
            {
                _selectedCell = null;
                return;
            }
            
            _selectedCell = cell;
        }

        private bool AreCellsAdjacent(Cell cell1, Cell cell2)
        {
            return Mathf.Abs(cell1.Row - cell2.Row) + Mathf.Abs(cell1.Col - cell2.Col) == 1;
        }

        public override void Enable()
        {
            Cell.OnAnyPointerDown += OnAnyPointerDownCellEvent;
            Cell.OnAnyPointerUp += OnAnyPointerUpCellEvent;
            Cell.OnAnyPointerEnter += OnAnyPointerEnterCellEvent;
        }

        public override void Disable()
        {
            Cell.OnAnyPointerDown -= OnAnyPointerDownCellEvent;
            Cell.OnAnyPointerUp -= OnAnyPointerUpCellEvent;
            Cell.OnAnyPointerEnter -= OnAnyPointerEnterCellEvent;
        }
    }
}
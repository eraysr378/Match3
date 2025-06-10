using Cells;
using Interfaces;
using OperationBlockTrackers;
using UnityEngine;

namespace Handlers
{
    public class UserInputHandler : InputHandler
    {
        private BaseCell _selectedBaseCell;
        private UserInputBlockTracker _userInputBlockTracker;

        private void Awake()
        {
            _userInputBlockTracker = GetComponent<UserInputBlockTracker>();
        }

        private void OnEnable()
        {
            BaseCell.OnAnyPointerDown += OnAnyPointerDownCellEvent;
            BaseCell.OnAnyPointerUp += OnAnyPointerUpCellEvent;
            BaseCell.OnAnyPointerEnter += OnAnyPointerEnterCellEvent;
        }

        private void OnDisable()
        {
            BaseCell.OnAnyPointerDown -= OnAnyPointerDownCellEvent;
            BaseCell.OnAnyPointerUp -= OnAnyPointerUpCellEvent;
            BaseCell.OnAnyPointerEnter -= OnAnyPointerEnterCellEvent;
        }

        private void OnAnyPointerEnterCellEvent(BaseCell baseCell)
        {
            if (_userInputBlockTracker.HasActiveOperations())
            {
                _selectedBaseCell = null;
                return;
            }

            if (!_selectedBaseCell || _selectedBaseCell == baseCell)
                return;

            if (!_selectedBaseCell.gameObject.activeSelf ||
                !AreCellsAdjacent(_selectedBaseCell, baseCell))
            {
                _selectedBaseCell = null;
                return;
            }

            ProcessInput(_selectedBaseCell, baseCell);
            _selectedBaseCell = null;
        }


        private void OnAnyPointerUpCellEvent(BaseCell baseCell)
        {
            if (_userInputBlockTracker.HasActiveOperations())
            {
                _selectedBaseCell = null;
                return;
            }

            if (baseCell == _selectedBaseCell && baseCell.CurrentPiece is IActivatable activatable)
            {
                if (moveManager.CanMakeMove() && activatable.TryActivate())
                {
                    moveManager.MakeMove();
                }
            }

            _selectedBaseCell = null;
        }

        private void OnAnyPointerDownCellEvent(BaseCell baseCell)
        {
            if (_userInputBlockTracker.HasActiveOperations())
            {
                _selectedBaseCell = null;
                return;
            }

            _selectedBaseCell = baseCell;
        }

        private bool AreCellsAdjacent(BaseCell cell1, BaseCell cell2)
        {
            return Mathf.Abs(cell1.Row - cell2.Row) + Mathf.Abs(cell1.Col - cell2.Col) == 1;
        }

        // public void Enable()
        // {
        //     BaseCell.OnAnyPointerDown += OnAnyPointerDownCellEvent;
        //     BaseCell.OnAnyPointerUp += OnAnyPointerUpCellEvent;
        //     BaseCell.OnAnyPointerEnter += OnAnyPointerEnterCellEvent;
        // }
        //
        // public void Disable()
        // {
        //     BaseCell.OnAnyPointerDown -= OnAnyPointerDownCellEvent;
        //     BaseCell.OnAnyPointerUp -= OnAnyPointerUpCellEvent;
        //     BaseCell.OnAnyPointerEnter -= OnAnyPointerEnterCellEvent;
        // }
    }
}
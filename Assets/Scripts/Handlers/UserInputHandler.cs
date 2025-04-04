using Interfaces;
using Managers;
using OperationTrackers;
using Pieces;
using UnityEngine;

namespace Handlers
{
    public class UserInputHandler : InputHandler
    {
        private Piece _selectedPiece;
        private UserInputTracker _userInputTracker;

        private void Awake()
        {
            _userInputTracker = GetComponent<UserInputTracker>();
        }

        private void OnEnable()
        {
            Enable();
        }

        private void OnDisable()
        {
            Disable();
        }

        private void OnAnyPointerEnterCellEvent(Piece secondPiece)
        {
            if (_userInputTracker.HasActiveOperations())
            {
                _selectedPiece = null;
                return;
            }

            if (!_selectedPiece || _selectedPiece == secondPiece)
                return;

            if (!_selectedPiece.gameObject.activeSelf || secondPiece is not ISwappable ||
                !ArePiecesAdjacent(_selectedPiece, secondPiece))
            {
                _selectedPiece = null;
                return;
            }
            ProcessInput(_selectedPiece, secondPiece);
            _selectedPiece = null;
        }
    

        private void OnAnyPointerUpCellEvent(Piece piece)
        {
            if (_userInputTracker.HasActiveOperations())
            {
                _selectedPiece = null;
                return;
            }

            if (piece == _selectedPiece)
            {
                if (piece is IActivatable activatable)
                {
                    activatable.Activate();
                }
            }

            _selectedPiece = null;
        }

        private void OnAnyPointerDownCellEvent(Piece piece)
        {
            if (_userInputTracker.HasActiveOperations())
            {
                _selectedPiece = null;
                return;
            }


            if (piece is ISwappable)
            {
                _selectedPiece = piece;
            }
        }

        private bool ArePiecesAdjacent(Piece piece1, Piece piece2)
        {
            return Mathf.Abs(piece1.Row - piece2.Row) + Mathf.Abs(piece1.Col - piece2.Col) == 1;
        }

        public override void Enable()
        {
            InteractablePiece.OnAnyPointerDown += OnAnyPointerDownCellEvent;
            InteractablePiece.OnAnyPointerUp += OnAnyPointerUpCellEvent;
            InteractablePiece.OnAnyPointerEnter += OnAnyPointerEnterCellEvent;
        }

        public override void Disable()
        {
            InteractablePiece.OnAnyPointerDown -= OnAnyPointerDownCellEvent;
            InteractablePiece.OnAnyPointerUp -= OnAnyPointerUpCellEvent;
            InteractablePiece.OnAnyPointerEnter -= OnAnyPointerEnterCellEvent;
        }
    }
}
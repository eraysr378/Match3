using System;
using Cells;
using Interfaces;
using OperationBlockTrackers;
using Pieces;
using SwapSystem;
using UnityEngine;
using Utils;

namespace Managers
{
    public class SwapManager : MonoBehaviour
    {
        public static event Action<Piece, Piece> OnValidSwapCompleted;
        public static event Action<Piece, Piece> OnInvalidSwapCompleted;

        [SerializeField] private float swapDuration;

        private Piece _swappedFirstPiece;
        private Piece _swappedSecondPiece;
        private bool _canSwap = true;
        private bool _isValidSwap;

        private SwapBlockTracker _swapBlockTracker;
        private PositionSwapper _positionSwapper;
        private ISwapCommand _swapCommand;
        private SwapValidator _swapValidator;

        private void Awake()
        {
            _swapBlockTracker = GetComponent<SwapBlockTracker>();
            _positionSwapper = new PositionSwapper(swapDuration, OnPositionsSwapped);
            _swapValidator = new SwapValidator();
        }

        private void OnEnable()
        {
            EventManager.OnSwapRequested += HandleSwap;
        }

        private void OnDisable()
        {
            EventManager.OnSwapRequested -= HandleSwap;
        }

        private void HandleSwap(Piece first, Piece second)
        {
            if (_swapBlockTracker.HasActiveOperations() || !_canSwap)
                return;

            _canSwap = false;

            _isValidSwap = _swapValidator.IsValidSwap(first, second);
            _swappedFirstPiece = first;
            _swappedSecondPiece = second;

            _swapCommand = new SwapCommand(first, second, _positionSwapper);

            _swapCommand.Execute();
        }

        private void OnPositionsSwapped(GameObject first, GameObject second)
        {
            if (_swapCommand == null)
            {
                _canSwap = true;
                OnInvalidSwapCompleted?.Invoke(_swappedFirstPiece, _swappedSecondPiece);
                return;
            }

            if (!_isValidSwap)
            {
                _swapCommand.Undo();
                _swapCommand = null;
                return;
            }
            
            SwapPieceCells(_swappedFirstPiece, _swappedSecondPiece);
            _swapCommand = null;
            _canSwap = true;
            
            if (_swappedFirstPiece.TryGetComponent<ISwappable>(out var swappableFirst))
                swappableFirst.OnSwap(_swappedSecondPiece);
            if (_swappedSecondPiece.TryGetComponent<ISwappable>(out var swappableSecond))
                swappableSecond.OnSwap(_swappedFirstPiece);
            
            
            swappableFirst?.OnPostSwap(_swappedSecondPiece);
            swappableSecond?.OnPostSwap(_swappedFirstPiece);
            
            
            OnValidSwapCompleted?.Invoke(_swappedFirstPiece, _swappedSecondPiece);
        }
        

        private void SwapPieceCells(Piece firstPiece, Piece secondPiece)
        {
            Cell firstCell = firstPiece.CurrentCell;
            Cell secondCell = secondPiece.CurrentCell;

            secondPiece.SetCell(null);
            firstPiece.SetCell(secondCell);
            secondPiece.SetCell(firstCell);
        }
    }
}
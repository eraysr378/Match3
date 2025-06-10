using Cells;
using OperationBlockTrackers;
using Pieces;
using Pieces.Behaviors;
using SwapSystem;
using UnityEngine;

namespace Managers
{
    public class SwapManager : MonoBehaviour
    {

        [SerializeField] private float swapDuration;
    
        private Piece _swappedFirstPiece;
        private Piece _swappedSecondPiece;
        private bool _canSwap = true;
        private bool _isValidSwap;
        private int _count;
        private SwapBlockTracker _swapBlockTracker;
        private SwapValidator _swapValidator;
        private SwapHandler _firstSwapHandler;
        private SwapHandler _secondSwapHandler;

        private void Awake()
        {
            _swapBlockTracker = GetComponent<SwapBlockTracker>();
            _swapValidator = new SwapValidator();
        }
        

        public bool TryHandleSwap(SwapHandler firstSwappable,Piece firstPiece, SwapHandler secondSwappable, Piece secondPiece)
        {
            if (_swapBlockTracker.HasActiveOperations() || !_canSwap)
            {
                Debug.LogWarning("SwapManager is blocked");
                return false;
            }
            _canSwap = false;
            _isValidSwap = _swapValidator.IsValidSwap(firstPiece, secondPiece);
            _firstSwapHandler = firstSwappable;
            _secondSwapHandler = secondSwappable;
            _swappedFirstPiece = firstPiece;
            _swappedSecondPiece = secondPiece;
            _swappedFirstPiece.OnDestroy += SwappedFirstPiece_OnDestroy;
            _swappedSecondPiece.OnDestroy += SwappedSecondPiece_OnDestroy;
            if (_isValidSwap)
            {
                SwapPieceCells(firstPiece, secondPiece);
                firstSwappable.OnValidSwap(firstPiece.CurrentCell,swapDuration, HandleValidPostSwap);
                secondSwappable.OnValidSwap(secondPiece.CurrentCell,swapDuration, HandleValidPostSwap);
                return true;
            }
            firstSwappable.OnInvalidSwap(secondPiece.CurrentCell,swapDuration, HandleInvalidPostSwap);
            secondSwappable.OnInvalidSwap(firstPiece.CurrentCell,swapDuration, HandleInvalidPostSwap);
            return false;
            
  
        }

        private void SwappedSecondPiece_OnDestroy()
        {
            _swappedSecondPiece.OnDestroy -= SwappedSecondPiece_OnDestroy;
            _swappedSecondPiece = null;
            _count++;
            _secondSwapHandler = null;
            if(_count == 2) ResetSwapState();
        }

        private void SwappedFirstPiece_OnDestroy()
        {
            _swappedFirstPiece.OnDestroy -= SwappedFirstPiece_OnDestroy;
            _swappedFirstPiece = null;
            _count++;
            _firstSwapHandler = null;
            if(_count == 2) ResetSwapState();
        }

        private void HandleInvalidPostSwap()
        {
            _count++;
            if(_count < 2) return;
            _firstSwapHandler?.OnInvalidSwapCompleted();
            _secondSwapHandler?.OnInvalidSwapCompleted();
            ResetSwapState();
        }
        private void HandleValidPostSwap()
        {
            _count++;
            if(_count < 2) return;
            if (_firstSwapHandler.GetReactionPriority() > _secondSwapHandler.GetReactionPriority())
            {
                _firstSwapHandler?.OnValidSwapCompleted(_swappedSecondPiece);
                _secondSwapHandler?.OnValidSwapCompleted(_swappedFirstPiece);
            }
            else
            {
                _secondSwapHandler?.OnValidSwapCompleted(_swappedFirstPiece);
                _firstSwapHandler?.OnValidSwapCompleted(_swappedSecondPiece);
            }
            ResetSwapState();
            
        }
        private void ResetSwapState()
        {
            if (_swappedFirstPiece != null)
                _swappedFirstPiece.OnDestroy -= SwappedFirstPiece_OnDestroy;

            if (_swappedSecondPiece != null)
                _swappedSecondPiece.OnDestroy -= SwappedSecondPiece_OnDestroy;

            _count = 0;
            _swappedFirstPiece = null;
            _swappedSecondPiece = null;
            _firstSwapHandler = null;
            _secondSwapHandler = null;
            _canSwap = true;

        }

        private void SwapPieceCells(Piece firstPiece, Piece secondPiece)
        {
            BaseCell firstBaseCell = firstPiece.CurrentCell;
            BaseCell secondBaseCell = secondPiece.CurrentCell;

            secondPiece.SetCell(null);
            firstPiece.SetCell(secondBaseCell);
            secondPiece.SetCell(firstBaseCell);
        }
    }
}
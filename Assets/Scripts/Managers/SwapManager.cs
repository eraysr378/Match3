using System;
using Cells;
using OperationTrackers;
using Pieces;
using UnityEngine;
using Utils;

namespace Managers
{
    public class SwapManager : MonoBehaviour
    {
        public static event Action<Piece, Piece> OnSwapCompleted;

        [SerializeField] private float swapDuration;
        private Piece _swappedFirstPiece;
        private Piece _swappedSecondPiece;
        private bool _canSwap = true;
        private bool _isRevertingSwap = false;
        private SwapTracker _swapTracker;
        private PositionSwapper _positionSwapper;


        private void Awake()
        {
            _swapTracker = GetComponent<SwapTracker>();
            _positionSwapper = new PositionSwapper(swapDuration, OnPositionsSwapped);
        }

        private void OnEnable()
        {
            EventManager.OnSwapRequested += TrySwap;
            EventManager.OnInstantSwapRequested += SwapInstantly;
        }

        private void OnDisable()
        {
            EventManager.OnSwapRequested -= TrySwap;
            EventManager.OnInstantSwapRequested -= SwapInstantly;
        }

        private void SwapInstantly(Piece firstPiece, Piece secondPiece)
        {
            if (_swapTracker.HasActiveOperations() || !_canSwap)
                return;
            SwapPieceCells(firstPiece, secondPiece);
        }

        private void TrySwap(Piece firstPiece, Piece secondPiece)
        {
            if (_swapTracker.HasActiveOperations() || !_canSwap)
                return;
            Swap(firstPiece, secondPiece);
        }

        public void RevertSwap()
        {
            _isRevertingSwap = true;
            StartSwap(_swappedFirstPiece, _swappedSecondPiece);
        }

        private void Swap(Piece firstPiece, Piece secondPiece)
        {
            _isRevertingSwap = false;
            _swappedFirstPiece = firstPiece;
            _swappedSecondPiece = secondPiece;
            StartSwap(firstPiece, secondPiece);
        }


        private void StartSwap(Piece firstPiece, Piece secondPiece)
        {
            _canSwap = false;

            _positionSwapper.SwapPositions(firstPiece.gameObject, secondPiece.gameObject);
        }

        private void OnPositionsSwapped(GameObject first, GameObject second)
        {
            SwapPieceCells(_swappedFirstPiece, _swappedSecondPiece);
            if (!_isRevertingSwap)
            {
                OnSwapCompleted?.Invoke(_swappedFirstPiece, _swappedSecondPiece);
            }
            else
            {
                _swappedFirstPiece = null;
                _swappedSecondPiece = null;
            }

            _canSwap = true;
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
using Pieces;
using Interfaces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        private SwapManager _swapManager;
        private MatchManager _matchManager;
        private ActivationManager _activationManager;
        private FillManager _fillManager;

        private Piece _firstSelectedPiece;
        private Piece _secondSelectedPiece;
        private Grid _grid;

        private void Awake()
        {
            _swapManager = GetComponent<SwapManager>();
            _matchManager = GetComponent<MatchManager>();
            _activationManager = GetComponent<ActivationManager>();
            _fillManager = GetComponent<FillManager>();
        }

        private void OnEnable()
        {
            EventManager.OnPointerDownPiece += OnPointerDownCellEvent;
            EventManager.OnPointerUpPiece += OnPointerUpCellEvent;
            EventManager.OnPointerEnterPiece += OnPointerEnterCellEvent;
            EventManager.OnPointerClickedPiece += OnPointerClickedCellEvent;
            EventManager.OnGridInitialized += OnGridInitialized;
            EventManager.OnSwapCompleted += OnSwapCompleted;

        }

        private void OnDisable()
        {
            EventManager.OnPointerDownPiece -= OnPointerDownCellEvent;
            EventManager.OnPointerUpPiece -= OnPointerUpCellEvent;
            EventManager.OnPointerEnterPiece -= OnPointerEnterCellEvent;
            EventManager.OnPointerClickedPiece -= OnPointerClickedCellEvent;
            EventManager.OnGridInitialized -= OnGridInitialized;
            EventManager.OnSwapCompleted -= OnSwapCompleted;

        }     
        private void OnGridInitialized(Grid grid)
        {
            _grid = grid;
            _matchManager.Initialize(_grid);
            _fillManager.Initialize(_grid);
        }

        
        
        private void OnSwapCompleted(Piece swappedFirstPiece, Piece swappedSecondPiece)
        {
            bool anyMatchFound = false;
            bool isAnyActivated = false;
            // If both pieces are Activatable, there may be combinated piece created

            anyMatchFound |= _matchManager.GetMatch(swappedFirstPiece) is not null;
            anyMatchFound |= _matchManager.GetMatch(swappedSecondPiece) is not null;


            isAnyActivated |= _activationManager.TryActivatePiece(swappedFirstPiece);
            isAnyActivated |= _activationManager.TryActivatePiece(swappedSecondPiece);

            if (!anyMatchFound && !isAnyActivated)
            {
                _swapManager.RevertSwap();
            }
            else
            {
                _matchManager.ClearAllValidMatches();
                _fillManager.StartFilling();
            }


            ResetSelectedPieces();
        }

        private void OnPointerEnterCellEvent(Piece piece)
        {
            if (!_swapManager.CanSwap)
                return;

            if (!_firstSelectedPiece || _firstSelectedPiece == piece)
                return;

            if (piece is not ISwappable || !ArePiecesAdjacent(_firstSelectedPiece, piece))
                return;

            _secondSelectedPiece = piece;
            _swapManager.Swap(_firstSelectedPiece, _secondSelectedPiece);
        }

        private void OnPointerClickedCellEvent(Piece piece)
        {
            _activationManager.TryActivatePiece(piece);
        }
        private void OnPointerDownCellEvent(Piece piece)
        {
            if(_fillManager.IsFilling)
                return;
            if (piece is ISwappable)
            {
                _firstSelectedPiece = piece;
            }
        }
        
        private void OnPointerUpCellEvent(Piece piece)
        {
            ResetSelectedPieces();
        }

        private bool ArePiecesAdjacent(Piece piece1, Piece piece2)
        {
            return Mathf.Abs(piece1.Row - piece2.Row) + Mathf.Abs(piece1.Col - piece2.Col) == 1;
        }
        private void ResetSelectedPieces()
        {
            _firstSelectedPiece = null;
            _secondSelectedPiece = null;
        }
    }
}
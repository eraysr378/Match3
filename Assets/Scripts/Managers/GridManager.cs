using Pieces;
using Interfaces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class GridManager : MonoBehaviour
    {
        private SwapManager SwapManager { get; set; }
        private MatchManager MatchManager { get; set; }
        private ActivationManager ActivationManager { get; set; }
        private FillManager FillManager { get; set; }
        // private FallManager FallManager { get; set; }

        private Piece _firstSelectedPiece;
        private Piece _secondSelectedPiece;
        private Grid _grid;

        private void Awake()
        {
            SwapManager = GetComponent<SwapManager>();
            MatchManager = GetComponent<MatchManager>();
            ActivationManager = GetComponent<ActivationManager>();
            FillManager = GetComponent<FillManager>();
            // FallManager  = GetComponent<FallManager>();
        }

        private void OnEnable()
        {
            EventManager.OnPointerDownCell += OnPointerDownCellEvent;
            EventManager.OnPointerUpCell += OnPointerUpCellEvent;
            EventManager.OnPointerEnterCell += OnPointerEnterCellEvent;
            EventManager.OnPointerClickedCell += OnPointerClickedCellEvent;
            EventManager.OnGridInitialized += OnGridInitialized;
            EventManager.OnSwapCompleted += OnSwapCompleted;

        }

        private void OnDisable()
        {
            EventManager.OnPointerDownCell -= OnPointerDownCellEvent;
            EventManager.OnPointerUpCell -= OnPointerUpCellEvent;
            EventManager.OnPointerEnterCell -= OnPointerEnterCellEvent;
            EventManager.OnPointerClickedCell -= OnPointerClickedCellEvent;
            EventManager.OnGridInitialized -= OnGridInitialized;
            EventManager.OnSwapCompleted -= OnSwapCompleted;

        }     
        private void OnGridInitialized(Grid grid)
        {
            _grid = grid;
            SwapManager.Initialize(_grid);
            MatchManager.Initialize(_grid);
            FillManager.Initialize(_grid);
            // FallManager.Initialize(_grid);
        }

        
        
        private void OnSwapCompleted(Piece swappedFirstPiece, Piece swappedSecondPiece)
        {
            bool anyMatchFound = false;
            bool isAnyActivated = false;

            anyMatchFound |= MatchManager.GetMatch(swappedFirstPiece) is not null;
            anyMatchFound |= MatchManager.GetMatch(swappedSecondPiece) is not null;


            isAnyActivated |= ActivationManager.ActivateCell(swappedFirstPiece);
            isAnyActivated |= ActivationManager.ActivateCell(swappedSecondPiece);

            if (!anyMatchFound && !isAnyActivated)
            {
                SwapManager.RevertSwap();
            }
            else
            {
                MatchManager.ClearAllValidMatches();
                FillManager.StartFilling();
            }


            _firstSelectedPiece = null;
            _secondSelectedPiece = null;
        }

        private void OnPointerEnterCellEvent(Piece piece)
        {
            if (!SwapManager.CanSwap())
            {
                return;
            }

            if (!_firstSelectedPiece || _firstSelectedPiece == piece)
            {
                return;
            }

            if (piece is not ISwappable || !ArePiecesAdjacent(_firstSelectedPiece, piece))
            {
                return;
            }

            _secondSelectedPiece = piece;
            SwapManager.Swap(_firstSelectedPiece, _secondSelectedPiece);
        }

        private void OnPointerClickedCellEvent(Piece piece)
        {
            ActivationManager.ActivateCell(piece);
        }
        private void OnPointerDownCellEvent(Piece piece)
        {
            if (piece is ISwappable)
            {
                _firstSelectedPiece = piece;
            }
        }
        
        private void OnPointerUpCellEvent(Piece piece)
        {
            _firstSelectedPiece = null;
            _secondSelectedPiece = null;
        }

        private bool ArePiecesAdjacent(Piece piece1, Piece piece2)
        {
            int dx = Mathf.Abs(piece1.Row - piece2.Row);
            int dy = Mathf.Abs(piece1.Col - piece2.Col);
            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1); // Ensure adjacent swap only
        }
    }
}
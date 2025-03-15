using Cells;
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
        private FallManager FallManager { get; set; }

        private Cell _firstSelectedCell;
        private Cell _secondSelectedCell;
        private Grid _grid;

        private void Awake()
        {
            SwapManager = GetComponent<SwapManager>();
            MatchManager = GetComponent<MatchManager>();
            ActivationManager = GetComponent<ActivationManager>();
            FillManager = GetComponent<FillManager>();
            FallManager  = GetComponent<FallManager>();
        }

        private void OnEnable()
        {
            EventManager.OnPointerDownCell += OnPointerDownCellEvent;
            EventManager.OnPointerUpCell += OnPointerUpCellEvent;
            EventManager.OnPointerEnterCell += OnPointerEnterCellEvent;
            EventManager.OnPointerClickedCell += OnPointerClickedCellEvent;
            EventManager.OnGridInitialized += OnGridInitialized;
            EventManager.OnCellDestroyed += OnCellDestroyed;
            EventManager.OnFilledCell += OnFilledCell;
            EventManager.OnSwapCompleted += OnSwapCompleted;

        }

        private void OnDisable()
        {
            EventManager.OnPointerDownCell -= OnPointerDownCellEvent;
            EventManager.OnPointerUpCell -= OnPointerUpCellEvent;
            EventManager.OnPointerEnterCell -= OnPointerEnterCellEvent;
            EventManager.OnPointerClickedCell -= OnPointerClickedCellEvent;
            EventManager.OnGridInitialized -= OnGridInitialized;
            EventManager.OnCellDestroyed -= OnCellDestroyed;
            EventManager.OnFilledCell -= OnFilledCell;
            EventManager.OnSwapCompleted -= OnSwapCompleted;

        }     
        private void OnGridInitialized(Grid grid)
        {
            _grid = grid;
            SwapManager.Initialize(_grid);
            MatchManager.Initialize(_grid);
            FillManager.Initialize(_grid);
            FallManager.Initialize(_grid);
        }

        private void OnFilledCell(int prevRow, int prevCol, Cell cell)
        {
            _grid.SetCell(prevRow, prevCol, null); // Clear old position
            _grid.SetCell(cell.Row, cell.Col, cell); // Set the cell in its position
        }
        private void OnCellDestroyed(Cell cell)
        {
            _grid.SetCell(cell.Row, cell.Col,null);
        }
        
        private void OnSwapCompleted(Cell swappedFirstCell, Cell swappedSecondCell)
        {
            bool anyMatchFound = false;
            bool isAnyActivated = false;

            anyMatchFound |= MatchManager.GetMatch(swappedFirstCell) is not null;
            anyMatchFound |= MatchManager.GetMatch(swappedSecondCell) is not null;


            isAnyActivated |= ActivationManager.ActivateCell(swappedFirstCell);
            isAnyActivated |= ActivationManager.ActivateCell(swappedSecondCell);

            if (!anyMatchFound && !isAnyActivated)
            {
                SwapManager.RevertSwap();
            }
            else
            {
                MatchManager.ClearAllValidMatches();
                FillManager.StartFilling();
            }


            _firstSelectedCell = null;
            _secondSelectedCell = null;
        }

        private void OnPointerEnterCellEvent(Cell cell)
        {
            if (!SwapManager.CanSwap()) return;
            if (!_firstSelectedCell || _firstSelectedCell == cell) return;
            if (cell is not ISwappable || !AreCellsAdjacent(_firstSelectedCell, cell)) return;

            _secondSelectedCell = cell;
            SwapManager.Swap(_firstSelectedCell, _secondSelectedCell);
        }

        private void OnPointerClickedCellEvent(Cell cell)
        {
            ActivationManager.ActivateCell(cell);
        }
        private void OnPointerDownCellEvent(Cell cell)
        {
            if (cell is ISwappable)
            {
                _firstSelectedCell = cell;
            }
        }
        
        private void OnPointerUpCellEvent(Cell cell)
        {
            _firstSelectedCell = null;
            _secondSelectedCell = null;
        }

        private bool AreCellsAdjacent(Cell cell1, Cell cell2)
        {
            int dx = Mathf.Abs(cell1.Row - cell2.Row);
            int dy = Mathf.Abs(cell1.Col - cell2.Col);
            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1); // Ensure adjacent swap only
        }
    }
}
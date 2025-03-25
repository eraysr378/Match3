using System;
using System.Collections.Generic;
using Cells;
using GridRelated;
using OperationTrackers;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class FillManager : MonoBehaviour
    {
        public static event Action OnFillStarted;
        public static event Action OnFillCompleted;
        [SerializeField] private float _fillTime = 0.25f;
        [SerializeField] private int _activeFillables = 0;
        [SerializeField] private List<Fillable> _fillables = new ();
        private Grid _grid;
        private bool _isFilling;
        private FillTracker _fillTracker;

        private void Awake()
        {
            _fillTracker = GetComponent<FillTracker>();
        }

        public void Initialize(Grid grid)
        {
            _grid = grid;
        }

        private void OnEnable()
        {
            _fillTracker.OnAllOperationsCompleted += StartFilling;
            GridInitializer.OnGridInitialized += Initialize;

        }

        private void OnDisable()
        {
            _fillTracker.OnAllOperationsCompleted -= StartFilling;
            GridInitializer.OnGridInitialized -= Initialize;

        }

        private void StartFilling()
        {
            if (_isFilling)
                return;

            _isFilling = true;
            // Debug.Log("Fill started");
            OnFillStarted?.Invoke();
            ProcessFillStep();
        }
        

        private bool ProcessFillStep()
        {
            var isAnyMoved = MovePiecesDown();
            isAnyMoved |= SpawnNewPieces();
            return isAnyMoved;
        }

        private bool MovePiecesDown()
        {
            bool isAnyMoved = false;

            for (int row = 1; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Piece piece = _grid.GetCell(row, col).CurrentPiece;
                    if (piece is null || !piece.TryGetComponent<Fillable>(out var fillable)) continue;
                    if (TryMoveDown(fillable, row, col) || TryMoveDiagonally(fillable, row, col))
                    {
                        isAnyMoved = true;
                    }
                }
            }

            return isAnyMoved;
        }

        private bool TryMoveDown(Fillable fillable, int row, int col)
        {
            Cell targetCell = _grid.GetCell(row - 1, col);
            if (targetCell.CurrentPiece == null)
            {
                StartFillingPiece(fillable, targetCell);
                return true;
            }

            return false;
        }

        private bool TryMoveDiagonally(Fillable fillable, int row, int col)
        {
            for (int offset = -1; offset <= 1; offset += 2) // -1 (left), +1 (right)
            {
                int diagCol = col + offset;
                if (diagCol < 0 || diagCol >= _grid.Width) continue;

                Cell targetCell = _grid.GetCell(row - 1, diagCol);
                if (targetCell.CurrentPiece != null) continue;

                if (IsPathBlockedAbove(row, diagCol))
                {
                    StartFillingPiece(fillable, targetCell);
                    return true;
                }
            }

            return false;
        }

        private bool IsPathBlockedAbove(int row, int col)
        {
            for (int aboveRow = row; aboveRow < _grid.Height; aboveRow++)
            {
                Piece pieceAbove = _grid.GetCell(aboveRow, col).CurrentPiece;
                if (pieceAbove != null && !pieceAbove.TryGetComponent<Fillable>(out _))
                    return true;
            }
            return false;
        }


        private bool SpawnNewPieces()
        {
            bool isSpawned = false;

            for (int col = 0; col < _grid.Width; col++)
            {
                Cell topCell = _grid.GetCell(_grid.Height - 1, col);
                if (topCell.CurrentPiece != null) continue;

                Piece newPiece = EventManager.OnRandomNormalPieceSpawnRequested(_grid.Height, col);
                newPiece.TryGetComponent<Fillable>(out var fillable);

                StartFillingPiece(fillable, topCell);

                isSpawned = true;
            }

            return isSpawned;
        }

        private void StartFillingPiece(Fillable fillable, Cell targetCell)
        {
            _activeFillables++;
            _fillables.Add(fillable);
            fillable.OnFilled -= OnPieceFilled;
            fillable.OnFilled += OnPieceFilled;
            fillable.Fill(targetCell, _fillTime);
        }

        private void OnPieceFilled(Fillable fillable)
        {
            _activeFillables--;
            _fillables.Remove(fillable);
            fillable.OnFilled -= OnPieceFilled;
            if (_activeFillables == 0)
            {
                if (!ProcessFillStep())
                {
                    _isFilling = false;
                    // Debug.Log("fill completed");
                    OnFillCompleted?.Invoke();
                }
            }

            if (_activeFillables < 0)
            {
                Debug.LogError("active fillables is negative");
            }
        }
    }
}
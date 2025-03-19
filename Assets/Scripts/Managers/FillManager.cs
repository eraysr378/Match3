using System.Collections;
using Cells;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class FillManager : MonoBehaviour
    {
        public bool IsFilling => _isFilling;
        public float fillTime = 0.25f;
        private Grid _grid;
        private bool _isFilling;

        public void Initialize(Grid grid)
        {
            _grid = grid;
            fillTime = 0.15f;
        }

        private void OnEnable()
        {
            EventManager.OnValidMatchCleared += StartFilling;
        }

        private void OnDisable()
        {
            EventManager.OnValidMatchCleared -= StartFilling;
        }


        public void StartFilling()
        {
            // Prevent multiple filling processes
            if (_isFilling)
                return;

            _isFilling = true;
            StartCoroutine(FillIE());
        }

        private IEnumerator FillIE()
        {
            yield return new WaitForSeconds(fillTime);

            while (FillStep())
            {
                yield return new WaitForSeconds(fillTime);
            }

            _isFilling = false;
            EventManager.OnFillCompleted?.Invoke();
        }

        private bool FillStep()
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
                fillable.Fill(targetCell, fillTime);
                return true;
            }

            return false;
        }

        private bool TryMoveDiagonally(Fillable fillable, int row, int col)
        {
            int[] diagonalOffsets = { -1, 1 };

            foreach (int offset in diagonalOffsets)
            {
                int diagCol = col + offset;
                if (diagCol < 0 || diagCol >= _grid.Width) continue;

                Cell targetCell = _grid.GetCell(row - 1, diagCol);
                if (targetCell.CurrentPiece != null) continue;

                if (IsPathBlockedAbove(row, diagCol))
                {
                    fillable.Fill(targetCell, fillTime);
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

                if (pieceAbove == null) continue;

                if (!pieceAbove.TryGetComponent<Fillable>(out _))
                {
                    return true;
                }
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

                Piece newPiece = EventManager.OnRequestRandomNormalPieceSpawn(_grid.Height, col);
                newPiece.TryGetComponent<Fillable>(out var fillable);
                fillable.Fill(topCell, fillTime);
                isSpawned = true;
            }

            return isSpawned;
        }
    }
}
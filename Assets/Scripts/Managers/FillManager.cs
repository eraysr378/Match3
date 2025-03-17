using System.Collections;
using Cells;
using GridRelated;
using Pieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Managers
{
    public class FillManager : MonoBehaviour
    {
        public float fillTime = 0.25f;
        private Grid _grid;
        private Coroutine _fillingCoroutine;

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
            if (_fillingCoroutine != null)
            {
                StopCoroutine(_fillingCoroutine);
            }

            _fillingCoroutine = StartCoroutine(FillIE());
        }

        private IEnumerator FillIE()
        {
            yield return new WaitForSeconds(fillTime);

            while (FillStep())
            {
                yield return new WaitForSeconds(fillTime);
            }
            
            EventManager.OnFillCompleted?.Invoke();
        }

        private bool FillStep()
        {
            bool movedCell = false;
            for (int row = 1; row < _grid.Height; row++)
            {
                for (int col = 0; col < _grid.Width; col++)
                {
                    Piece piece = _grid.GetCell(row, col).CurrentPiece;
                    if (piece is null || !piece.TryGetComponent<Fillable>(out var fillable)) continue;
                    Piece pieceBelow = _grid.GetCell(row - 1, col).CurrentPiece;
                    if (pieceBelow is null)
                    {
                        Cell targetCell = _grid.GetCell(row - 1, col);
                        fillable.Fill(targetCell, fillTime);
                        movedCell = true;
                    }
                    else
                    {
                        for (int diag = -1; diag <= 1; diag++)
                        {
                            if (diag == 0) continue;
                            int diagCol = col + diag;
                            if (diagCol < 0 || diagCol >= _grid.Width) continue;

                            Piece diagonalPiece = _grid.GetCell(row - 1, diagCol).CurrentPiece;
                            if (diagonalPiece is not null) continue;
                            bool hasCellAbove = true;
                            for (int aboveRow = row; aboveRow < _grid.Height; aboveRow++)
                            {
                                Piece pieceAbove = _grid.GetCell(aboveRow, diagCol).CurrentPiece;
                                if (pieceAbove is not null && pieceAbove.GetComponent<Fillable>()) break;
                                if (pieceAbove is not null)
                                {
                                    hasCellAbove = false;
                                    break;
                                }
                            }

                            if (!hasCellAbove)
                            {
                                Cell targetCell = _grid.GetCell(row - 1, diagCol);
                                fillable.Fill(targetCell, fillTime);
                                movedCell = true;
                                break;
                            }
                        }
                    }
                }
            }

            for (int col = 0; col < _grid.Width; col++)
            {
                Piece pieceBelow = _grid.GetCell(_grid.Height - 1, col).CurrentPiece;
                if (pieceBelow is not null) continue;
                Piece newPiece = EventManager.OnRequestRandomNormalCellSpawn(_grid.Height, col);
                newPiece.TryGetComponent<Fillable>(out var fillable);
                Cell targetCell = _grid.GetCell(_grid.Height - 1, col);
                newPiece.Init(newPiece.transform.position,GridUtility.PropertiesSo.elementSize,null,targetCell);

                fillable.Fill(targetCell, fillTime);
                movedCell = true;
            }

            return movedCell;
        }
    }
}
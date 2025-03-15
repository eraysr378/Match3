using System.Collections;
using Cells;
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
                    Cell cell = _grid.GetCell(row, col);
                    if (cell is null || !cell.TryGetComponent<Fillable>(out var fillable)) continue;
                    Cell cellBelow = _grid.GetCell(row - 1, col);
                    if (cellBelow is null)
                    {
                        fillable.Fill(row - 1, col, fillTime);
                        EventManager.OnFilledCell?.Invoke(row, col, cell);
                        movedCell = true;
                    }
                    else
                    {
                        for (int diag = -1; diag <= 1; diag++)
                        {
                            if (diag == 0) continue;
                            int diagCol = col + diag;
                            if (diagCol < 0 || diagCol >= _grid.Width) continue;

                            Cell diagonalCell = _grid.GetCell(row - 1, diagCol);
                            if (diagonalCell is not null) continue;
                            bool hasCellAbove = true;
                            for (int aboveRow = row; aboveRow < _grid.Height; aboveRow++)
                            {
                                Cell cellAbove = _grid.GetCell(aboveRow, diagCol);
                                if (cellAbove is not null && cellAbove.GetComponent<Fillable>()) break;
                                if (cellAbove is not null)
                                {
                                    hasCellAbove = false;
                                    break;
                                }
                            }

                            if (!hasCellAbove)
                            {
                                fillable.Fill(row - 1, diagCol, fillTime);
                                EventManager.OnFilledCell?.Invoke(row, col, cell);
                                movedCell = true;
                                break;
                            }
                        }
                    }
                }
            }

            for (int col = 0; col < _grid.Width; col++)
            {
                Cell cellBelow = _grid.GetCell(_grid.Height - 1, col);
                if (cellBelow is not null) continue;
                Cell newCell = EventManager.OnRequestRandomCellSpawn(_grid.Height, col);
                newCell.TryGetComponent<Fillable>(out var fillable);
                fillable.Fill(_grid.Height - 1, col, fillTime);
                EventManager.OnFilledCell?.Invoke(newCell.Row, newCell.Col, newCell);
                movedCell = true;
            }

            return movedCell;
        }
    }
}
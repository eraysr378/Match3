using System;
using System.Collections.Generic;
using Cells;
using GridRelated;
using TileRelated;
using Unity.VisualScripting;
using UnityEngine;
using Grid = GridRelated.Grid;
using Random = UnityEngine.Random;

namespace Pieces.Behaviors
{
    public class FillNotifier : MonoBehaviour
    {
        private GridRelated.Grid _grid;
        
        private void OnEnable()
        {
            Cell.OnAnyRequestFill += OnAnyCellCleared;
            TilemapLoader.OnCellsCreated += GridInitializerOnGridInitialized;

        }
        private void OnDisable()
        {
            Cell.OnAnyRequestFill -= OnAnyCellCleared;
            TilemapLoader.OnCellsCreated -= GridInitializerOnGridInitialized;

        }
        private void GridInitializerOnGridInitialized(Grid obj)
        {
            _grid = obj;
        }

        private void OnAnyCellCleared(Cell clearedCell)
        {
            if (clearedCell == null) return;
            if (clearedCell.CurrentPiece == null)
            {
                NotifyAbove(clearedCell);
                return;
            }

            if (clearedCell.CurrentPiece.TryGetComponent<FillHandler>(out var fillable))
            {
                fillable.TryStartFill();
            }
        }
        private void NotifyAbove(Cell clearedCell)
        {
            int startRow = clearedCell.Row + 1;
            int col = clearedCell.Col;
            Cell aboveCell = _grid.GetCellAt(startRow, col);
            Cell leftAboveCell = _grid.GetCellAt(startRow, col-1);
            Cell rightAboveCell = _grid.GetCellAt(startRow, col+1);
            OnAnyCellCleared(aboveCell);
            // If above is blocked then cell should be filled diagonally, make it filled from left or right randomly.
            if (Random.Range(0, 2) == 0)
            {
                OnAnyCellCleared(leftAboveCell);
                OnAnyCellCleared(rightAboveCell);
            }
            else
            {
                OnAnyCellCleared(rightAboveCell);
                OnAnyCellCleared(leftAboveCell);
            }
            
        }
    }
}
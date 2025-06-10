using System;
using System.Collections.Generic;
using BuildSystem;
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
            BaseCell.OnAnyRequestFill += OnAnyCellCleared;
            GridBuilder.OnCellsCreated += GridInitializerOnGridInitialized;

        }
        private void OnDisable()
        {
            BaseCell.OnAnyRequestFill -= OnAnyCellCleared;
            GridBuilder.OnCellsCreated -= GridInitializerOnGridInitialized;

        }
        private void GridInitializerOnGridInitialized(Grid obj)
        {
            _grid = obj;
        }

        private void OnAnyCellCleared(BaseCell clearedBaseCell)
        {
            if (clearedBaseCell == null) return;
            if (clearedBaseCell.CurrentPiece == null)
            {
                NotifyAbove(clearedBaseCell);
                return;
            }

            if (clearedBaseCell.CurrentPiece.TryGetComponent<FillHandler>(out var fillable))
            {
                fillable.TryStartFill();
            }
        }
        private void NotifyAbove(BaseCell clearedBaseCell)
        {
            int startRow = clearedBaseCell.Row + 1;
            int col = clearedBaseCell.Col;
            BaseCell aboveBaseCell = _grid.GetCellAt(startRow, col);
            BaseCell leftAboveBaseCell = _grid.GetCellAt(startRow, col-1);
            BaseCell rightAboveBaseCell = _grid.GetCellAt(startRow, col+1);
            OnAnyCellCleared(aboveBaseCell);
            // If above is blocked then cell should be filled diagonally, make it filled from left or right randomly.
            if (Random.Range(0, 2) == 0)
            {
                OnAnyCellCleared(leftAboveBaseCell);
                OnAnyCellCleared(rightAboveBaseCell);
            }
            else
            {
                OnAnyCellCleared(rightAboveBaseCell);
                OnAnyCellCleared(leftAboveBaseCell);
            }
            
        }
    }
}
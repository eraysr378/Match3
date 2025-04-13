using System;
using System.Collections.Generic;
using Cells;
using GridRelated;
using Unity.VisualScripting;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Pieces.Behaviors
{
    public class FillNotifier : MonoBehaviour
    {
        private GridRelated.Grid _grid;

        private void Awake()
        {
            GridInitializer.OnGridInitialized += GridInitializerOnGridInitialized;
        }

        private void OnEnable()
        {
            Cell.OnRequestFill += OnCellCleared;
        }
        private void OnDisable()
        {
            Cell.OnRequestFill -= OnCellCleared;
        }
        private void GridInitializerOnGridInitialized(Grid obj)
        {
            _grid = obj;
        }

        private void OnCellCleared(Cell clearedCell)
        {
            if (clearedCell == null)
            {
                return;
            }
            if (clearedCell.CurrentPiece == null)
            {
                NotifyAbove(clearedCell);
                return;
            }

            if (clearedCell.CurrentPiece.TryGetComponent<Fillable>(out var fillable))
            {
                fillable.StartFill();
            }
        }
        private void NotifyAbove(Cell clearedCell)
        {
            int startRow = clearedCell.Row + 1;
            int col = clearedCell.Col;
            Cell aboveCell = _grid.GetCellAt(startRow, col);
            OnCellCleared(aboveCell);
            
        }
    }
}
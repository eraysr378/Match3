using System;
using System.Collections;
using Cells;
using Managers;
using Pieces;
using Pieces.Behaviors;
using UnityEngine;

namespace Managers
{
    public class FallManager : MonoBehaviour
    {

        private void OnEnable()
        {
            Cell.OnRequestFall += HandlePieceFall;
        }

        private void OnDisable()
        {
            Cell.OnRequestFall -= HandlePieceFall;

        }

        private void HandlePieceFall(Cell cell)
        {

            if (cell.CurrentPiece == null)
            {
                SpawnNewPiece(cell);
            }
        }

        private void SpawnNewPiece(Cell cell)
        {
            
            Piece newPiece = EventManager.OnRandomNormalPieceSpawnRequested(cell.Row+1, cell.Col);
            newPiece.SetCell(cell);
            newPiece.TryGetComponent<Fillable>(out var fillable);
            fillable.Fall();
            // fillable.fill

        }
    }
}
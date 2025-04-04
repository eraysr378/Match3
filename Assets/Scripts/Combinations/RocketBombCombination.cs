using Cells;
using Managers;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Pieces.CombinationPieces
{
    public class RocketBombCombination : Combination
    {
        public override void ExecuteEffect(int row,int col)
        {
            
            SpawnRockets(row,col);
            CompleteCombination();
        }

        protected override void CompleteCombination()
        {
            base.CompleteCombination();
            // DestroyPieceInstantly();

        }
        private void SpawnRocket(int row, int col, bool isHorizontal)
        {
            Grid grid = GridManager.Instance.Grid;
            Cell referenceCell = grid.GetCell(0, 0); // will be used just to resize rocket

            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);
            piece.transform.SetParent(referenceCell.transform); 
            piece.transform.localScale = new Vector3(1, 1, 1);
            if (piece is not RocketPiece rocketPiece)
                return;
            
            if (isHorizontal) rocketPiece.SetHorizontal();
            else rocketPiece.SetVertical();

            rocketPiece.Activate();
        }

        private void SpawnRockets(int row,int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                SpawnRocket(row + i, col, true);  // Horizontal
                SpawnRocket(row, col + i, false); // Vertical
            }
        }

        // private void SpawnRockets()
        // {
        //     Grid grid = GridManager.Instance.Grid;
        //     int row = Row;
        //     int col  = Col;
        //     for (int i = -1; i <= 1; i++)
        //     {
        //         Cell targetCell = grid.GetCell(row + i, col);
        //         if (targetCell != null)
        //         {
        //             Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row + i, col);
        //             piece.transform.SetParent(targetCell.transform); // just to resize rocket
        //             piece.transform.localScale = new Vector3(1, 1, 1);
        //             
        //             RocketPiece rocketPiece = piece as RocketPiece;
        //             rocketPiece.SetHorizontal();
        //             rocketPiece.Activate();
        //         }
        //     }
        //
        //     for (int i = -1; i <= 1; i++)
        //     {
        //         Cell targetCell = grid.GetCell(row, col + i);
        //         if (targetCell != null)
        //         {
        //             Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col + i);
        //             piece.transform.SetParent(targetCell.transform); // just to resize rocket
        //             piece.transform.localScale = new Vector3(1, 1, 1);
        //
        //             RocketPiece rocketPiece = piece as RocketPiece;
        //             rocketPiece.SetVertical();
        //             rocketPiece.Activate();
        //         }
        //     }
        // }
    }
}
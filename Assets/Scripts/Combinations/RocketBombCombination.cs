using Cells;
using Managers;
using Misc;
using Pieces;
using Pieces.SpecialPieces;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Combinations
{
    public class RocketBombCombination : BaseCombination
    {
        protected override void ActivateCombination(int row,int col)
        {
            
            SpawnRockets(row,col);
            CompleteCombination();
        }

        protected override void CompleteCombination()
        {
            base.CompleteCombination();
        }
        private void SpawnRocket(int row, int col, bool isHorizontal)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);
   
            if (piece is not RocketPiece rocketPiece)
                return;
            
            if (isHorizontal) rocketPiece.SetHorizontal();
            else rocketPiece.SetVertical();
            Cell cellToSpawn = GridManager.Instance.GetCellAt(row, col);
            rocketPiece.ActivateAt(cellToSpawn);
        }

        private void SpawnRockets(int row,int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                SpawnRocket(row + i, col, true);  // Horizontal
                SpawnRocket(row, col + i, false); // Vertical
            }
        }
        
    }
}
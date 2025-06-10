using Cells;
using Managers;
using Misc;
using Pieces;
using Pieces.SpecialPieces;
using UnityEngine;

namespace Combinations
{
    public class RocketRocketCombination : BaseCombination
    {
        protected override void ActivateCombination(int row,int col)
        {
            SpawnRocket(row, col, false);
            SpawnRocket(row, col, true);
            CompleteCombination();
        }
        
        private void SpawnRocket(int row, int col, bool isHorizontal)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);

            if (piece is not RocketPiece rocketPiece)
            {
                Debug.Log("Couldn't spawn rocket piece");
                return;
            }
            
            if (isHorizontal) rocketPiece.SetHorizontal();
            else rocketPiece.SetVertical();

            BaseCell baseCellToSpawn = GridManager.Instance.GetCellAt(row, col);
            rocketPiece.ActivateAt(baseCellToSpawn);        }
    }
}
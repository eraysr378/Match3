using Cells;
using Managers;
using Misc;
using UnityEngine;
using Grid = GridRelated.Grid;

namespace Pieces.CombinationPieces
{
    public class RocketRocketCombination : Combination
    {
        public override void ExecuteEffect(int row,int col)
        {
            SpawnRocket(row, col, false);
            SpawnRocket(row, col, true);
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
    }
}
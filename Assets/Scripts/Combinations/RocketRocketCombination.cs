using Cells;
using Managers;
using Misc;
using Pieces;

namespace Combinations
{
    public class RocketRocketCombination : Combination
    {
        protected override void ExecuteEffect(int row,int col)
        {
            SpawnRocket(row, col, false);
            SpawnRocket(row, col, true);
            CompleteCombination();
        }

        protected override void CompleteCombination()
        {
            base.CompleteCombination();
            DestroySelf();
        }
        private void SpawnRocket(int row, int col, bool isHorizontal)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);

            if (piece is not RocketPiece rocketPiece)
                return;
            
            if (isHorizontal) rocketPiece.SetHorizontal();
            else rocketPiece.SetVertical();

            rocketPiece.Activate();
        }
    }
}
using Managers;
using Misc;
using Pieces;

namespace Combinations
{
    public class RainbowBombCombination : RainbowSpecialPieceCombination<BombPiece>
    {
        protected override BombPiece SpawnSpecialPiece(int row, int col)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.BombPiece, row, col);
            return piece as BombPiece;
        }
    }
}
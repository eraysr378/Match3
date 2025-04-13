using Managers;
using Misc;
using Pieces;
using Pieces.SpecialPieces;

namespace Combinations
{
    public class RainbowBombCombination : BaseRainbowSpecialPieceCombination<BombPiece>
    {
        protected override BombPiece SpawnSpecialPiece(int row, int col)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.BombPiece, row, col);
            return piece as BombPiece;
        }
    }
}
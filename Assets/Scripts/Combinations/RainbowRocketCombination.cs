using Managers;
using Misc;
using Pieces;
using Pieces.SpecialPieces;

namespace Combinations
{
    public class RainbowRocketPieceCombination : RainbowSpecialPieceCombination<RocketPiece>
    {
        protected override RocketPiece SpawnSpecialPiece(int row, int col)
        {
            Piece piece = EventManager.OnPieceSpawnRequested?.Invoke(PieceType.RocketPiece, row, col);
            return piece as RocketPiece;
        }
    }
}